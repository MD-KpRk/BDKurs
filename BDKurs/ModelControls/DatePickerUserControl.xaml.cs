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
    /// Логика взаимодействия для DatePickerUserControl.xaml
    /// </summary>
    public partial class DatePickerUserControl : UserControl
    {
        public DatePickerUserControl(Params par)
        {
            InitializeComponent();
            if (par.Req)
                lb.Content = par.ColumnName + "*";
            else
                lb.Content = par.ColumnName;

        }
    }
}
