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
    public partial class ComboBoxUserControl : UserControl
    {
        public Params par;


        public ComboBoxUserControl(Params _par, LibraryDbContext _context)
        {
            par = _par;
            InitializeComponent();
            if (par.Req)
                lb.Content = par.ColumnName + "*";
            else
                lb.Content = par.ColumnName;

            Repository repository = new Repository(_context);
            List<BDObject> objectList = repository.GetDbSetByName(par.FieldName+"s");

            tb.ItemsSource = objectList;

            if(objectList != null && objectList.Count()>0 && par.Req)
            {
                tb.SelectedItem = objectList[0];
            }

        }
    }
}
