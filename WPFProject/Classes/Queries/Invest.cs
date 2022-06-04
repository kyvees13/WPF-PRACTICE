using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
using WPFProject.Classes;

namespace WPFProject.Classes.Queries
{
    public class Invest : Table
    {
        protected Helper.RowData row { get; set; }

        string searchtag;
        string searchtext;

        protected string SearchTag { get => ; set; };
        protected string SearchText { get; set; };

        protected Invest(Helper.RowData row)
        {
            this.row = row;
        }

        public class Add : Invest
        {
            public Add(Helper.RowData row) : base(row)
            {
                this.Query = Queries.Constants.Invest.Add;
                this.parameters = Helper.GenerateRowSQLParameters(row, isAdd: false);
            }
        }
        public class Edit : Invest
        {
            public Edit(Helper.RowData row) : base(row)
            {
                this.Query = Queries.Constants.Invest.Edit;
                parameters = Helper.GenerateRowSQLParameters(row, isAdd: false);
            }

        }
        public class Delete : Invest
        {
            public Delete(Helper.RowData row) : base(row)
            {
                this.Query = Queries.Constants.Invest.Edit;
                parameters = Helper.GenerateRowSQLParameters(row, isAdd: false);
            }
        }
        public class Search : Invest
        {
            public Search(string SearchTag, string SearchText) : base(SearchTag, SearchText)
            {
                this.Query = String.Format(format: Queries.Constants.Invest.Search, arg0: SearchText);
                parameters = Helper.GenerateRowSQLParameters(row, isAdd: false);
            }
        }
    }
}
