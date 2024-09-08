using BDKurs.ModelControls;
using BDKurs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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


        public ModelWindow(string typeName, LibraryDbContext _context)
        {
            List<Params> paramslist = GetPropertyDetails(Type.GetType(typeName));
            InitializeComponent();

            foreach (Params abc in paramslist)
            {
                //
                if (abc.PropertyType == typeof(string) || abc.PropertyType == typeof(int) || abc.PropertyType == typeof(int?))
                    stack.Children.Add(new TextBoxUserControl(abc));
                else if (abc.PropertyType == typeof(DateTime))
                    stack.Children.Add(new DatePickerUserControl(abc));
                else stack.Children.Add(new ComboBoxUserControl(abc, _context));
                //else MessageBox.Show("Ошибка процедурной генерации окна " + abc.PropertyType);
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
