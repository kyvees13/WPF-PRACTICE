using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using WPFProject.Classes;

namespace WPFProject.Classes.Queries
{
    public class Table
    {
        // define db
        protected private Database db;

        // define variables
        protected private string Query;
        protected private List<SQLiteParameter> parameters;

        // setter getter fields
        public string GetString { get => Query; set => Query = value; }
        public List<SQLiteParameter> GetParameters { get => parameters; set => parameters = value; }

        // constructors
        public Table() { db = new Database(db_path: "wpfproject.db"); }
        public Table(string QuerySQL, List<SQLiteParameter> Params) { Query = QuerySQL; parameters = Params; }

        // main method execute query
        public int Execute() { return db.ExecuteCommand(GetString, GetParameters); }
        public Queries.RequestStatement ExecuteWithStatement() { return this.SelectorStatement(Execute()); }

        // selector to return statement
        protected Queries.RequestStatement SelectorStatement(int result)
        {
            switch (result)
            {
                case 1: return Queries.RequestStatement.Success;
                case 0: return Queries.RequestStatement.Warning;
                case -1: return Queries.RequestStatement.Error;

                default: return Queries.RequestStatement.Unexpected;
            }
        }
    }
}
