using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.DB
{
   public partial class Customers
    {
        public override string ToString()
        {
            return LastName+ " " +FirstName;
        }
       
    }
}
