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

using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace WPFProject.Pages
{
    public partial class DataPage : Window
    {

        int result;

        public Database db = new Database(db_path: "wpfproject.db");
        public Helper.RowData row;

        public DataPage()
        {
            InitializeComponent(); row = new Helper.RowData(); 
            this.Title = "Добавление новой записи ";
        }
        public DataPage(DataRowView currRow)
        {
            InitializeComponent(); row = new Helper.RowData(currRow); 
            this.Title = $"Изменение данных для записи | ID: {row.id}";
        }

        private void LoadCombobox()
        {
            ComboboxCategory.ItemsSource = Helper.ComboboxPairs.Category;
            ComboboxCashFlow.ItemsSource = Helper.ComboboxPairs.CashFlow;
            ComboboxOriginality.ItemsSource = Helper.ComboboxPairs.Originality;
            ComboboxSocialProfit.ItemsSource = Helper.ComboboxPairs.SocialProfit;
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
            
            if (row.id == null) result = db.ExecuteCommand(QuerySQL: Helper.insertRowQuery, Params: Helper.generateSQLParameters(currRow: row, isAdd: true));
            if (row.id != null) result = db.ExecuteCommand(QuerySQL: Helper.updateRowQuery, Params: Helper.generateSQLParameters(currRow: row, isAdd: false));
            
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
