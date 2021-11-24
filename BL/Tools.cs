using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;
using BL;



namespace BL
{
    public static class Tools
    {       
        public static void CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                {
                    continue;
                }
                object value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                {
                    propTo.SetValue(to, value);
                }
            }
        }
        public static void CopyPropertiesToIEnumerable<T, S>(this IEnumerable<S> from, List<T> to)
            where T : new()
        {
            foreach (S s in from)
            {
                T t = new ();
                s.CopyPropertiesTo(t);
                to.Add(t);
            }
        }
    }
}

