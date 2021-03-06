using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SQLite;

using WPFProject.Classes;
using WPFProject.Classes.Data;

namespace WPFProject.Classes.Queries
{
    public class Table : Database
    {
        // define variables
        protected private string Query;
        protected private List<SQLiteParameter> parameters;
        protected private bool isSelect;

        // setter getter query fields
        public string QueryString { get => Query; set => Query = value; }
        // setter getter parameter fields
        public List<SQLiteParameter> QueryParameters { get => parameters; set => parameters = value; }
        public bool QueryType { get => isSelect; set => isSelect = value; }

        // constructors
        public Table() : base(db_path: "wpfproject.db") { }

        // main method execute query
        public long Execute() { return this.ExecuteCommand(QuerySQL: QueryString, Params: QueryParameters, isSelect: QueryType); }
        public RequestStatement ExecuteWithStatement() { return this.SelectorStatement(Execute()); }

        // set query callback in datatable
        public DataTable Fill() { return this.GetFilledTable(QueryString, QueryParameters); }

        // selector to return statement
        protected RequestStatement SelectorStatement(long result)
        {
            switch (result)
            {
                case 1: return RequestStatement.POSITIVE;
                case 0: return RequestStatement.NULL;
                case -1: return RequestStatement.ERROR;
                default: return RequestStatement.UNEXPECTED;
            }
        }
    }
}
