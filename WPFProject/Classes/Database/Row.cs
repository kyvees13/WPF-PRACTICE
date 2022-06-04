using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using WPFProject.Classes.Comboboxes;

namespace WPFProject.Classes.Database
{
    public class Row
    {
        public Row() { }

        public string id { get; set; } = null;
        public string name_of { get; set; } = null;
        public string organization { get; set; } = null;
        public string district { get; set; } = null;
        public string review { get; set; } = null;
        public string taxes { get; set; } = null;
        public string num_workers { get; set; } = null;
        public string paid_salary { get; set; } = null;
        public string realize_period { get; set; } = null;

        public string rating { get; set; } = null;

        public KeyValuePair<string, string> category { get; set; } = 
            new KeyValuePair<string, string> ( null, null );

        public KeyValuePair<string, string> cashflow_category { get; set; } = 
            new KeyValuePair<string, string>(null, null);

        public KeyValuePair<string, string> originality { get; set; } = 
            new KeyValuePair<string, string>(null, null);

        public KeyValuePair<string, string> social_profit { get; set; } = 
            new KeyValuePair<string, string>(null, null);

        public void Fill(DataRowView currRow)
        {
            id = currRow.Row["id"].ToString();

            name_of = currRow.Row["name_of"] as string;
            organization = currRow.Row["organization"] as string;
            district = currRow.Row["district"] as string;
            review = currRow.Row["review"] as string;
            taxes = currRow.Row["taxes"] as string;
            num_workers = currRow.Row["num_workers"] as string;
            paid_salary = currRow.Row["paid_salary"] as string;
            realize_period = currRow.Row["realize_period"] as string;

            rating = currRow.Row["rating"] as string;

            category = 
                new ComboboxFinder(
                    currRow.Row["category"] as string).CategoryPair;

            cashflow_category = 
                new ComboboxFinder(
                    currRow.Row["cashflow_category"] as string).CashFlowPair;

            originality = 
                new ComboboxFinder(
                    currRow.Row["originality"] as string).OriginalityPair;

            social_profit = 
                new ComboboxFinder(
                    currRow.Row["social_profit"] as string).SocialProfitPair;
        }
    }
}
