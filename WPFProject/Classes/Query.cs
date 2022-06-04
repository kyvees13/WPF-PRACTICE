using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using WPFProject;
using static WPFProject.Classes.Helper;

namespace WPFProject.Queries
{

    public class Table
    {
        public string TableName { get; set; }
    }

    public class Search
    {
        private string SearchTag { get; set; }
        private string SearchText { get; set; }

        public Search(string SearchTag, string SearchText) { this.SearchTag = SearchTag; this.SearchText = SearchText; }

        private string GetString
        {
            get => $"SELECT * FROM investTable WHERE {this.SearchTag} LIKE @SearchText";
        }

        private List<SQLiteParameter> GetParameters
        {
            get => new List<SQLiteParameter> { new SQLiteParameter("@SearchText", this.SearchText) };
        }

        private int Execute { get => this.ExecuteCommand(); }


    }

    public class Table
    {
        protected List<SQLiteParameter> parameters { get; set; }
    }

    public class Account
    {
        protected string Username { get; set; }
        protected string Password { get; set; }

        protected List<SQLiteParameter> parameters;

        protected Account(string Username)
        {
            this.Username = Username;
        }
        protected Account(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
    }

    public class Invest
    {
        protected RowData row { get; set; }

        protected List<SQLiteParameter> parameters;
    }
    
    public class Search : Account
    {
        public Search(string Username) : base(Username: Username)
        {
            parameters.Add(new SQLiteParameter("@login", Username));
        }

        public string GetString { get => Queries.Constants.Users.Search; }
        public List<SQLiteParameter> GetParameters { get => this.parameters; }

        public RequestStatement Execute { get => RequestStatement.Success; }
            
    }
    public class Authorize : Account
    {
        public Authorize(string Username, string Password) : base(Username, Password)
        {
            parameters.Add(new SQLiteParameter("@login", Username));
            parameters.Add(new SQLiteParameter("@password", Password));
        }

        public string GetString { get => Queries.Constants.Users.Authorize; }
        public List<SQLiteParameter> GetParameters { get => this.parameters; }

        public RequestStatement Execute { get => RequestStatement.Success; }

    }

    public static class Constants
    {


        public static class Users 
        {
            public const string Authorize = "select * from usersAcc where login=@login and password=@password";
            public const string Add = "insert into usersAcc(login,password) VALUES(@login,@password)";
            public const string Recover = "";
        }
        public static class Invest
        {
            public const string Search = "SELECT * FROM investTable WHERE {this.SearchTag} LIKE @SearchText";
            public const string Recover = "";
        }
    }


    public class Qwe
    {

        public RequestStatement SelectorStatement(int result)
        {
            switch (result)
            {
                case 1: return RequestStatement.Success;
                case 0: return RequestStatement.Warning;
                case -1: return RequestStatement.Error;

                default: return RequestStatement.Unexpected;
            }
        }

        public class Users
        {
            public class Auth
            {
                string login, password;

                public Auth(string login, string password) { this.login = login; this.password = password; }

                public RequestStatement Execute { get => RequestStatement.Success; }

                public string GetString { get => Queries.Constants.Users.Authorize; }
                public List<SQLiteParameter> GetParameters { get => new List<SQLiteParameter>
                    {
                        new SQLiteParameter("@login", this.login),
                        new SQLiteParameter("@password", this.password)
                    };
                }
            }

            public class Add
            {
                string login, password;

                public Add(string login, string password)
                {
                    this.login = login;
                    this.password = password;
                }

                public string GetString { get => Queries.Constants.Users.Add; }
                public List<SQLiteParameter> GetParameters
                {
                    get => new List<SQLiteParameter>
                    {
                        new SQLiteParameter("@login", this.login),
                        new SQLiteParameter("@password", this.password)
                    };
                }
                public int Execute { get => ExecuteCommand(GetString, GetParameters); }
            }

        }

        public class Table
        {
            public string TableName { get; set; }

            public class Search : Table
            {
                private string SearchTag { get; set; }
                private string SearchText{ get; set; }

                public Search(string SearchTag, string SearchText) { this.SearchTag = SearchTag; this.SearchText = SearchText; }

                private string GetString {
                    get => $"SELECT * FROM {this.TableName} WHERE {this.SearchTag} LIKE @SearchText"; }

                private List<SQLiteParameter> GetParameters {
                    get => new List<SQLiteParameter> { new SQLiteParameter("@SearchText", this.SearchText) }; }

                private int Execute { get => this.ExecuteCommand(); }


            }

            public class Delete
            {
                private string DeleteTag, DeleteText;

                public Delete(string DeleteTag, string DeleteText) { this.DeleteTag = DeleteTag; this.DeleteText = DeleteText; }

                public string GetString {
                    get => $"DELETE FROM investTable WHERE {this.DeleteTag} = @DeleteText"; }

                public List<SQLiteParameter> GetParameters {
                    get => new List<SQLiteParameter> { new SQLiteParameter("@DeleteText", this.DeleteText) }; }
            }

            public class Add
            {
                private string DeleteTag, DeleteText;

                public Add(string DeleteTag, string DeleteText) { this.DeleteTag = DeleteTag; this.DeleteText = DeleteText; }

                public string GetString
                {
                    get => $"DELETE FROM investTable WHERE {this.DeleteTag} = @DeleteText";
                }

                public List<SQLiteParameter> GetParameters
                {
                    get => new List<SQLiteParameter> { new SQLiteParameter("@DeleteText", this.DeleteText) };
                }
            }

            public class Edit
            {
                private Helper.RowData row;

                public Edit(Helper.RowData row) { this.row = row; }

                public string GetString
                {
                    get => $"DELETE FROM investTable WHERE {this.DeleteTag} = @DeleteText";
                }

                public List<SQLiteParameter> GetParameters
                {
                    get => new List<SQLiteParameter> { new SQLiteParameter("@DeleteText", this.DeleteText) };
                }
            }
        }
    }
}
