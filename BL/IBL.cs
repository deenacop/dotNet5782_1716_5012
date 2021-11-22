using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        public IEnumerable<CustomerToList> ListCustomerDsplay();
        public Customer CustomerDisplay(int ID);
        public void UpdateCustomer(int ID, string name = null, string phone = null);
        public void AddCustomer(Customer customer);




    }
}
