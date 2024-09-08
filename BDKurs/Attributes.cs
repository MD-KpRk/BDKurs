using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDKurs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        public string Name { get; set; }
        public ColumnNameAttribute(string Name) { this.Name = Name; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class HiddenAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class NonEditable : Attribute { }
}
