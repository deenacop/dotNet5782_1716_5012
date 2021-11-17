using System.Text;
using System.Threading.Tasks;
using DalObject;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        public void AddCustomer(Customer customer)
        {
            if (ChackingNumOfDigits(customer.CustomerID) != 9)
                throw new WrongIDException("Worng ID");
            if (customer.CustomerLocation.Latitude < 31 || customer.CustomerLocation.Latitude > 32
             || customer.CustomerLocation.Longitude < 35 || customer.CustomerLocation.Longitude > 36)
                throw new UnlogicalLocation("The location is not logical");
            try
            {
                IDAL.DO.Customer customerDO = new IDAL.DO.Customer();
                customer.CopyPropertiesTo(customerDO);
                dal.Add(customerDO);
            }
            catch (IDAL.DO.AlreadyExistedItemException ex)
            {
                throw new AlreadyExistedItemException(ex.Message);
            }
        }
    }
}
