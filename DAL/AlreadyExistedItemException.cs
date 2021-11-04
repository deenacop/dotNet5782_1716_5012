using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.DO
{
    public class AlreadyExistedItemException : Exception
    {
        public AlreadyExistedItemException() : base() { }
        public AlreadyExistedItemException(string message) : base(message) { }
        public AlreadyExistedItemException(string message, Exception inner) : base(message, inner) {}

    }
}
