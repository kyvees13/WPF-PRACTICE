using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace WPFProject.Classes
{
    public class Helper
    {
        public static readonly string insertRowQuery =
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

        public static readonly string updateRowQuery = 
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

        public class ComboboxPairs
        {
            private string comboboxKey;

            public ComboboxPairs() { }
            public ComboboxPairs(string comboboxKey) { this.comboboxKey = comboboxKey; }

            public static List<KeyValuePair<string, string>> Category = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(key: "Услуги млн. руб.", value: "1"),
                new KeyValuePair<string, string>(key: "Строительные смеси тыс. тонн", value: "2"),
                new KeyValuePair<string, string>(key: "Древесина тыс. куб. м.", value: "3"),
                new KeyValuePair<string, string>(key: "Кирпич и бетонные блоки тыс. шт.", value: "4"),
                new KeyValuePair<string, string>(key: "Транспортные услуги млн. руб.", value: "5"),
                new KeyValuePair<string, string>(key: "Продукты питания млн. руб.", value: "6")
            };
            public static List<KeyValuePair<string, string>> CashFlow = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(key: "0-1", value: "1"),
                new KeyValuePair<string, string>(key: "1.0-10", value: "3"),
                new KeyValuePair<string, string>(key: "10.0-50", value: "5"),
                new KeyValuePair<string, string>(key: "50.0+", value: "6")
            };
            public static List<KeyValuePair<string, string>> Originality = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(key: "Уникальный", value: "5"),
                new KeyValuePair<string, string>(key: "Есть аналоги в РФ", value: "3"),
                new KeyValuePair<string, string>(key: "Есть аналоги в регионе", value: "2"),
            };
            public static List<KeyValuePair<string, string>> SocialProfit = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(key: "Все население", value: "5"),
                new KeyValuePair<string, string>(key: "Средний класс", value: "3"),
                new KeyValuePair<string, string>(key: "Социальнонезащищенные", value: "3"),
                new KeyValuePair<string, string>(key: "2% населения", value: "1")
            };

            private KeyValuePair<string, string> FindValueByKey(List<KeyValuePair<string, string>> pairs)
            {
                foreach (var pair in pairs) if (pair.Key == this.comboboxKey) return pair; 
                return new KeyValuePair<string, string>(null, null);
            }

            public KeyValuePair<string, string> CategoryPair { get => FindValueByKey(pairs: Category); }
            public KeyValuePair<string, string> CashFlowPair { get => FindValueByKey(pairs: CashFlow); }
            public KeyValuePair<string, string> OriginalityPair { get => FindValueByKey(pairs: Originality); }
            public KeyValuePair<string, string> SocialProfitPair { get => FindValueByKey(pairs: SocialProfit); }
        }

        public class RowData
        {
            public string id { get; set; }

            public string name_of { get; set; }
            public string organization { get; set; }
            public string district { get; set; }
            public string review { get; set; }

            public KeyValuePair<string, string> category { get; set; }
            public KeyValuePair<string, string> cashflow_category { get; set; }
            public KeyValuePair<string, string> originality { get; set; }
            public KeyValuePair<string, string> social_profit { get; set; }

            public string taxes { get; set; }
            public string num_workers { get; set; }
            public string paid_salary { get; set; }
            public string realize_period { get; set; }

            public string rating { get; set; }

            public RowData() 
            {
                this.id = null;

                this.name_of = null;
                this.organization = null;
                this.district = null;
                this.review = null;

                this.category = new KeyValuePair<string, string>(null, null);
                this.cashflow_category = new KeyValuePair<string, string>(null, null);
                this.originality = new KeyValuePair<string, string>(null, null);
                this.social_profit = new KeyValuePair<string, string>(null, null);

                this.taxes = null;
                this.num_workers = null;
                this.paid_salary = null;
                this.realize_period = null;

                this.rating = null;
            }
            public RowData(DataRowView currRow)
            {
                this.id = currRow.Row["id"].ToString();

                this.name_of = currRow.Row["name_of"] as string;
                this.organization = currRow.Row["organization"] as string;
                this.district = currRow.Row["district"] as string;
                this.review = currRow.Row["review"] as string;

                this.category = new ComboboxPairs(currRow.Row["category"] as string).CategoryPair;
                this.cashflow_category = new ComboboxPairs(currRow.Row["cashflow_category"] as string).CashFlowPair;
                this.originality = new ComboboxPairs(currRow.Row["originality"] as string).OriginalityPair;
                this.social_profit = new ComboboxPairs(currRow.Row["social_profit"] as string).SocialProfitPair;

                this.taxes = currRow.Row["taxes"] as string;
                this.num_workers = currRow.Row["num_workers"] as string;
                this.paid_salary = currRow.Row["paid_salary"] as string;
                this.realize_period = currRow.Row["realize_period"] as string;

                this.rating = currRow.Row["rating"] as string;
            }
        }

        public static List<SQLiteParameter> generateSQLParameters(RowData currRow, bool isAdd)
        {
            List<SQLiteParameter> parameters = new List<SQLiteParameter>
            {
                new SQLiteParameter("@name_of", currRow.name_of.ToString()),
                new SQLiteParameter("@organization", currRow.organization.ToString()),
                new SQLiteParameter("@district", currRow.district.ToString()),
                new SQLiteParameter("@review", currRow.review.ToString()),

                new SQLiteParameter("@category", currRow.category.Key.ToString()), //
                new SQLiteParameter("@cashflow_category", currRow.cashflow_category.Key.ToString()), //
                new SQLiteParameter("@originality", currRow.originality.Key.ToString()), //
                new SQLiteParameter("@social_profit", currRow.social_profit.Key.ToString()), //

                new SQLiteParameter("@taxes", currRow.taxes.ToString()),
                new SQLiteParameter("@num_workers", currRow.num_workers.ToString()),
                new SQLiteParameter("@paid_salary", currRow.paid_salary.ToString()),
                new SQLiteParameter("@realize_period", currRow.realize_period.ToString()),

                new SQLiteParameter("@rating", Helper.CalculateRating(currRow).ToString())
            };

            if (!isAdd) parameters.Add(new SQLiteParameter("@id", currRow.id.ToString()));
            
            return parameters;
        }

        private static double CalculateRating(RowData currRow)
        {
            double C7 = Double.Parse(currRow.cashflow_category.Value);
            double C8 = Double.Parse(currRow.originality.Value);
            double C9 = Double.Parse(currRow.social_profit.Value);

            double C10 = Double.Parse(currRow.taxes);
            double C12 = Double.Parse(currRow.paid_salary);
            double C13 = Double.Parse(currRow.realize_period);

            double rating = (C10 + C12) * C13 / 12 * (C7 + C8 + C9) / 16;

            return Math.Round(rating, 4);
        }
    }
}
