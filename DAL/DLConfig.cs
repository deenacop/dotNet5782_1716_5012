using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DalApi
{
    internal static class DLConfig
    {
        /// <summary>
        /// a new type decleration for xml reading
        /// </summary>
        public class DLPackage
        {
            public string Name;
            public string PkgName;
            public string NameSpace;
            public string ClassName;
        }

        internal static string DLName;
        internal static Dictionary<string, DLPackage> DLPackages;

        /// <summary>
        /// Static constructor extracts Dal packages list and Dal type from
        /// Dal configuration file config.xml
        /// </summary>
        static DLConfig()
        {
            XElement dlConfig = XElement.Load(@"xml\‏‏config.xml");
            DLName = dlConfig.Element("dal").Value;
            DLPackages = (from pkg in dlConfig.Element("dal-packages").Elements()
                          let tmp1 = pkg.Attribute("namespace")
                          let nameSpace = tmp1 == null ? "DL" : tmp1.Value
                          let tmp2 = pkg.Attribute("class")
                          let className = tmp2 == null ? pkg.Value : tmp2.Value
                          select new DLPackage()
                          {
                              Name = "" + pkg.Name,
                              PkgName = pkg.Value,
                              NameSpace = nameSpace,
                              ClassName = className
                          })
                           .ToDictionary(p => "" + p.Name, p => p);
        }
    }

    /// <summary>
    /// Represents errors during DalApi initialization
    /// </summary>
    [Serializable]
    public class DLConfigException : Exception
    {
        public DLConfigException(string message) : base(message) { }
        public DLConfigException(string message, Exception inner) : base(message, inner) { }
    }
}