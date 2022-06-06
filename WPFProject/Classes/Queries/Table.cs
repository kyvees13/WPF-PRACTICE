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
    public class Table
    {
        // define db
        protected private Database db;

        // define variables
        protected private string Query;
        protected private List<SQLiteParameter> parameters;
        protected private bool isSelect;

        // setter getter fields
        public string GetString { get => Query; set => Query = value; }
        public List<SQLiteParameter> GetParameters { get => parameters; set => parameters = value; }
        public bool Gettype { get => isSelect; set => isSelect = value; }

        // constructors
        public Table() { db = new Database(db_path: "wpfproject.db"); }
        //public Table(string QuerySQL, List<SQLiteParameter> Params) { Query = QuerySQL; parameters = Params; }

        // main method execute query
        public long Execute() { return db.ExecuteCommand(QuerySQL: GetString, Params: GetParameters, isSelect: Gettype); }
        public RequestStatement ExecuteWithStatement() { return this.SelectorStatement(Execute()); }

        // set query callback in datatable
        public DataTable Fill() { return db.GetFilledTable(GetString, GetParameters); }

        // selector to return statement
        protected RequestStatement SelectorStatement(object result)
        {
            switch (result)
            {
                case 1: return RequestStatement.Success;
                case 0: return RequestStatement.Warning;
                case -1: return RequestStatement.Error;

                default: return RequestStatement.Unexpected;
            }
        }
    }
}
