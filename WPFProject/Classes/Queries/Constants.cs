using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFProject.Classes.Queries
{
    public enum RequestStatement
    {
        Unexpected = -2,
        Error = -1,
        Warning = 0,
        Success = 1
    }

    public static class Constants
    {
        public static class Users
        {
            public const string Authorize = "select * from usersAcc where login=@login and password=@password";
            public const string Search = "SELECT * FROM usersAcc WHERE login=@login";

            public const string Add = "insert into usersAcc(login,password) VALUES(@login,@password)";
            public const string Recover = "";
        }
        public static class Invest
        {
            public const string Search = "SELECT * FROM investTable WHERE {0} LIKE @SearchText";
            public const string Recover = "";

            
            public const string Delete = "DELETE FROM investTable WHERE {this.DeleteTag} = @DeleteText";

            public const string Add = "";
            public const string Edit = "";

        }
    }
}
