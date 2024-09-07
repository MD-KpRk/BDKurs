using BDKurs.ModelControls;
using BDKurs.Models;
using System;
using System.Collections.Generic;
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

        public Params(string FieldName, Type PropertyType, string ColumnName) 
        { 
            this.FieldName = FieldName;
            this.PropertyType = PropertyType;
            this.ColumnName = ColumnName;
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

                if (atrname != null && property.GetCustomAttribute<HiddenAttribute>() == null && property.GetCustomAttribute<NonEditable>() == null)
                {

                    result.Add(new Params(property.Name, property.PropertyType, atrname));
                }
            }

            return result;
        }


        public ModelWindow(string typeName)
        {
            List<Params> paramslist = GetPropertyDetails(Type.GetType(typeName));


            InitializeComponent();

            foreach (var a in paramslist)
            {
                //
                stack.Children.Add(new StringTextBoxUserControl(a.ColumnName));
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
