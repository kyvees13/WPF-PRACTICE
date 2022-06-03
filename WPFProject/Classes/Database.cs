using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Security.Cryptography;

namespace WPFProject
{
    public class Database
    {

        private static readonly string _rootFolder = Path.GetDirectoryName(
            System.AppDomain.CurrentDomain.BaseDirectory);

        private SQLiteDataAdapter SQLiteAdapter;
        private SQLiteConnection conn;
        private SQLiteCommand cmd;
        private DataTable Table;

        private string db_path;
        private string connectionString;

        public string QuerySQL;


        public Database(string db_path)
        {
            this.db_path = _rootFolder + db_path;
            this.connectionString = $"Data Source={db_path}; Version=3;";

            this.Recover();
        }

        public void Recover()
        {
            if (!File.Exists(db_path))
                SQLiteConnection.CreateFile(this.db_path);

            this.ExecuteCommand(
                QuerySQL:
                "CREATE TABLE IF NOT EXISTS usersAcc (" +
                    "id INTEGER NOT NULL UNIQUE," +
                    "login TEXT NOT NULL," +
                    "password  TEXT NOT NULL," +
                    "PRIMARY KEY(id AUTOINCREMENT))",
                Params: new List<SQLiteParameter>());

            this.ExecuteCommand(

                QuerySQL:
                "CREATE TABLE IF NOT EXISTS investTable (" +
                    "id INTEGER NOT NULL UNIQUE," +
                    "name_of TEXT NOT NULL," +
                    "organization  TEXT NOT NULL," +
                    "district  TEXT NOT NULL," +
                    "review TEXT NOT NULL," +
                    "category TEXT NOT NULL," +
                    "cashflow_category TEXT NOT NULL," +
                    "originality TEXT NOT NULL," +
                    "social_profit TEXT NOT NULL," +
                    "taxes TEXT NOT NULL," +
                    "num_workers TEXT NOT NULL," +
                    "paid_salary TEXT NOT NULL," +
                    "realize_period TEXT NOT NULL," +
                    "rating TEXT NOT NULL," +
                "PRIMARY KEY(id AUTOINCREMENT));",

                Params: new List<SQLiteParameter>());
        }

        public int ExecuteCommand(string QuerySQL, List<SQLiteParameter> Params)
        {
            int result = 0;
            try
            {
                conn = new SQLiteConnection(connectionString);
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = QuerySQL;
                cmd = this.setup_parameters(cmd, Params);

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

        public SQLiteCommand setup_parameters(SQLiteCommand cmd, List<SQLiteParameter> Params)
        {
            foreach (SQLiteParameter param in Params) cmd.Parameters.Add(param); 
            return cmd;
        }

        public DataTable GetFilledTable(string QuerySQL, List<SQLiteParameter> Params)
        {
            SQLiteAdapter = new SQLiteDataAdapter();
            Table = new DataTable();

            conn = new SQLiteConnection(connectionString);
            conn.Open();

            cmd = conn.CreateCommand();
            cmd.CommandText = QuerySQL;

            cmd = this.setup_parameters(cmd, Params);

            SQLiteAdapter.SelectCommand = cmd;
            SQLiteAdapter.Fill(Table);

            conn.Close();

            return Table;
        }

        public static string HashingPass(string login, string password)
        {
            using (SHA512CryptoServiceProvider hashAlgorithm = new SHA512CryptoServiceProvider())
            {
                byte[] byteValue = Encoding.UTF8.GetBytes(login + password);
                byte[] hashValue = hashAlgorithm.ComputeHash(byteValue);

                string hashString = BitConverter.ToString(hashValue);

                return hashString;
            };
        }
    }
}
