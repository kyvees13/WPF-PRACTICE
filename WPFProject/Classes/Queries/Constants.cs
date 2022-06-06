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
            public const string Authorize = "SELECT COUNT(*) from usersAcc where login=@login and password=@password";
            public const string Search = "SELECT COUNT(*) FROM usersAcc WHERE login=@login";
            public const string Add = "insert into usersAcc(login,password) VALUES(@login,@password)";
            public const string Recover = 
                "CREATE TABLE IF NOT EXISTS usersAcc (" +
                    "id INTEGER NOT NULL UNIQUE," +
                    "login TEXT NOT NULL," +
                    "password  TEXT NOT NULL," +
                    "PRIMARY KEY(id AUTOINCREMENT))";
        }
        public static class Invest
        {
            public const string Search = "SELECT * FROM investTable WHERE {0} LIKE @SearchText";
            public const string Delete = "DELETE FROM investTable WHERE {0} = @DeleteText";
            public const string Load = "SELECT * from investTable";
            public const string Recover = 
                "CREATE TABLE IF NOT EXISTS investTable (" +
                    "id INTEGER NOT NULL UNIQUE," +
                    "name_of TEXT NOT NULL," +
                    "organization  TEXT NOT NULL," +
                    "district  TEXT NOT NULL," +
                    "review TEXT NOT NULL," +
                    "category TEXT NOT NULL," +
                    "cashflow_category TEXT NOT NULL," +
                    "originality TEXT NOT NULL," +
                    "social_profit TEXT NOT NULL," +
                    "taxes TEXT NOT NULL," +
                    "num_workers TEXT NOT NULL," +
                    "paid_salary TEXT NOT NULL," +
                    "realize_period TEXT NOT NULL," +
                    "rating TEXT NOT NULL," +
                "PRIMARY KEY(id AUTOINCREMENT));";

            public const string Add = 
                "INSERT INTO investTable(" +
                    "name_of," +
                    "organization," +
                    "district," +
                    "review," +
                    "category," +
                    "cashflow_category," +
                    "originality," +
                    "social_profit," +
                    "taxes," +
                    "num_workers," +
                    "paid_salary," +
                    "realize_period," +
                    "rating) " +
                "VALUES(" +
                    "@name_of," +
                    "@organization," +
                    "@district," +
                    "@review," +
                    "@category," +
                    "@cashflow_category," +
                    "@originality," +
                    "@social_profit," +
                    "@taxes," +
                    "@num_workers," +
                    "@paid_salary," +
                    "@realize_period," +
                    "@rating) ";

            public const string Edit = 
                "UPDATE investTable SET " +
                    "name_of=@name_of," +
                    "organization=@organization," +
                    "district=@district," +
                    "review=@review," +
                    "category=@category," +
                    "cashflow_category=@cashflow_category," +
                    "originality=@originality," +
                    "social_profit=@social_profit," +
                    "taxes=@taxes," +
                    "num_workers=@num_workers," +
                    "paid_salary=@paid_salary," +
                    "realize_period=@realize_period," +
                    "rating=@rating " +
                "WHERE id=@id ";
        }
    }
}
