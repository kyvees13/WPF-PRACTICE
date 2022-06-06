﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

using WPFProject.Classes;
using WPFProject.Classes.Queries;

namespace WPFProject.Pages
{

    public partial class MainViewer : Window
    {
        private static readonly string _rootFolder = System.IO.Path.GetDirectoryName(
            System.AppDomain.CurrentDomain.BaseDirectory);

        private long result;

        private string SearchText { get => SearchTextBox.Text.ToString(); }
        private string ComboboxText { get => SearchCombobox.SelectedValue.ToString(); }

        public MainViewer()
        {
            InitializeComponent();
        }

        private void LoadDataGrid()
        {
            DataTable table = new Invest.Load().Fill();
            RefreshDatagrid(table);
        }

        private void LoadComboboxFromDatagrid()
        {
            for (int i = 0; i < DataGridView.Columns.Count; i++)
            {
                DataGridColumn column = DataGridView.Columns[i];
                Binding binding = (column as DataGridBoundColumn).Binding as Binding;

                string headerName = column.Header.ToString();
                string databaseName = binding.Path.Path.ToString();

                SearchCombobox.Items.Add(
                    new KeyValuePair<string, string>(headerName, databaseName));
            }
            SearchCombobox.Items.Refresh();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadDataGrid();
            this.LoadComboboxFromDatagrid();
        }

        private void RefreshDatagrid(DataTable table)
        {
            DataGridView.DataContext = null;
            DataGridView.DataContext = table.DefaultView;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text == "" || SearchCombobox.Text == "" )
            {
                this.LoadDataGrid();
            }
            else
            {
                var table = new Invest.Load().Fill();
                this.RefreshDatagrid(table);
            }
        }

        private void ClickToAddPage(object sender, RoutedEventArgs e)
        {
            AdditionalForm addpage = new AdditionalForm();
            addpage.Owner = this;

            addpage.ShowDialog();

            this.LoadDataGrid();
        }
        private void ClickToEditPage(object sender, RoutedEventArgs e)
        {
            DataRowView selectedrow = DataGridView.SelectedItem as DataRowView;

            if (DataGridView.Items.Count > 0 && selectedrow != null)
            {
                AdditionalForm editpage = new AdditionalForm(currRow: selectedrow);
                editpage.Owner = this;

                editpage.ShowDialog();

                this.LoadDataGrid();
            }
            else MessageBox.Show(
                "Необходимо выбрать изменяемую запись!", 
                "Запись не выбрана",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        private void ClickToDeleteRow(object sender, RoutedEventArgs e)
        {

            DataRowView selectedrow = DataGridView.SelectedItem as DataRowView;

            if (selectedrow != null)
            {
                string id = selectedrow.Row.ItemArray[0].ToString();

                MessageBoxResult answer = MessageBox.Show(
                    owner: Application.Current.MainWindow,
                    $"Вы действительно хотите удалить запись? | ID: {id}",
                    "Удаление записи",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (answer == MessageBoxResult.Yes)
                {
                    var QuerySQL = new Invest.Delete(id);
                    result = QuerySQL.Execute();

                    this.LoadDataGrid();
                }
            }
        }
        private void ClickToDownloadExcel(object sender, RoutedEventArgs e)
        {

            string filename = _rootFolder + @"\ExcelFile.xls";

            var xlApp = new Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show(
                    "Excel is not installed on your computer! You cant use this function.", 
                    "Ошибка открытия файла", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);

                return;
            }

            object misValue = System.Reflection.Missing.Value;
            
            var xlWorkBook = xlApp.Workbooks.Add(misValue);
            var xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            // Заполнение данными файла эксель с именованиями колонок из хедеров датагрида
            for (int i = 0; i < DataGridView.Columns.Count; i++)
            {
                xlWorkSheet.Cells[1, i+1]= DataGridView.Columns[i].Header;

                // iteration column values by rows
                int j = 1; foreach (DataRowView dr in DataGridView.ItemsSource)
                {
                    object[] rowArray = dr.Row.ItemArray;
                    xlWorkSheet.Cells[j+1, i+1] = rowArray[i];
                    
                    j++;
                }
            }

            if (System.IO.File.Exists(filename)) 
                System.IO.File.Delete(filename);

            xlWorkBook.SaveAs(
                Filename: filename,
                Excel.XlFileFormat.xlWorkbookNormal,
                misValue, misValue, misValue, misValue,
                Excel.XlSaveAsAccessMode.xlExclusive,
                misValue, misValue, misValue, misValue, misValue);

            MessageBox.Show($"База данных Excel была сохранена: {filename}");

            xlWorkBook.Close();

        }
    }
}