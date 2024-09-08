using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDKurs.ModelControls
{
    /// <summary>
    /// Логика взаимодействия для ComboBoxUserControl.xaml
    /// </summary>
    public partial class ComboBoxUserControl : UserControl
    {
        public ComboBoxUserControl(Params par, LibraryDbContext _context)
        {
            InitializeComponent();
            if (par.Req)
                lb.Content = par.ColumnName + "*";
            else
                lb.Content = par.ColumnName;

            MessageBox.Show(_context.Genders.Count().ToString() + "  name: "+par.FieldName);
            MessageBox.Show(LibraryDbContext.GetDbSetByName((par.FieldName+"s"),_context).ToString());
            tb.ItemsSource = LibraryDbContext.GetDbSetByName((par.FieldName + "s"), _context);
        }
    }
}
