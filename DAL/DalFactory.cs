using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using DO;

namespace DAL
{
    public static class DalFactory
    {
        public static DalApi GetDal(string str)
        {
            DalApi dal;
            if(str=="DalObject")
                return dal = new DalObject.DalObject();
            throw new WrongInputException("wrong input");
        }
    }


}

