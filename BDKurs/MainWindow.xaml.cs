using BDKurs.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Microsoft.Win32;
using DocumentFormat.OpenXml.InkML;
using System.Configuration;

namespace BDKurs
{
    public enum Tables
    {
        AccessCategory = 1,
        Author,
        Book,
        BookOrder,
        Employee,
        Gender,
        Genre,
        Position,
        Publisher,
        Reader,
        ReaderCategory,
        Status
    }


    public partial class MainWindow : Window
    {
        private readonly LibraryDbContext _context;

        private Tables currentchoose;

        AccessCategory currentAcess;

        string filePath = "Отчет.docx";

        public Tables CurrentChoose {

            get
            {
                return currentchoose;
            }
            set
            {
                switch(value)
                {
                    case Tables.AccessCategory:
                        currentchoose = value;
                        LoadData("Категории доступа", () => _context.AccessCategorys.AsQueryable());
                        break;

                    case Tables.Author:
                        currentchoose = value;
                        LoadData("Авторы", () => _context.Authors.Include(a => a.Gender).AsQueryable());
                        break;

                    case Tables.Book:
                        currentchoose = value;
                        LoadData("Книги", () => _context.Books
                        .Include(a => a.Publisher)
                        .Include(a => a.Author)
                        .Include(a => a.Status)
                        .Include(a => a.Genre)
                        .AsQueryable());
                        break;

                    case Tables.BookOrder:
                        currentchoose = value;
                        LoadData("Ордера", () => _context.BookOrders
                        .Include(a => a.Book)
                        .Include(a => a.Reader)
                        .Include(a => a.Employee)
                        .AsQueryable());
                        break;

                    case Tables.Employee:
                        currentchoose = value;
                        LoadData("Сотрудники", () => _context.Employees
                        .Include(a => a.Gender)
                        .Include(a => a.Position)
                        .Include(a => a.AccessCategory)
                        .AsQueryable());
                        break;

                    case Tables.Genre:
                        currentchoose = value;
                        LoadData("Жанры", () => _context.Genres
                        .AsQueryable());
                        break;

                    case Tables.Position:
                        currentchoose = value;
                        LoadData("Статусы книг", () => _context.Positions.AsQueryable());
                        break;

                    case Tables.Publisher:
                        currentchoose = value;
                        LoadData("Издатели", () => _context.Publishers.AsQueryable());
                        break;

                    case Tables.Reader:
                        currentchoose = value;
                        LoadData("Читатели", () => _context.Readers.Include(a => a.Gender).Include(a => a.ReaderCategory).AsQueryable());
                        break;

                    case Tables.ReaderCategory:
                        currentchoose = value;
                        LoadData("Категории читателей", () => _context.ReaderCategorys.AsQueryable());
                        break;

                    case Tables.Status:
                        currentchoose = value;
                        LoadData("Статусы книг", () => _context.Statuss.AsQueryable());
                        break;

                    default:
                        MessageBox.Show("Таблица не найдена");
                        break;
                }
                currentchoose = value;
            }
        }

        private T GetAttribute<T>(PropertyDescriptor propertyDescriptor) where T : Attribute => propertyDescriptor.Attributes.OfType<T>().FirstOrDefault();


        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor? propertyDescriptor = e.PropertyDescriptor as PropertyDescriptor;

            HiddenAttribute hiddenAttribute = GetAttribute<HiddenAttribute>(propertyDescriptor);
            if (hiddenAttribute != null)
                e.Column.Visibility = Visibility.Collapsed;
            else
            {
                var columnNameAttr = GetAttribute<ColumnNameAttribute>(propertyDescriptor);
                if (columnNameAttr != null)
                    e.Column.Header = columnNameAttr.Name;

                if (propertyDescriptor.PropertyType == typeof(DateTime) || propertyDescriptor.PropertyType == typeof(DateTime?))
                {
                    DataGridTextColumn? column = e.Column as DataGridTextColumn;
                    if (column != null)
                        column.Binding.StringFormat = "{0:dd/MM/yyyy}";
                }
            }
        }

        private void LoadData<T>(string Name, Func<IQueryable<T>> getData) where T : class
        {
            try
            {
                var data = getData().ToList();
                dg1.ItemsSource = data;
                MainLabel.Content = Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }


        public MainWindow(LibraryDbContext _context, AccessCategory currAcess)
        {
            InitializeComponent();

            this._context = _context;

            CurrentChoose = Tables.Author;

            currentAcess = currAcess;
            lbb1.Text = currentAcess.Name;
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void MenuItem_Click_Authors(object sender, RoutedEventArgs e)
            => CurrentChoose = Tables.Author;
        

        private void MenuItem_Click_Books(object sender, RoutedEventArgs e) 
            => CurrentChoose = Tables.Book;

        private void MenuItem_Click_Publishers(object sender, RoutedEventArgs e)
            => CurrentChoose = Tables.Publisher;

        private void MenuItem_Click_Genres(object sender, RoutedEventArgs e)
            => CurrentChoose = Tables.Genre;


        private void MenuItem_Click_Readers(object sender, RoutedEventArgs e)
            => CurrentChoose = Tables.Reader;

        private void MenuItem_Click_Employees(object sender, RoutedEventArgs e)
        {
            if(currentAcess.AddAcess == false || currentAcess.EditAcess == false || currentAcess.DeleteAcess == false)
            {
                MessageBox.Show("Для доступа к этой таблице нужны права на добавление/редактирование/удаление");
                return;
            }
            CurrentChoose = Tables.Employee;

        }

        private void MenuItem_Click_Positions(object sender, RoutedEventArgs e)
            => CurrentChoose = Tables.Position;

        private void MenuItem_Click_BookOrders(object sender, RoutedEventArgs e)
            => CurrentChoose = Tables.BookOrder;

        private void MenuItem_Click_AccessCat(object sender, RoutedEventArgs e)
            => CurrentChoose = Tables.AccessCategory;

        private void MenuItem_Click_ReaderCat(object sender, RoutedEventArgs e)
            => CurrentChoose = Tables.ReaderCategory;

        private void MenuItem_Click_BooksStatuses(object sender, RoutedEventArgs e)
             => CurrentChoose = Tables.Status;

        #region Стиль кнопок

        bool pressed = false;

        private void Button1_MouseEnter(object sender, MouseEventArgs e) => SetGridColor((Grid)sender, 0, 156, 192); // синий
        private void Button1_MouseLeave(object sender, MouseEventArgs e) // тёмно серый
        {
            pressed = false;
            SetGridColor((Grid)sender, 39, 48, 56);
        }
        private void Button1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pressed = true;
            SetGridColor((Grid)sender, 138, 198, 212); // светло синий
        }
        private void SetGridColor(Grid g, byte R,byte G, byte B) => g.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        #endregion

        private void ReloadButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CurrentChoose = currentchoose;
            MessageBox.Show("Данные обновлены");
        }

        private void AddButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (currentAcess.AddAcess == false)
            {
                MessageBox.Show("Вашей категории запрещено данное действие");
                return;
            }

            ModelWindow modelBuilder = new ModelWindow(Enum.GetName(CurrentChoose), _context, null);
            modelBuilder.ShowDialog();
            CurrentChoose = currentchoose;
        }

        private void RemoveButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(currentAcess.DeleteAcess == false)
            {
                MessageBox.Show("Вашей категории запрещено данное действие");
                return;

            }
            if (dg1.SelectedItem == null)
            {
                MessageBox.Show("Выделите объект удаления");
                return;
            }
            if (MessageBox.Show("Вы уверены, что хотите удалить выделенные элементы?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                foreach (var item in dg1.SelectedItems)
                {
                    _context.Remove(item);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                CancelChanges();
            }
            CurrentChoose = currentchoose;
        }

        private void CancelChanges()
        {
            var changedEntries = _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted)
                .ToList();
            foreach (var entry in changedEntries)
            {
                if (entry.State == EntityState.Modified)
                    entry.State = EntityState.Unchanged;
                else if (entry.State == EntityState.Added)
                    entry.State = EntityState.Detached;
                else if (entry.State == EntityState.Deleted)
                    entry.Reload();
            }
        }

        private void EditButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (currentAcess.EditAcess == false)
            {
                MessageBox.Show("Вашей категории запрещено данное действие");
                return;

            }

            if (dg1.SelectedItem == null)
            {
                MessageBox.Show("Выделите объект редактирования");
                return;
            }
            if(dg1.SelectedItems.Count >1)
            {
                MessageBox.Show("За раз можно редактировать только 1 объект");
                return;
            }

            ModelWindow modelBuilder = new ModelWindow(Enum.GetName(CurrentChoose), _context, dg1.SelectedItem);
            modelBuilder.ShowDialog();
            CurrentChoose = currentchoose;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) // leave
        {
            new AuthWindow().Show();
            Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)//rawsql
        {
            if (currentAcess.EditAcess == false || currentAcess.AddAcess == false || currentAcess.DeleteAcess == false)
            {
                MessageBox.Show("Вашей категории запрещено данное действие");
                return;

            }
            new SqlWindow(_context).Show();

        }



        private void MenuItem_Click_2(object sender, RoutedEventArgs e) // Сформировать
        {
            try
            {
                string filePath = "Отчет.docx";

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                    Body body = new Body();

                    // Создание таблицы
                    Table table = new Table();

                    // Установка границ для таблицы
                    TableProperties tableProperties = new TableProperties(
                        new TableBorders(
                            new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 2 },
                            new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 2 },
                            new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 2 },
                            new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 2 },
                            new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 2 },
                            new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 2 }
                        )
                    );
                    table.AppendChild(tableProperties);

                    // Добавляем строку заголовков из DataGrid
                    TableRow headerRow = new TableRow();
                    foreach (var column in dg1.Columns)
                    {
                        TableCell headerCell = new TableCell(new Paragraph(new Run(new Text(column.Header.ToString()))));
                        headerRow.Append(headerCell);
                    }
                    table.Append(headerRow);

                    // Заполнение таблицы данными из DataGrid
                    foreach (var item in dg1.Items)
                    {
                        if (item is not null)
                        {
                            TableRow dataRow = new TableRow();

                            foreach (var column in dg1.Columns)
                            {
                                var cellValue = column.GetCellContent(item) as TextBlock;
                                TableCell dataCell = new TableCell(new Paragraph(new Run(new Text(cellValue?.Text ?? string.Empty))));
                                dataRow.Append(dataCell);
                            }

                            table.Append(dataRow);
                        }
                    }

                    body.Append(table);
                    mainPart.Document.Append(body);
                    mainPart.Document.Save();
                }

                MessageBox.Show($"Файл {filePath} успешно создан");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e) // Открыть
        {
            try
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e) // Печатать
        {
            try
            {
                string pdfFilePath = "Отчет.pdf";
                PrintDocx(pdfFilePath);
                Process.Start(new ProcessStartInfo(pdfFilePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void PrintDocx(string pdfFilePath)
        {

            // Создание PDF с использованием PdfSharp
            using (PdfDocument document = new PdfDocument())
            {
                document.Info.Title = "Отчёт";
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Verdana", 10);

                double margin = 10; // Отступ между ячейками
                double pageWidth = page.Width - 50; // Ширина страницы с учетом отступов
                double[] columnWidths; // Массив для ширины столбцов

                // Извлечение таблицы из документа Word
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
                {
                    var tables = wordDoc.MainDocumentPart.Document.Body.Elements<Table>();
                    foreach (var table in tables)
                    {
                        // Получаем количество столбцов
                        int columnCount = table.Elements<TableRow>().First().Elements<TableCell>().Count();
                        columnWidths = new double[columnCount];

                        // Первый проход для вычисления ширины столбцов и высоты строк
                        List<double> rowHeights = new List<double>();
                        foreach (var row in table.Elements<TableRow>())
                        {
                            double maxHeight = 0;
                            int cellIndex = 0;
                            foreach (var cell in row.Elements<TableCell>())
                            {
                                // Извлекаем текст из ячейки
                                string cellText = cell.InnerText;

                                // Вычисляем ширину текста
                                double textWidth = gfx.MeasureString(cellText, font).Width;

                                // Сохраняем максимальную ширину для каждого столбца
                                if (textWidth > columnWidths[cellIndex])
                                {
                                    columnWidths[cellIndex] = textWidth;
                                }

                                // Вычисляем высоту текста и обновляем максимальную высоту
                                double textHeight = gfx.MeasureString(cellText, font).Height;
                                if (textHeight > maxHeight)
                                {
                                    maxHeight = textHeight;
                                }

                                cellIndex++;
                            }
                            rowHeights.Add(maxHeight + margin); // Добавляем высоту строки
                        }
                        double y = 50;
                        foreach (var row in table.Elements<TableRow>())
                        {
                            double x = 50; 
                            int cellIndex = 0;

                            foreach (var cell in row.Elements<TableCell>())
                            {
                                string cellText = cell.InnerText;
                                double cellWidth = columnWidths[cellIndex] + margin;
                                gfx.DrawString(cellText, font, XBrushes.Black, new XRect(x, y, cellWidth, rowHeights[cellIndex]), XStringFormats.TopLeft);
                                x += cellWidth;
                                cellIndex++;
                            }

                            y += rowHeights[0]; 
                            document.AddPage();
                        }
                    }
                }

                document.Save(pdfFilePath);
            }
            MessageBox.Show("Документ pdf создан в указанном вами пути и готов к печати");
        }


        private async void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            await BackupDatabaseAsync(_context);
        }

        private async void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            await RestoreDatabaseAsync();
        }


        public async Task BackupDatabaseAsync(DbContext context)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Backup Files (*.bak)|*.bak",
                Title = "Сохранить резервную копию базы данных"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string backupFilePath = saveFileDialog.FileName;

                string query = $"BACKUP DATABASE [{context.Database.GetDbConnection().Database}] TO DISK = '{backupFilePath}'";

                await context.Database.ExecuteSqlRawAsync(query);
            }
        }

        public async Task RestoreDatabaseAsync()
        {
            MessageBox.Show("Ожидайте");
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Backup Files (*.bak)|*.bak",
                    Title = "Выберите файл резервной копии для восстановления"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string backupFilePath = openFileDialog.FileName;

                    // Получаем строку подключения к базе данных master
                    string masterConnectionString = ConfigurationManager.ConnectionStrings["MasterDbConnection"].ConnectionString;

                    // Получаем строку подключения к базе данных, которую мы восстанавливаем
                    string libraryDbConnectionString = ConfigurationManager.ConnectionStrings["LibraryDbConnection"].ConnectionString;

                    string dbName = new SqlConnectionStringBuilder(libraryDbConnectionString).InitialCatalog;

                    using (var connection = new SqlConnection(masterConnectionString))
                    {
                        await connection.OpenAsync();

                        // Переводим базу данных в однопользовательский режим для завершения всех активных сеансов
                        string setSingleUserQuery = $"ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                        using (var command = new SqlCommand(setSingleUserQuery, connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }

                        // Выполняем команду восстановления
                        string restoreQuery = $"RESTORE DATABASE [{dbName}] FROM DISK = '{backupFilePath}' WITH REPLACE;";
                        using (var command = new SqlCommand(restoreQuery, connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }

                        // Возвращаем базу данных в многопользовательский режим
                        string setMultiUserQuery = $"ALTER DATABASE [{dbName}] SET MULTI_USER;";
                        using (var command = new SqlCommand(setMultiUserQuery, connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
                CurrentChoose = currentchoose;
                MessageBox.Show("Готово");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



    }
}