using BankScraper.Enums;
using BankScraper.Helpers;
using BankScraper.Interfaces;
using BankScraper.Models;
using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BankScraper.Banks
{
    internal class Intermedium : IBank
    {
        private const string BASE_URL = "https://internetbanking.bancointer.com.br";
        private readonly WebNavigator _webNavigator;
        private readonly HtmlDocument _htmlDocument;
        private string _password;
        private string _userAccount;
        private string _viewStatePrimary;
        private string _viewStateSecundary;

        public BankFlag Flag
        {
            get { return BankFlag.Intermedium; }
        }

        #region Constructors

        public Intermedium()
        {
            _htmlDocument = new HtmlDocument();
            _webNavigator = new WebNavigator();
        }

        #endregion

        #region Public Actions

        public async Task<bool> LoginAsync(string userOrAccount, string password)
        {
            try
            {
                _userAccount = userOrAccount.Replace("-", string.Empty);
                _password = password;

                var responseString = await InitializeCookiesAsync();

                responseString = await PostUserAccountAsync(responseString);
                responseString = await OpenVirtualKeyboardAsync(responseString);
                responseString = await PostPasswordAsync(responseString);

                return await GetHomePageAsync(responseString);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Login error on Intermedium bank.\nDetails:{ex.Message}");
                return false;
            }
        }
        
        public async Task<UserDetails> GetUserDetailsAsync()
        {
            if (_htmlDocument == null)
                throw new UnauthorizedAccessException("Do login first!");

            var nodeCollection = _htmlDocument.DocumentNode?.Descendants("span")?.ToList();
            var bankData = nodeCollection?
                .FirstOrDefault(s => s.InnerText.Contains("Agência:"));

            var bankDataStrings = bankData.InnerText
                .Replace("\n", "").Replace("\t", "")
                .Replace(" ","").Split('/');

            var agency = bankDataStrings[0].Substring(8, 4);
            var account = bankDataStrings[1].Substring(6);
            var userName = nodeCollection[nodeCollection.IndexOf(bankData) - 1]
                .InnerText.Replace("\n", "").Replace("\t", "");

            var balance = await GetUserBalanceAsync();

            return new UserDetails
            {
                Name = userName,
                Account = account,
                Agency = agency,
                Balance = balance
            };
        }

        #endregion

        #region Local Actions

        private async Task<string> InitializeCookiesAsync()
        {
            _webNavigator.AditionalHeaders.Add("Faces-request", "partial/ajax");
            _webNavigator.AditionalHeaders.Add("Origin", BASE_URL);
            _webNavigator.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            _webNavigator.Referer = $"{BASE_URL}/login.jsf";
            _webNavigator.Method = "GET";
            _webNavigator.Accept = "*/*";

            return await _webNavigator.NavigateAsync($"{BASE_URL}/login.jsf");
        }

        private async Task<string> PostUserAccountAsync(string responseString)
        {
            _viewStatePrimary = responseString.Substring(responseString.LastIndexOf("![CDATA[") + 8);
            _viewStateSecundary = _viewStatePrimary.Substring(_viewStatePrimary.IndexOf(":") + 1);

            _viewStateSecundary = _viewStateSecundary.Remove(_viewStateSecundary.IndexOf("]"));
            _viewStatePrimary = _viewStatePrimary.Remove(_viewStatePrimary.IndexOf(":"));

            _webNavigator.AditionalHeaders.Add("Faces-Request", "partial/ajax");
            _webNavigator.AditionalHeaders.Add("Origin", $"{BASE_URL}");
            _webNavigator.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            _webNavigator.Referer = $"{BASE_URL}/login.jsf";
            _webNavigator.Method = "POST";
            _webNavigator.Accept = "*/*";

            _webNavigator.PostData = $"frmLogin=frmLogin&javax.faces.ViewState={_viewStatePrimary}%3A{_viewStateSecundary}" +
                $"&j_idt21=CLIENTE_RENDA_FIXA&loginv20170605={_userAccount}" +
                "&javax.faces.source=loginv20170605&javax.faces.partial.event=change" +
                "&javax.faces.partial.execute=loginv20170605%20loginv20170605&javax.faces.partial.render=loginv20170605" +
                "&javax.faces.behavior.event=valueChange&javax.faces.partial.ajax=true";

            responseString =  await _webNavigator.NavigateAsync($"{BASE_URL}/login.jsf");

            _htmlDocument.LoadHtml(await WebNavigator.GetHtmlFrom(BASE_URL));

            var collection = _htmlDocument.DocumentNode.Descendants()
                .Where(descendant => descendant.Name.Equals("input", StringComparison.OrdinalIgnoreCase));

            var button = collection.FirstOrDefault(
                node => node.GetAttributeValue("value", string.Empty)
                .Equals("Avançar", StringComparison.OrdinalIgnoreCase))
                .GetAttributeValue("name", string.Empty);

            var comboBox = _htmlDocument.DocumentNode.Descendants()
                .FirstOrDefault(descendant => descendant.Name.Equals("select", StringComparison.OrdinalIgnoreCase));

            _viewStatePrimary = responseString.Substring(responseString.LastIndexOf("![CDATA[") + 8);
            _viewStateSecundary = _viewStatePrimary.Substring(_viewStatePrimary.IndexOf(":") + 1);

            _viewStateSecundary = _viewStateSecundary.Remove(_viewStateSecundary.IndexOf("]"));
            _viewStatePrimary = _viewStatePrimary.Remove(_viewStatePrimary.IndexOf(":"));

            _webNavigator.PostData = $"frmLogin=frmLogin&javax.faces.ViewState={_viewStatePrimary}%3A{_viewStateSecundary}" +
                $"&{comboBox.Id}=CLIENTE_RENDA_FIXA&loginv20170605={_userAccount}&{button}=Aguarde...";

            _webNavigator.AditionalHeaders.Add("Cache-Control", "max-age=0");
            _webNavigator.AditionalHeaders.Add("Upgrade-Insecure-Requests", "1");
            _webNavigator.AditionalHeaders.Add("Origin", BASE_URL);
            _webNavigator.Method = "POST";
            _webNavigator.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            _webNavigator.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            _webNavigator.Referer = $"{BASE_URL}/login.jsf";

            return await _webNavigator.NavigateAsync($"{BASE_URL}/login.jsf");
        }

        private async Task<string> OpenVirtualKeyboardAsync(string responseString)
        {
            _htmlDocument.LoadHtml(responseString);

            var htmlNodes = _htmlDocument.DocumentNode.Descendants()
                    .Where(descendant => descendant.Name.Equals("a", StringComparison.OrdinalIgnoreCase));

            var userConfirmation = htmlNodes.FirstOrDefault(
                node => node.GetAttributeValue("onclick", string.Empty)
                .Contains("mojarra.ab"));

            _viewStatePrimary = responseString.Substring(responseString.LastIndexOf("id=\"javax.faces.ViewState\" value=\"") + 34);
            _viewStateSecundary = _viewStatePrimary.Substring(_viewStatePrimary.IndexOf(":") + 1);

            _viewStateSecundary = _viewStateSecundary.Remove(_viewStateSecundary.IndexOf("\""));
            _viewStatePrimary = _viewStatePrimary.Remove(_viewStatePrimary.IndexOf(":"));

            _webNavigator.PostData = $"frmLogin=frmLogin&javax.faces.ViewState={_viewStatePrimary}%3A{_viewStateSecundary}&loginv20170605={_userAccount}" +
                $"&javax.faces.source={userConfirmation.Id}&javax.faces.partial.event=click&javax.faces.partial.execute={userConfirmation.Id}%20panelGeralv20170605" +
                $"&javax.faces.partial.render=panelGeralv20170605&javax.faces.behavior.event=action&javax.faces.partial.ajax=true";

            _webNavigator.AditionalHeaders.Add("Faces-Request", "partial/ajax");
            _webNavigator.AditionalHeaders.Add("Origin", BASE_URL);
            _webNavigator.Accept = "*/*";
            _webNavigator.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            _webNavigator.Referer = $"{BASE_URL}/login.jsf";
            _webNavigator.Method = "POST";

            return await _webNavigator.NavigateAsync($"{BASE_URL}/login.jsf");
        }

        private async Task<string> PostPasswordAsync(string responseString)
        {
            var passwordArray = _password.ToCharArray();

            foreach (var character in passwordArray)
            {
                _htmlDocument.LoadHtml(responseString);

                var passwordKeyboard = _htmlDocument.GetElementbyId("panelTeclado")
                    .InnerHtml.Replace("]]>", "</div>").Replace("<![CDATA[", "");

                _htmlDocument.LoadHtml(passwordKeyboard);

                var buttonCollection = _htmlDocument.DocumentNode.Descendants()
                    .Where(descendant => descendant.Name.Equals("input", StringComparison.OrdinalIgnoreCase));

                var charButton = buttonCollection.FirstOrDefault(
                    node => node.GetAttributeValue("value", string.Empty)
                    .Equals(character.ToString(), StringComparison.OrdinalIgnoreCase));

                var buttonName = charButton.Id.Replace(":", "%3A");

                _webNavigator.PostData = $"frmLogin=frmLogin&javax.faces.ViewState={_viewStatePrimary}%3A{_viewStateSecundary}&loginv20170605={_userAccount}" +
                    $"&javax.faces.source={buttonName}" +
                    $"&javax.faces.partial.event=click&javax.faces.partial.execute={buttonName}" +
                    $"%20{buttonName}&javax.faces.partial.render=panelTeclado" +
                    $"&javax.faces.behavior.event=action&javax.faces.partial.ajax=true";

                _webNavigator.AditionalHeaders.Add("Faces-Request", "partial/ajax");
                _webNavigator.AditionalHeaders.Add("Origin", BASE_URL);
                _webNavigator.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                _webNavigator.Accept = "*/*";
                _webNavigator.Method = "POST";
                _webNavigator.Referer = $"{BASE_URL}/login.jsf";

                responseString = await _webNavigator.NavigateAsync($"{BASE_URL}/login.jsf");
            }

            _htmlDocument.LoadHtml(responseString);

            var keyboardPanel = _htmlDocument.GetElementbyId("panelTeclado")
                .InnerHtml.Replace("]]>", "</div>").Replace("<![CDATA[", "");

            _htmlDocument.LoadHtml(keyboardPanel);

            var collection = _htmlDocument.DocumentNode.Descendants()
                .Where(descendant => descendant.Name.Equals("input", StringComparison.OrdinalIgnoreCase));

            var button = collection.FirstOrDefault(
                node => node.GetAttributeValue("value", string.Empty).ToString()
                .Equals("CONFIRMAR", StringComparison.OrdinalIgnoreCase))
                .Id;

            _webNavigator.PostData = $"frmLogin=frmLogin&javax.faces.ViewState={_viewStatePrimary}%3A{_viewStateSecundary}&loginv20170605={_userAccount}" +
                $"&javax.faces.source={button}&javax.faces.partial.event=click&javax.faces.partial.execute={button}%20{button}" +
                $"&javax.faces.partial.render=frmLogin&javax.faces.behavior.event=action&javax.faces.partial.ajax=true";

            _webNavigator.AditionalHeaders.Add("Faces-Request", "partial/ajax");
            _webNavigator.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            _webNavigator.AditionalHeaders.Add("Origin", BASE_URL);
            _webNavigator.Referer = $"{BASE_URL}/login.jsf";
            _webNavigator.Accept = "*/*";
            _webNavigator.Method = "POST";

            return await _webNavigator.NavigateAsync($"{BASE_URL}/login.jsf");
        }

        private async Task<bool> GetHomePageAsync(string responseString)
        {
            var isLoggedIn = false;

            _webNavigator.AditionalHeaders.Add("Upgrade-Insecure-Requests", "1");
            _webNavigator.Referer = $"{BASE_URL}/login.jsf";
            _webNavigator.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            _webNavigator.Method = "GET";

            responseString = await _webNavigator.NavigateAsync($"{BASE_URL}/comum/home.jsf");

            _htmlDocument.LoadHtml(responseString);

            isLoggedIn = _htmlDocument.DocumentNode?.Descendants("span")?
                .FirstOrDefault(s => s.InnerText.Contains("Agência:")) != null;
            
            return isLoggedIn;
        }
        
        private async Task<string> GetUserBalanceAsync()
        {
            var htmlNodes = _htmlDocument.DocumentNode.Descendants()
                .Where(descendant => descendant.Name.Equals("a", StringComparison.OrdinalIgnoreCase));

            var balanceButton = htmlNodes.FirstOrDefault(
                node => node.GetAttributeValue("onclick", string.Empty)
                .Contains("mojarra.ab(this,event,'action',0,'frmSaldos')"));

            var viewState = _htmlDocument.DocumentNode
                .Descendants("input")
                .FirstOrDefault(input => input.Id.Equals("javax.faces.ViewState", StringComparison.OrdinalIgnoreCase))
                .GetAttributeValue("value", string.Empty).Split(':');

            _viewStatePrimary = viewState[0];
            _viewStateSecundary = viewState[1];

            _webNavigator.Accept = "*/*";
            _webNavigator.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            _webNavigator.AditionalHeaders.Add("Faces-Request", "partial/ajax");
            _webNavigator.AditionalHeaders.Add("Upgrade-Insecure-Requests", "1");
            _webNavigator.Referer = $"{BASE_URL}/comum/home.jsf";
            _webNavigator.AditionalHeaders.Add("Origin", BASE_URL);
            _webNavigator.Method = "POST";

            _webNavigator.PostData = $"frmSaldos=frmSaldos&javax.faces.ViewState={_viewStatePrimary}%3A{_viewStateSecundary}" +
                $"&javax.faces.source={balanceButton.Id}&javax.faces.partial.event=click&javax.faces.partial.execute={balanceButton.Id}%20{balanceButton.Id}" +
                $"&javax.faces.partial.render=frmSaldos&javax.faces.behavior.event=action&javax.faces.partial.ajax=true";

            var responseString =  await _webNavigator.NavigateAsync($"{BASE_URL}/comum/home.jsf");

            _htmlDocument.LoadHtml(responseString);

            var balancePanel = _htmlDocument.GetElementbyId("frmSaldos")
                .InnerHtml.Replace("]]>", "</div>").Replace("<![CDATA[", "");

            _htmlDocument.LoadHtml(balancePanel);

            var spanValue = _htmlDocument.DocumentNode.Descendants("span")
                .FirstOrDefault(span => span.GetAttributeValue("class", string.Empty)
                .Equals("spanValores", StringComparison.OrdinalIgnoreCase)).InnerHtml;

            var spanHtml = new HtmlDocument();
            spanHtml.LoadHtml(spanValue);

            return spanHtml.DocumentNode.InnerText.Replace("\n","").Replace("\t", "");
        }

        #endregion
    }
}
