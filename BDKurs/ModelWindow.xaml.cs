using BDKurs.ModelControls;
using BDKurs.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

        public List<Params> GetPropertyDetails(Type type)
        {
            var result = new List<Params>();

            // Перебор всех свойств типа
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


        public ModelWindow(string typeName, LibraryDbContext context)
        {
            _context = context;
            ModelType = Type.GetType(typeName);
            List<Params> paramslist = GetPropertyDetails(ModelType);
            InitializeComponent();

            foreach (Params abc in paramslist)
            {
                //
                if (abc.PropertyType == typeof(string) || abc.PropertyType == typeof(int) || abc.PropertyType == typeof(int?))
                {
                    TextBoxUserControl newtb = new TextBoxUserControl(abc);
                    tblist.Add(newtb);
                    stack.Children.Add(newtb);
                }
                else if (abc.PropertyType == typeof(DateTime) || abc.PropertyType == typeof(DateTime?))
                {
                    DatePickerUserControl newdp = new DatePickerUserControl(abc);
                    dplist.Add(newdp);
                    stack.Children.Add(newdp);
                }
                else if (abc.PropertyType == typeof(bool))
                {
                    CheckBoxUserControl newchb = new CheckBoxUserControl(abc);
                    chblist.Add(newchb);
                    stack.Children.Add(newchb);
                }
                else
                {
                    ComboBoxUserControl newcb = new ComboBoxUserControl(abc,_context);
                    cblist.Add(newcb);

                    stack.Children.Add(newcb);
                }
                //else MessageBox.Show("Ошибка процедурной генерации окна " + abc.PropertyType);
            }

        }

        public string ValidateData()
        {
            foreach(var a in tblist)
            {
                if (a.par.Req && string.IsNullOrEmpty(a.tb.Text))
                    return "Не заполнены все обязательные поля";
            }
            foreach (var a in dplist)
            {
                if (a.par.Req && a.tb.SelectedDate == null)
                    return "Не заполнены все обязательные поля";
            }
            foreach (var a in cblist)
            {
                if (a.par.Req && a.tb.Items == null)
                    return "Не заполнены все обязательные поля";
            }

            return "1";
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<Type, Action> @switch = new Dictionary<Type, Action> {
            { typeof(Author), () => CreateAuthor() },
            { typeof(AccessCategory), () => CreateAccessCategory()},
            };

            string result = ValidateData();
            if(result!="1")
            {
                MessageBox.Show(result);
                return;
            }

            

            @switch[ModelType]();

        }

        public void CreateAuthor()
        {
            Author author = new Author();
            author.FirstName = tblist[0].tb.Text;
            author.LastName = tblist[1].tb.Text;
            author.MiddleName = tblist[2].tb.Text;
            author.BirthDate = dplist[0].tb.SelectedDate;

            author.Gender = (Gender)cblist[0].tb.SelectedItem;

            _context.Authors.Add(author);
            _context.SaveChanges();
            Close();
        }

        public void CreateAccessCategory()
        {
            AccessCategory category = new AccessCategory();

            category.Name = tblist[0].tb.Text;

            category.AddAcess = (bool)chblist[0].tb.IsChecked;
            category.EditAcess = (bool)chblist[1].tb.IsChecked;
            category.DeleteAcess = (bool)chblist[2].tb.IsChecked;

            _context.AccessCategorys.Add(category);
            _context.SaveChanges();
            Close ();

        }
    }
}
