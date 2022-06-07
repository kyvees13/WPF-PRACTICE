using System;
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

        private RequestStatement ExecuteStatement;

        private string ComboboxText { get => SearchCombobox.SelectedValue.ToString(); }
        private string SearchText { get => SearchTextBox.Text.ToString(); }

        public MainViewer()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadDataGrid();
            this.LoadComboboxFromDatagrid();
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

        // обновление привязки данных 
        private void RefreshDatagrid(DataTable table)
        {
            DataGridView.DataContext = null;
            DataGridView.DataContext = table.DefaultView;
        }

        // обращение SQL-запроса и перепривязка данных
        private void LoadDataGrid()
        {
            RefreshDatagrid(table: new Invest.Load().Fill());
        }

        // ивент изменения текстбокса для поиска
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text == "" || SearchCombobox.Text == "" )
            {
                this.LoadDataGrid();
            }
            else
            {
                this.RefreshDatagrid(table: new Invest.Search(SearchTag: ComboboxText, SearchText: SearchText).Fill());
            }
        }

        // ивент кнопки для перехода на формуляр добавления
        private void ClickToAddPage(object sender, RoutedEventArgs e)
        {
            AdditionalForm addpage = new AdditionalForm();
            addpage.Owner = this;

            addpage.ShowDialog();
            this.LoadDataGrid();
        }

        // ивент кнопки для перехода на формуляр изменения
        private void ClickToEditPage(object sender, RoutedEventArgs e)
        {
            DataRowView selectedrow = DataGridView.SelectedItem as DataRowView;

            if (selectedrow != null)
            {
                AdditionalForm editpage = new AdditionalForm(currRow: selectedrow);
                editpage.Owner = this;

                editpage.ShowDialog();
                this.LoadDataGrid();
            }
            else
            {
                MessageBox.Show( "Необходимо выбрать изменяемую запись!", "Запись не выбрана", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        // ивент кнопки для удаления записи
        private void ClickToDeleteRow(object sender, RoutedEventArgs e)
        {
            DataRowView selectedrow = DataGridView.SelectedItem as DataRowView;

            if (selectedrow != null)
            {
                string id = selectedrow.Row.ItemArray[0].ToString();

                MessageBoxResult answer = MessageBox.Show( owner: Application.Current.MainWindow,
                    $"Вы действительно хотите удалить запись? | ID: {id}", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (answer == MessageBoxResult.Yes)
                {
                    ExecuteStatement = new Invest.Delete(id).ExecuteWithStatement();

                    switch (ExecuteStatement)
                    {
                        case RequestStatement.POSITIVE: break;
                        case RequestStatement.NULL: MessageBox.Show("Не удалось удалить элемент!", "Ошибка!"); break;
                        case RequestStatement.ERROR: MessageBox.Show("Ошибка запроса!", "Ошибка базы данных!"); break;
                        case RequestStatement.UNEXPECTED: MessageBox.Show("Непредвиденная ошибка"); break;
                    }

                    this.LoadDataGrid();
                } 

                else return;
            }
        }

        private void ClickToDownloadExcel(object sender, RoutedEventArgs e)
        {

            string filename = _rootFolder + @"\ExcelFile.xls";

            var xlApp = new Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Не удалось создать файл!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
