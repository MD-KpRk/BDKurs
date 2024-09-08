using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
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
    public partial class TextBoxUserControl : UserControl
    {
        public TextBoxUserControl(Params par)
        {
            InitializeComponent();

            if(par.Req )
                lb.Content = par.ColumnName+"*";
            else
                lb.Content = par.ColumnName;

            if (par.MaxLength == -1) tb.MaxLength=8;

            else
            {
                tb.MaxLength = par.MaxLength;
            }

            if (par.MaxLength > 40 && par.MaxLength <= 160)
            {
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Height = 60;
                tb.VerticalContentAlignment = VerticalAlignment.Top;
            }
            else if (par.MaxLength > 160)
            {
                tb.VerticalContentAlignment = VerticalAlignment.Top;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Height = 90;
            }

        }


    }
}
