using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPFProject.Classes.Queries;
using System.Data.SQLite;

namespace WPFProject.Classes.Queries
{
    public class Account : Table
    {
        private protected string _username;
        private protected string _password;

        public string Username { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }

        protected Account(string Username)
        {
            this.Username = Username;
        }
        protected Account(string Username, string Password) : this(Username)
        {
            this.Password = Password;
        }

        public class Authorize : Account
        {
            public Authorize(string Username, string Password) : base(Username, Password)
            {
                this.Query = Queries.Constants.Users.Authorize;
                this.parameters = new List<SQLiteParameter> { new SQLiteParameter("@login", Username), new SQLiteParameter("@password", Password) };
                this.isSelect = true;
            }
        }
        public class Register : Account
        {
            public Register(string Username, string Password) : base(Username, Password)
            {
                this.Query = Queries.Constants.Users.Add;
                this.parameters = new List<SQLiteParameter> { new SQLiteParameter("@login", Username), new SQLiteParameter("@password", Password) };
                this.isSelect = false;
            }
        }
        public class Search : Account
        {
            public Search(string Username) : base(Username)
            {
                this.Query = String.Format(format: Queries.Constants.Users.Search, arg0: Username);
                this.parameters = new List<SQLiteParameter> { new SQLiteParameter("@login", Username) };
                this.isSelect = true;
            }
        }
    }
}
