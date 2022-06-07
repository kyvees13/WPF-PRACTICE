using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
using WPFProject.Classes.Data;
using WPFProject.Classes.Comboboxes;

using System.Data.SQLite;
using System.Text.RegularExpressions;
using WPFProject.Classes.Queries;

namespace WPFProject.Pages
{
    public partial class AdditionalForm : Window
    {
        public Row row;

        RequestStatement ExecuteStatement;

        public AdditionalForm()
        {
            InitializeComponent();

            row = new Row();
            this.Title = "Добавление новой записи ";
        }
        public AdditionalForm(DataRowView currRow)
        {
            InitializeComponent();

            row = new Row(currRow);
            this.Title = $"Изменение данных для записи | ID: {row.id}";
        }

        private void LoadCombobox()
        {
            ComboboxCategory.ItemsSource = ComboboxPairs.Category;
            ComboboxCashFlow.ItemsSource = ComboboxPairs.CashFlow;
            ComboboxOriginality.ItemsSource = ComboboxPairs.Originality;
            ComboboxSocialProfit.ItemsSource = ComboboxPairs.SocialProfit;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCombobox();
            DataContext = row;
        }

        private void SaveRowPropertiesToSQL(object sender, RoutedEventArgs e)
        {

            // проверка ввода всех полей
            if (!WindowAssist.isAllTextBoxesFilled(this) || !WindowAssist.isAllComboboxFilled(this))
            {
                MessageBox.Show("Не все поля введены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            // проверка на выбор типа запроса к базе данных (создание новой или редактирование существующей)
            if (row.id == null) ExecuteStatement = new Invest.Add(row: row).ExecuteWithStatement();
            if (row.id != null) ExecuteStatement = new Invest.Edit(row: row).ExecuteWithStatement();
            
            // обработка ошибок
            if (ExecuteStatement is RequestStatement.ERROR)
            {
                MessageBox.Show("Запрос к базе данных не удовлетворен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (ExecuteStatement is RequestStatement.NULL)
            {
                MessageBox.Show("Данные не были изменены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }
            else if (ExecuteStatement is RequestStatement.POSITIVE)
            {
                // Успешно добавлено
                this.Close();
                return;
            }
            else if (ExecuteStatement is RequestStatement.UNEXPECTED)
            {
                MessageBox.Show("Непредвиденная ошибка.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }
        }

        private void isOnlyDigitTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void isOnlyCharDigitTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9\s]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

}
