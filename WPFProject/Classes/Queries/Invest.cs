using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;

using WPFProject.Classes;
using WPFProject.Classes.Data;

namespace WPFProject.Classes.Queries
{
    public class Invest : Table
    {
        protected Row row { get; set; }

        private string id;

        private string _searchtag;
        private string _searchtext;

        protected string ID {  get => id; set => id = value; }
        protected string SearchTag { get => _searchtag; set => _searchtag = value; }
        protected string SearchText { get => _searchtext; set => _searchtext = value; }

        protected Invest() { }
        protected Invest(string ID) { this.ID = id; }
        protected Invest(Row row) { this.row = row; }
        protected Invest(string SearchTag, string SearchText) { this.SearchTag = SearchTag; this.SearchText = SearchText; }

        public class Add : Invest
        {
            public Add(Row row) : base(row)
            {
                this.Query = Queries.Constants.Invest.Add;
                this.parameters = Database.GenerateRowSQLParameters(row, isAdd: true);
                this.isSelect = false;
            }
        }
        public class Edit : Invest
        {
            public Edit(Row row) : base(row)
            {
                this.Query = Queries.Constants.Invest.Edit;
                parameters = Database.GenerateRowSQLParameters(row, isAdd: false);
                this.isSelect = false;
            }
        }
        public class Delete : Invest
        {
            public Delete(string ID) : base(ID)
            {
                this.Query = String.Format(Queries.Constants.Invest.Delete, arg0: "id");
                this.parameters = new List<SQLiteParameter> { new SQLiteParameter("@DeleteText", ID)};
                this.isSelect = false;
            }
        }
        public class Search : Invest
        {
            public Search(string SearchTag, string SearchText) : base(SearchTag, SearchText)
            {
                this.Query = String.Format(format: Queries.Constants.Invest.Search, arg0: SearchTag);
                this.parameters = new List<SQLiteParameter> { new SQLiteParameter("@SearchText", SearchText) };
                this.isSelect = true;
            }
        }
        public class Load : Invest
        {
            public Load() : base()
            {
                this.Query = Queries.Constants.Invest.Load;
                this.parameters = new List<SQLiteParameter> { };
                this.isSelect = true;
            }
        }
    }
}
