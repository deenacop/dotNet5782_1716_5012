using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public static class Tools
    {
        /// <summary>
        /// Copy properties (value type only) to destination object
        /// according to property name (i.e. only values of same name value type
        /// properties are copied)
        /// </summary>
        /// <typeparam name="T">any type</typeparam>
        /// <typeparam name="S">any type</typeparam>
        /// <param name="from">source object</param>
        /// <param name="to">destination object</param>
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
        /// <summary>
        ///Copy properties (value type only) to destination object
        /// according to property name (i.e. only values of same name value type
        /// properties are copied) 
        /// </summary>
        /// <typeparam name="T">any type</typeparam>
        /// <typeparam name="S">any type</typeparam>
        /// <param name="from">source objects</param>
        /// <param name="to">destination objects</param>
        public static void CopyPropertiesToIEnumerable<T, S>(this IEnumerable<S> from, List<T> to)
            where T : new()
        {
            foreach (S s in from)
            {
                T t = new();
                s.CopyPropertiesTo(t);
                to.Add(t);
            }
        }
    }
}


