using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.DO
{
    class ItemNotExistException: Exception
    {
        public ItemNotExistException() : base() { }
        public ItemNotExistException(string message) : base(message) { }
        public ItemNotExistException(string message, Exception inner) : base(message, inner) { }
    }
}
