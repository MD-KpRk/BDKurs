using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDKurs.Models
{
    public abstract class BDObject
    {
        public BDObject() { 
        
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
