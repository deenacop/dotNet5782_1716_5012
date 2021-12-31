using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    internal partial class BL : BlApi.IBL
    {
        public void AddUser(User user)
        {
            try
            {
                DO.User tmpUser = new();
                object obj = tmpUser;//Boxing and unBoxing
                user.CopyPropertiesTo(obj);
                tmpUser = (DO.User)obj;
                dal.Add(tmpUser);
            }
            catch (Exception ex)
            {
                throw new ItemAlreadyExistsException(ex.Message);
            }
        }
        public IEnumerable<User> GetListUsers(Predicate<User> predicate = null)
        {
            IEnumerable<DO.User> usersDO = dal.GetListUsers();
            List<User> usersBO = new();
            usersDO.CopyPropertiesToIEnumerable(usersBO);
            return usersBO;
        }
        public User GetUser(string mail)
        {
            try
            {
                DO.User userDO = dal.GetUser(mail);
                User userBO = new();
                userDO.CopyPropertiesTo(userBO);
                return userBO;
            }
            catch (Exception ex)
            {
                throw new ItemNotExistException(ex.Message);
            }

        }


    }
}
