﻿using System;
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
                catch (Exception ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
            }

        }
        public void updateUser(string mail, string password)
        {
            lock (dal)
            {
                try
                {
                    dal.updateUser(mail, password);
                }
                catch (Exception ex)
                {
                    throw new ItemNotExistException(ex.Message);
                }
            }
        }


    }
}
