using BDKurs.Models;
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
    public partial class CheckBoxUserControl : UserControl
    {
        public Params par;

        public CheckBoxUserControl(Params _par)
        {
            par = _par;
            InitializeComponent();
            if (par.Req)
                lb.Content = par.ColumnName + "*";
            else
                lb.Content = par.ColumnName;





        }
    }
}
