using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFProject.Classes
{
    static class Query
    {

        public class Search
        {
            private string SearchTag;
            public Search(string SearchTag) { this.SearchTag = SearchTag; }

            public string GetString { get => $"SELECT * FROM investTable WHERE {this.SearchTag} LIKE @content"; }
        }

        public class Delete
        {
            string DeleteTag;
            public Delete(string DeleteTag) { this.DeleteTag = DeleteTag; }

            public string GetString { get => $"DELETE FROM investTable WHERE {this.DeleteTag} = @content"; }
        }

        public class Update
        {
            public const string Row =
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

        public class Insert
        {
            public const string Row = 
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

            public const string User = 
        }

        public class Create
        {

            public class Table
            {
                public const string Users =
                    "CREATE TABLE IF NOT EXISTS usersAcc (" +
                            "id INTEGER NOT NULL UNIQUE," +
                            "login TEXT NOT NULL," +
                            "password  TEXT NOT NULL," +
                            "PRIMARY KEY(id AUTOINCREMENT))";
                public const string InvestTable =
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
            }
        }

        public class Select
        {
            public const string AllMainTable = "select * from investTable";

            public const string Login = "select * from usersAcc where login=@login";
            public const string Account = "select * from usersAcc where login=@login and password=@password";
        }

    }
}
