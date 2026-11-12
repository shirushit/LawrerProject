using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LawyerProject.DB;
using LawyerProject.GUI;
using LawyerProject.BLL;


namespace LawyerProject.DB
{
    public partial class Tikim_Letipul_Orech_Din
    {
       public Customers getCustomerNegdi()
        { 
        DB.LawyerDBEntities1 d= new LawyerDBEntities1();
        List<Customers> lst = new List<Customers>();
            lst = d.Customers.ToList();
            //לקוח נגדי
          return  lst.FirstOrDefault(x => x.IDcustemer == this.getCustomerNegdi().IDcustemer);  
        }
        public Customers getOrechDinNegdi()
        {
            DB.LawyerDBEntities1 d = new LawyerDBEntities1();
            List<Customers> lst = new List<Customers>();
            lst = d.Customers.ToList();
            //לקוח נגדי
            return lst.FirstOrDefault(x => x.IDcustemer == this.getOrechDinNegdi().IDcustemer);
        }
    }
}
