using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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

namespace BDKurs
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private readonly LibraryDbContext _context;
        public AuthWindow()
        {
            InitializeComponent();

            if (App.ServiceProvider == null)
                throw new InvalidOperationException("ServiceProvider не был инициализирован.");
            _context = App.ServiceProvider.GetRequiredService<LibraryDbContext>();
            if (_context == null)
                throw new InvalidOperationException("LibraryDbContext не был зарегистрирован.");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (cb.SelectedItem == null) return;

            Employee currentEmp = (Employee)cb.SelectedItem;

            if (currentEmp.Passw == tb3.Password)
            {

                new MainWindow(_context, currentEmp.AccessCategory).Show();
                Close();

            }
            else
            {
                MessageBox.Show("Указан неправильный пароль","Ошибка",MessageBoxButton.OK,MessageBoxImage.Stop);
            }
            // if passed
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Employee> list = _context.Employees.Include(a => a.AccessCategory).ToList();

                if (list != null && list.Count > 0)
                {
                    cb.ItemsSource = list;
                    cb.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Возникла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
