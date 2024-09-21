using BDKurs.ModelControls;
using BDKurs.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
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
    public class Params
    {
        public string FieldName = "";
        public Type PropertyType = typeof(object);
        public string ColumnName = "";
        public int MaxLength = -1;
        public bool Req = false;

        public Params(string FieldName, Type PropertyType, string ColumnName, int MaxLength, bool Req)
        {
            this.FieldName = FieldName;
            this.PropertyType = PropertyType;
            this.ColumnName = ColumnName;
            this.MaxLength = MaxLength;
            this.Req = Req;
        }
    }

    public partial class ModelWindow : Window
    {
        List<TextBoxUserControl> tblist = new List<TextBoxUserControl>();
        List<DatePickerUserControl> dplist = new List<DatePickerUserControl>();
        List<ComboBoxUserControl> cblist = new List<ComboBoxUserControl>();
        List<CheckBoxUserControl> chblist = new List<CheckBoxUserControl>();

        LibraryDbContext _context;
        public Type ModelType;
        object? amogus;

        public List<Params> GetPropertyDetails(Type type)
        {
            List<Params> result = new List<Params>();
            foreach (var property in type.GetProperties())
            {
                string? atrname = property.GetCustomAttribute<ColumnNameAttribute>()?.Name;
                int? maxlength = property.GetCustomAttribute<MaxLengthAttribute>()?.Length;
                if (atrname != null && property.GetCustomAttribute<HiddenAttribute>() == null && property.GetCustomAttribute<NonEditable>() == null)
                {
                    int newlength;
                    bool req = false;
                    if (property.GetCustomAttribute<RequiredAttribute>() != null) req = true; 

                    if (maxlength == null) newlength = -1;
                    else newlength = (int)maxlength;

                    result.Add(new Params(property.Name, property.PropertyType, atrname, newlength, req));
                }
            }

            return result;
        }


        public ModelWindow(string typeName, LibraryDbContext context, object? obj)
        {
            amogus = obj;
            _context = context;
            ModelType = Type.GetType(typeName);
            List<Params> paramslist = GetPropertyDetails(ModelType);
            InitializeComponent();

            for(int i = 0; i< paramslist.Count();i++)
            {
                if (paramslist[i].PropertyType == typeof(string) || paramslist[i].PropertyType == typeof(int) || paramslist[i].PropertyType == typeof(int?))
                {
                    TextBoxUserControl newtb = new TextBoxUserControl(paramslist[i]);
                    if(obj!=null)
                        newtb.tb.Text = ModelType.GetProperty(paramslist[i].FieldName)?.GetValue(obj)?.ToString();
                    tblist.Add(newtb);
                    stack.Children.Add(newtb);
                }
                else if (paramslist[i].PropertyType == typeof(DateTime) || paramslist[i].PropertyType == typeof(DateTime?))
                {
                    DatePickerUserControl newdp = new DatePickerUserControl(paramslist[i]);
                    if (obj != null)
                        newdp.tb.SelectedDate = Convert.ToDateTime(ModelType.GetProperty(paramslist[i].FieldName)?.GetValue(obj));
                    dplist.Add(newdp);
                    stack.Children.Add(newdp);
                }
                else if (paramslist[i].PropertyType == typeof(bool))
                {
                    CheckBoxUserControl newchb = new CheckBoxUserControl(paramslist[i]);
                    if (obj != null)
                        newchb.tb.IsChecked = Convert.ToBoolean(ModelType.GetProperty(paramslist[i].FieldName)?.GetValue(obj));
                    chblist.Add(newchb);
                    stack.Children.Add(newchb);
                }
                else
                {
                    ComboBoxUserControl newcb = new ComboBoxUserControl(paramslist[i], _context);
                    if (obj != null)
                        newcb.tb.SelectedItem = ModelType.GetProperty(paramslist[i].FieldName)?.GetValue(obj);
                    cblist.Add(newcb);
                    stack.Children.Add(newcb);
                }
            }

        }

        public string ValidateData()
        {
            foreach(var a in tblist)
                if (a.par.Req && string.IsNullOrEmpty(a.tb.Text))
                    return "Не заполнены все обязательные поля";
            foreach (var a in dplist)
                if (a.par.Req && a.tb.SelectedDate == null)
                    return "Не заполнены все обязательные поля";
            foreach (var a in cblist)
                if (a.par.Req && a.tb.Items == null)
                    return "Не заполнены все обязательные поля";
            return "1";
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<Type, Action<object>> @switch = new Dictionary<Type, Action<object>> {
            { typeof(Author),           (obj) => CreateAuthor(obj) },
            { typeof(AccessCategory),   (obj) => CreateAccessCategory(obj)},
            { typeof(Book),             (obj) => CreateBook(obj)},
            { typeof(BookOrder),        (obj) => CreateBookOrder(obj)},
            { typeof(Employee),         (obj) => CreateEmployee(obj)},
            { typeof(Genre),            (obj) => CreateGenre(obj)},
            { typeof(Publisher),        (obj) => CreatePublisher(obj)},
            { typeof(Reader),           (obj) => CreateReader(obj)},
            { typeof(ReaderCategory),   (obj) => CreateReaderCategory(obj)},
            { typeof(Status),           (obj) => CreateStatus(obj)},
            { typeof(Position),         (obj) => CreatePosition(obj)},
            };
            string result = ValidateData();
            if(result!="1")
            {
                MessageBox.Show(result);
                return;
            }

            @switch[ModelType](amogus);
        }

        public void CreateAccessCategory(object? obj = null)
        {
            AccessCategory category;
            if(obj == null)
                category = new AccessCategory();
            else
                category = amogus as AccessCategory;

            category.Name = tblist[0].tb.Text;
            category.AddAcess = (bool)chblist[0].tb.IsChecked;
            category.EditAcess = (bool)chblist[1].tb.IsChecked;
            category.DeleteAcess = (bool)chblist[2].tb.IsChecked;
            if (obj == null)
                _context.AccessCategorys.Add(category);
            _context.SaveChanges();

            Close();
        }

        public void CreateAuthor(object? obj = null)
        {
            Author author;
            if (obj == null)
                author = new Author();
            else
                author = amogus as Author;
            author.FirstName = tblist[0].tb.Text;
            author.LastName = tblist[1].tb.Text;
            author.MiddleName = tblist[2].tb.Text;
            author.BirthDate = dplist[0].tb.SelectedDate;
            author.Gender = (Gender)cblist[0].tb.SelectedItem;
            if (obj == null)
                _context.Authors.Add(author);
            _context.SaveChanges();
            Close();
        }

        public void CreateBook(object? obj = null)
        {
            Book book;
            if (obj == null)
                book = new Book();
            else
                book = amogus as Book;
            book.ISBN = tblist[0].tb.Text;
            book.Title = tblist[1].tb.Text;
            if (string.IsNullOrEmpty(tblist[2].tb.Text))
                book.PublicationYear = null;
            else
                book.PublicationYear = Convert.ToInt32(tblist[2].tb.Text);
            if(cblist[0].tb.SelectedItem != null)
                book.Publisher = (Publisher)cblist[0].tb.SelectedItem;
            if(cblist[1].tb.SelectedItem != null)
                book.Author = (Author)cblist[1].tb.SelectedItem;
            if(cblist[2].tb.SelectedItem != null)
                book.Status = (Status)cblist[2].tb.SelectedItem;
            if(cblist[3].tb.SelectedItem != null)
                book.Genre = (Genre)cblist[3].tb.SelectedItem;
            if (obj == null)
                _context.Books.Add(book);
            _context.SaveChanges();
            Close();
        }

        public void CreateBookOrder(object? obj = null)
        {
            BookOrder bookOrder;
            if (obj == null)
                bookOrder = new BookOrder();
            else
                bookOrder = amogus as BookOrder;
            bookOrder.BookOrderDate = (DateTime)dplist[0].tb.SelectedDate;
            bookOrder.ReturnDate = (DateTime)dplist[1].tb.SelectedDate;
            bookOrder.ActualReturnDate = dplist[2].tb.SelectedDate;
            bookOrder.Book = (Book)cblist[0].tb.SelectedItem;
            bookOrder.Reader = (Reader)cblist[1].tb.SelectedItem;
            bookOrder.Employee = (Employee)cblist[2].tb.SelectedItem;
            if (obj == null)
                _context.BookOrders.Add(bookOrder);
            _context.SaveChanges();
            Close();
        }

        public void CreateEmployee(object? obj = null)
        {
            Employee employee;
            if (obj == null)
                employee = new Employee();
            else
                employee = amogus as Employee;
            employee.FirstName = tblist[0].tb.Text;
            employee.LastName = tblist[1].tb.Text;
            employee.MiddleName = tblist[2].tb.Text;
            employee.Passw = tblist[3].tb.Text;
            employee.Phone = tblist[4].tb.Text;
            employee.Email = tblist[5].tb.Text;
            employee.Gender = (Gender)cblist[0].tb.SelectedItem;
            employee.Position = (Position)cblist[1].tb.SelectedItem;
            employee.AccessCategory = (AccessCategory)cblist[2].tb.SelectedItem;

            if (obj == null)
                _context.Employees.Add(employee);
            _context.SaveChanges();
            Close();
        }

        public void CreateGenre(object? obj = null)
        {
            Genre genre;
            if (obj == null)
                genre = new Genre();
            else
                genre = amogus as Genre;
            genre.Name = tblist[0].tb.Text;

            if (obj == null)
                _context.Genres.Add(genre);
            _context.SaveChanges();
            Close();
        }

        public void CreatePosition(object? obj = null)
        {
            Position pos;
            if (obj == null)
                pos = new Position();
            else
                pos = amogus as Position;
            pos.Name = tblist[0].tb.Text;
            if (obj == null)
                _context.Positions.Add(pos);
            _context.SaveChanges();
            Close();
        }

        public void CreatePublisher(object? obj = null)
        {
            Publisher publisher;
            if (obj == null)
                publisher = new Publisher();
            else
                publisher = amogus as Publisher;
            publisher.Name = tblist[0].tb.Text;
            publisher.Address = tblist[1].tb.Text;
            publisher.Phone = tblist[2].tb.Text;
            publisher.Email = tblist[3].tb.Text;

            if (obj == null)
                _context.Publishers.Add(publisher);
            _context.SaveChanges();
            Close();
        }

        public void CreateReader(object? obj = null)
        {
            Reader reader;
            if (obj == null)
                reader = new Reader();
            else
                reader = amogus as Reader;
            reader.FirstName = tblist[0].tb.Text;
            reader.LastName = tblist[1].tb.Text;
            reader.MiddleName = tblist[2].tb.Text;
            if(dplist[0].tb.SelectedDate != null)
                reader.BirthDate = dplist[0].tb.SelectedDate;
            reader.Address = tblist[3].tb.Text;
            reader.Phone = tblist[4].tb.Text;
            reader.Email = tblist[5].tb.Text;

            reader.Gender = (Gender)cblist[0].tb.SelectedItem;
            reader.ReaderCategory = (ReaderCategory)cblist[1].tb.SelectedItem;
            if (obj == null)
                _context.Readers.Add(reader);
            _context.SaveChanges();
            Close();
        }

        public void CreateReaderCategory(object? obj = null)
        {
            ReaderCategory readerCategory;
            if (obj == null)
                readerCategory = new ReaderCategory();
            else
                readerCategory = amogus as ReaderCategory;
            readerCategory.Name = tblist[0].tb.Text;
            if (obj == null)
                _context.ReaderCategorys.Add(readerCategory);
            _context.SaveChanges();
            Close();
        }
        public void CreateStatus(object? obj = null)
        {
            Status status;
            if (obj == null)
                status = new Status();
            else
                status = amogus as Status;
            status.Name = tblist[0].tb.Text;

            if (obj == null)
                _context.Statuss.Add(status);
            _context.SaveChanges();
            Close();
        }

    }
}
