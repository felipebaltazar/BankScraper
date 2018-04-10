using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BankScraper.Helpers
{
    public class WebNavigator
    {
        private string postData = string.Empty;
        private string contentType = string.Empty;
        private string method = string.Empty;
        private string accept = string.Empty;
        private string referer = string.Empty;
        private CookieContainer cookieContainer;
        public Dictionary<string, string> AditionalHeaders = new Dictionary<string, string>();

        public string PostData
        {
            get { return postData; }
            set { postData = value; }
        }

        public string Method
        {
            get { return method; }
            set { method = value; }
        }

        public string Accept
        {
            get { return accept; }
            set { accept = value; }
        }

        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        public string Referer
        {
            get { return referer; }
            set { referer = value; }
        }

        #region Constructors

        public WebNavigator()
        {
            cookieContainer = new CookieContainer();
        }

        #endregion

        #region Public Actions

        public async Task<string> NavigateAsync(string url)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.AllowAutoRedirect = true;

            SetRequestProperties(request);
            GenerateDefaultHeaders(request);
            
            if(!string.IsNullOrEmpty(postData))
            {
                var dataBytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = dataBytes.Length;

                using (var postStream = await request.GetRequestStreamAsync())
                    await postStream.WriteAsync(dataBytes, 0, dataBytes.Length);
            }

            using (var response = request.GetResponse())
            {
                using (var sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("iso-8859-1")))
                {
                    ClearDefaultVariables();
                    return sr.ReadToEnd();
                }
            }
        }

        public static async Task<string> GetHtmlFrom(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            using (WebClient client = new WebClient())
                return DecodeString(await client.DownloadStringTaskAsync(url));
        }

        #endregion

        #region Local Actions

        private void SetRequestProperties(HttpWebRequest request)
        {
            if (!string.IsNullOrEmpty(accept))
                request.Accept = accept;

            if (!string.IsNullOrEmpty(method))
                request.Method = method;

            if (!string.IsNullOrEmpty(referer))
                request.Referer = referer;

            if (!string.IsNullOrEmpty(contentType))
                request.ContentType = contentType;
        }

        private void GenerateDefaultHeaders(HttpWebRequest request)
        {
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None;
            request.CookieContainer = cookieContainer;
            //request.KeepAlive = true;

            request.Headers.GetType().InvokeMember(
                "ChangeInternal",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod,
                Type.DefaultBinder,
                request.Headers,
                new object[] { "Connection", "keep-alive" });

            //request.Headers.GetType().InvokeMember(
            //    "ChangeInternal",
            //    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod,
            //    Type.DefaultBinder,
            //    request.Headers,
            //    new object[] { "Accept-Encoding", "gzip, deflate, br" });

            request.Headers.Add("Accept-Language", "pt-BR,pt;q=0.9");
            
            if (AditionalHeaders.Count > 0)
            {
                foreach (var aditionalHeader in AditionalHeaders)
                    request.Headers.Add(aditionalHeader.Key, aditionalHeader.Value);
            }
        }
        
        private void ClearDefaultVariables()
        {
            postData = string.Empty;
            contentType = string.Empty;
            method = string.Empty;
            accept = string.Empty;
            referer = string.Empty;

            AditionalHeaders.Clear();
        }
        
        private static string DecodeString(string stringToDecode)
        {
            var bytes = Encoding.Default.GetBytes(stringToDecode);
            return Encoding.UTF8.GetString(bytes);
        }

        #endregion
    }
}
