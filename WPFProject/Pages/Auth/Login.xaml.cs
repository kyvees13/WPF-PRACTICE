using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFProject.Classes;

namespace WPFProject.Pages.Auth
{
    public partial class Login : Window
    {

        public Database db = new Database(db_path: "wpfproject.db");

        public Login()
        {
            InitializeComponent();
        }

        private void ButtonLogin(object sender, RoutedEventArgs e)
        {
            if (!WindowAssist.isAllTextBoxesFilled(this))
            {
                MessageBox.Show("Введите все поля!");
                return;
            }

            string login = LoginBox.Text.Trim();
            string hashpass = Cryptography.HashingPass(login, PassBox.Password.Trim());

            var query = new Queries(db_path: "wpfproject.db");

            Queries QuerySQL = new QuerySQL.Users.Add();

            var search = new Queries.Qwe.Table.Search(login, hashpass);
            search.TableName = "investTable";


            var QueryResult = QuerySQL.Result;

            if (QueryResult is RequestStatement.Success)
            {

            } else 

            if (QueryResult is RequestStatement.Warning)
            {

            } else 
            
            if (QueryResult is RequestStatement.Error)
            {

            }
            

            var QueryAuth = new Queries.Users.Auth(login, hashpass);
            int result = QueryAuth.Execute;
            

            var QuerySQL = new Query.Users.Add(login: login, password: hashpass);
            DataTable result = QuerySQL.Exe();

            var table = db.GetFilledTable(QuerySQL: QuerySQL.GetString, Params: QuerySQL.GetParameters);

            if (table.Rows.Count > 0)
            {
                DatabaseWindow main = new DatabaseWindow();
                main.Owner = this;

                main.Show();
                this.Hide();
            }
            else if (table.Rows.Count == 0)
            {
                LoginBox.Text = "";
                PassBox.Password = "";

                LoginBox.Background = Brushes.DarkRed;
                PassBox.Background = Brushes.DarkRed;

                MessageBox.Show(
                    "Логин или пароль введены неверно!", 
                    "Ошибка аутентификации", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            };
        }
        private void ClickToRegister(object sender, RoutedEventArgs e)
        {
            var regWindow = new Register();
            regWindow.Owner = this;

            this.Hide();
            regWindow.Show();
        }
        private void TextChangedEvent(object sender, RoutedEventArgs e)
        {
            LoginBox.Background = Brushes.White;
            PassBox.Background = Brushes.White;
        }
        private void isOnlyCharDigitTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9\s]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
