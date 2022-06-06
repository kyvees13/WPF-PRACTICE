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
            string hashpass = Cryptography.HashingPass(login, PassBox.Password.Trim());

            long result = new Account.Search(login).Execute();

            if (result == 1)
            {
                MessageBox.Show("Аккаунт уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (result == 0)
            {
                long result2 = new Account.Register(login, hashpass).Execute();

                if (result2 == 1)
                {
                    MessageBox.Show("Регистрация пройдена успешно!", "Ура!", MessageBoxButton.OK);
                    this.Owner.Show();
                    this.Hide();
                    return;
                }
            }

            /*switch (new Account.Search(Username: login).ExecuteWithStatement())
            {
                case RequestStatement.Success:
                    {
                        MessageBox.Show("Аккаунт уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                case RequestStatement.Warning:
                    {
                        switch (new Account.Register(login, hashpass).ExecuteWithStatement())
                        {
                            case RequestStatement.Success:
                                {
                                    MessageBox.Show("Регистрация пройдена успешно!", "Ура!", MessageBoxButton.OK);
                                    this.Owner.Show();
                                    this.Hide();
                                    return;
                                }
                            case RequestStatement.Warning:
                                {
                                    MessageBox.Show("Аккаунт не был зарегистрирован!", "Ошибка регистрации!", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            case RequestStatement.Error:
                                {
                                    MessageBox.Show("Непредвиденная ошибка запроса!", "Ошибка запроса", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                        }
                        break;
                    }
            }*/
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
