using SQLite;
using System;

namespace CrossBankScraperApp.Models
{
    public class UserDTO : IEquatable<UserDTO>
    {
        [PrimaryKey, AutoIncrement]
        private int Id { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Agency { get; set; }
        public string Balance { get; set; }
        
        public bool Equals(UserDTO other) =>
            Account == other.Account;

        public override bool Equals(object obj)
        {
            var other = obj as UserDTO;
            if (other == null)
                return false;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Id}:{Name}:{Account}:{Agency}:{Balance}";
        }

        public static bool operator ==(UserDTO u1, UserDTO u2) =>
            u1.Equals(u2);

        public static bool operator !=(UserDTO u1, UserDTO u2) =>
            !u1.Equals(u2);
    }
}