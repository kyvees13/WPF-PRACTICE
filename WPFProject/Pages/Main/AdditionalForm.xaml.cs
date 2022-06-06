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

            if (!WindowAssist.isAllTextBoxesFilled(this) || !WindowAssist.isAllComboboxFilled(this))
            {
                MessageBox.Show("Не все поля введены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            long result = -1;
            
            if (row.id == null) result = new Invest.Add(row: row).Execute();
            if (row.id != null) result = new Invest.Edit(row: row).Execute();
            
            if (result == -1)
            {
                MessageBox.Show("Запрос к базе данных не удовлетворен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (result == 0)
            {
                MessageBox.Show("Данные не были изменены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }

            this.Close();
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
