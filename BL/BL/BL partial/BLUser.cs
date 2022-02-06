using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    internal partial class BL : BlApi.IBL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddUser(User user)
        {
            lock (dal)
            {
                try
                {
                    DO.User tmpUser = new();
                    object obj = tmpUser;//Boxing and unBoxing
                    user.CopyPropertiesTo(obj);
                    tmpUser = (DO.User)obj;
                    tmpUser.Longitude = user.Location.Longitude;
                    tmpUser.Latitude = user.Location.Latitude;
                    dal.Add(tmpUser);
                }
                catch (Exception ex)
                {
                    throw new ItemAlreadyExistsException(ex.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetListUsers(Predicate<User> predicate = null)
        {
            lock (dal)
            {
                IEnumerable<DO.User> usersDO = dal.GetListUsers();
                List<User> usersBO = new();
                usersDO.CopyPropertiesToIEnumerable(usersBO);
                return usersBO;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public User GetUser(string mail)
        {
            lock (dal)
            {
                try
                {
                    DO.User userDO = dal.GetUser(mail);
                    User userBO = new();
                    userDO.CopyPropertiesTo(userBO);
                    return userBO;
                }
                catch (ItemNotExistException ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void updateUser(string mail, string password)
        {
            lock (dal)
            {
                try
                {
                    dal.updateUser(mail, password);
                }
                catch (ItemNotExistException ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
            }
        }


    }
}
