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
using WPFProject.Classes.Helper;
using WPFProject.Classes.Queries;

namespace WPFProject.Pages.Auth
{
    public partial class Login : Window
    {

        private RequestStatement ExecutedStatement;

        private string GetLogin { get => LoginBox.Text.Trim(); }
        private string GetPass { get => PassBox.Password.Trim(); }

        private string GetHashedPass { get => Cryptography.HashingPass(GetLogin, GetPass); }

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

            // Searching exist credentials of account in database
            ExecutedStatement = new Account.Authorize(GetLogin, GetHashedPass).ExecuteWithStatement();

            if (ExecutedStatement is RequestStatement.POSITIVE)
            {
                MainViewer main = new MainViewer();
                main.Owner = this;

                main.Show();
                this.Hide();
            }
            else if (ExecutedStatement is RequestStatement.NULL)
            {
                this.ClearFields();
                MessageBox.Show("Логин или пароль введены неверно!", "Ошибка аутентификации", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
            else if (ExecutedStatement is RequestStatement.ERROR)
            {
                MessageBox.Show("Ошибка запроса!", "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (ExecutedStatement is RequestStatement.UNEXPECTED)
            {
                MessageBox.Show("Непредвиденная ошибка!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClickToRegister(object sender, RoutedEventArgs e)
        {
            Register regWindow = new Register();
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

        private void ClearFields()
        {
            LoginBox.Text = "";
            PassBox.Password = "";

            LoginBox.Background = Brushes.DarkRed;
            PassBox.Background = Brushes.DarkRed;
        }
    }
}
