using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public partial class Employees
    {
        public string FIO
        {
            get
            {
                return surname + " " + name[0] + "." + patronomic[0] + ".";
            }
        }
    }
}
