using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public class DalObject
        {
            public void Add(Drone NewDrone)
            {
                DataSource.Drones[DataSource.Config.FirstAvailableDrone++] = NewDrone;
            }
            public void Add(Station NewStation)
            {
                DataSource.Stations[DataSource.Config.FirstAvailableStation++] = NewStation;
            }
            public void Add(Parcel NewParcel)
            {
                DataSource.Parcels[DataSource.Config.FirstAvailableParcel++] = NewParcel;
            }
            public void Add(Customer NewCustomer)
            {
                DataSource.Customers[DataSource.Config.FirstAvailableCustomer++] = NewCustomer;
            }
            
            public void AssignParcelToDrone (int ParcelID, int CustomerID)
            {
                for(int i=0;i<DataSource.Config.FirstAvailableParcel;i++)
                {

                }
            }
        }
    }
    
}
