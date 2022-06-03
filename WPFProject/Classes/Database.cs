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

        private readonly string _rootFolder = Path.GetDirectoryName(
            System.AppDomain.CurrentDomain.BaseDirectory);

        private SQLiteDataAdapter SQLiteAdapter;
        private SQLiteConnection conn;
        private SQLiteCommand cmd;
        private DataTable Table;

        private string db_path;

        public string DatabasePath { get => _rootFolder + this.db_path; }
        private string ConnectionString { get => $"Data Source={this.DatabasePath}; Version=3;"; }

        public Database(string db_path)
        {
            this.db_path = db_path;
            this.Recover();
        }
        private void Recover()
        {
            if (!File.Exists(this.DatabasePath)) SQLiteConnection.CreateFile(this.DatabasePath);
            this.ExecuteCommand(QuerySQL: Query.Create.UsersTable, Params: new List<SQLiteParameter>());
            this.ExecuteCommand(QuerySQL: Query.Create.InvestTable, Params: new List<SQLiteParameter>());
        }
        private SQLiteCommand LoadCommandWithParameters(SQLiteCommand cmd, List<SQLiteParameter> Params)
        {
            foreach (SQLiteParameter param in Params) cmd.Parameters.Add(param);
            return cmd;
        }

        public int ExecuteCommand(string QuerySQL, List<SQLiteParameter> Params)
        {
            int result = 0;
            try
            {
                conn = new SQLiteConnection(this.ConnectionString);
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = QuerySQL;
                cmd = this.LoadCommandWithParameters(cmd, Params);

                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
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
            SQLiteAdapter = new SQLiteDataAdapter();
            Table = new DataTable();

            try
            {
                conn = new SQLiteConnection(this.ConnectionString);
                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandText = QuerySQL;

                SQLiteAdapter.SelectCommand = LoadCommandWithParameters(cmd, Params);
                SQLiteAdapter.Fill(Table);

                return Table;
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
