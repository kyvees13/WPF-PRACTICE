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

namespace WPFProject
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
