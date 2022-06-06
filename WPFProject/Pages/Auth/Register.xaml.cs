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
    public partial class Register : Window
    {

        private RequestStatement ExecuteStatement;

        private string GetLogin { get => LoginBox.Text.Trim(); }
        private string GetPass { get => PassBox.Password.Trim(); }
        private string GetRePass { get => RePassBox.Password.Trim(); }
        private string GetHashedPass { get => Cryptography.HashingPass(GetLogin, GetPass); }


        public Register()
        {
            InitializeComponent();
        }

        private void CheckAccount(RequestStatement state)
        {

        }

        private void ButtonRegister(object sender, RoutedEventArgs e)
        {
            if (!WindowAssist.isAllTextBoxesFilled(this))
            {
                MessageBox.Show("Поля введены неверно!", "Ошибка ввода!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (GetPass != GetRePass)
            {
                MessageBox.Show("Пароли не равны!", "Ошибка ввода!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ExecuteStatement = new Account.Search(Username: GetLogin).ExecuteWithStatement();

            if (ExecuteStatement is RequestStatement.POSITIVE)
            {
                MessageBox.Show("Аккаунт уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (ExecuteStatement is RequestStatement.ERROR)
            {
                MessageBox.Show("Ошибка запроса к базе данных!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (ExecuteStatement is RequestStatement.UNEXPECTED)
            {
                MessageBox.Show("Непредвиденная ошибка!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (ExecuteStatement is RequestStatement.NULL)
            {
                MessageBox.Show("Аккаунт не был найден и сейчас попытаемся его зарегистрировать!", "Круто!");
                
                ExecuteStatement = new Account.Register(
                    Username: GetLogin, 
                    Password: GetHashedPass)
                    .ExecuteWithStatement();

                if (ExecuteStatement is RequestStatement.POSITIVE)
                {
                    MessageBox.Show(
                        "Регистрация пройдена успешно!", 
                        "Ура!", 
                        MessageBoxButton.OK);

                    this.Owner.Show();
                    this.Hide();
                    return;
                }
                else if (ExecuteStatement is RequestStatement.NULL)
                {
                    MessageBox.Show("Не удалось добавить аккаунт!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (ExecuteStatement is RequestStatement.ERROR)
                {
                    MessageBox.Show("Непредвиденная ошибка", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (ExecuteStatement is RequestStatement.UNEXPECTED)
                {
                    MessageBox.Show("esfsefsefsefse");
                }
                MessageBox.Show("111");
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
