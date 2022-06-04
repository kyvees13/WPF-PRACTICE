using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using WPFProject.Classes;

namespace WPFProject.Classes.Database
{
    public class Database
    {

        protected readonly string _rootFolder = Path.GetDirectoryName(
            System.AppDomain.CurrentDomain.BaseDirectory);

        protected SQLiteDataAdapter SQLiteAdapter;
        protected SQLiteConnection conn;
        protected SQLiteCommand cmd;
        protected DataTable Table;

        private string db_path;

        protected string GetDatabasePath { get => _rootFolder + this.db_path; set => db_path = _rootFolder + value; }
        protected string GetConnectionString { get => $"Data Source={this.GetDatabasePath}; Version=3;"; }

        public Database(string db_path)
        {
            this.db_path = db_path;
            this.Recover();
        }
        protected void Recover()
        {
            if (!File.Exists(this.GetDatabasePath)) SQLiteConnection.CreateFile(this.GetDatabasePath);
            this.ExecuteCommand(QuerySQL: Queries.Constants.Users.Recover, Params: new List<SQLiteParameter>());
            this.ExecuteCommand(QuerySQL: Queries.Constants.Invest.Recover, Params: new List<SQLiteParameter>());
        }
        private SQLiteCommand LoadParameters(SQLiteCommand cmd, List<SQLiteParameter> Params)
        {
            foreach (SQLiteParameter param in Params) cmd.Parameters.Add(param);
            return cmd;
        }
        private SQLiteCommand GenerateSQLiteCommand(string QuerySQL)
        {
            return new SQLiteCommand(QuerySQL);
        }
        private SQLiteCommand GenerateSQLiteCommand(string QuerySQL, List<SQLiteParameter> Params)
        {
            return LoadParameters(GenerateSQLiteCommand(QuerySQL), Params);
        }
        public static List<SQLiteParameter> GenerateRowSQLParameters(Row currRow, bool isAdd)
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

                new SQLiteParameter("@rating", CalculateRating(currRow).ToString())
            };

            if (!isAdd) parameters.Add(new SQLiteParameter("@id", currRow.id.ToString()));

            return parameters;
        }
        public int ExecuteCommand(string QuerySQL, List<SQLiteParameter> Params)
        {
            int result = -1;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString))
                {
                    conn.Open(); 
                    result = GenerateSQLiteCommand(QuerySQL, Params).ExecuteNonQuery();
                }
            }
            catch
            {
                result = -1;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public DataTable GetFilledTable(string QuerySQL, List<SQLiteParameter> Params)
        {
            SQLiteAdapter = new SQLiteDataAdapter(); Table = new DataTable();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString))
                {
                    conn.Open();

                    SQLiteAdapter.SelectCommand = GenerateSQLiteCommand(QuerySQL, Params);
                    SQLiteAdapter.Fill(Table);
                }
            }
            catch
            {
            }
            finally
            {
                conn.Close();
            }
            return Table;
        }
    }
}
