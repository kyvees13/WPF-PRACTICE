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
    public partial class Register : Window
    {

        public Database db = new Database(db_path: "wpfproject.db");

        int result;

        public Register()
        {
            InitializeComponent();
        }

        private void ButtonRegister(object sender, RoutedEventArgs e)
        {
            if (!WindowAssist.isAllTextBoxesFilled(this))
            {
                MessageBox.Show("Поля введены неверно!", "Ошибка ввода!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (PassBox.Password.Trim() != RePassBox.Password.Trim())
            {
                MessageBox.Show("Пароли не равны!", "Ошибка ввода!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string login = LoginBox.Text.Trim();
            string stringHashPass = Database.HashingPass(login, PassBox.Password.Trim());

            var table = db.GetFilledTable(
                QuerySQL: "select * from usersAcc where login=@login",
                Params: new List<SQLiteParameter> { new SQLiteParameter("@login", login) });


            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Аккаунт уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (table.Rows.Count == 0)
            {
                result = db.ExecuteCommand(
                    QuerySQL: "insert into usersAcc (login,password) VALUES(@login,@password)",
                    Params: new List<SQLiteParameter> {
                    new SQLiteParameter("@login", login),
                    new SQLiteParameter("@password", stringHashPass)
                });

                if (result == 1)
                {
                    MessageBox.Show("Регистрация пройдена успешно!", "Ура!", MessageBoxButton.OK);
                }
                else if (result == 0)
                {
                    MessageBox.Show("Аккаунт не был зарегистрирован!", "Ошибка регистрации!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (result == -1)
                {
                    MessageBox.Show("Непредвиденная ошибка запроса!", "Ошибка запроса", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                this.Owner.Show();
                this.Hide();

            }
        }
        private void TextChangedEvent(object sender, RoutedEventArgs e)
        {
            LoginBox.Background = Brushes.White;
            PassBox.Background = Brushes.White;
            RePassBox.Background = Brushes.White;
        }
        private void isOnlyCharDigitTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9\s]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
