﻿using BDKurs.Models;
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
        private void SetGridColor(Grid g, byte R,byte G, byte B) => g.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
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

        }
    }
}