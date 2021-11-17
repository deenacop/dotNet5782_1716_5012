using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    [Serializable]
    public class ItemNotExistException : Exception
    {
        public ItemNotExistException() : base() { }
        public ItemNotExistException(string message) : base(message) { }
        public ItemNotExistException(string message, Exception inner) : base(message, inner) { }
        public override string ToString()
        {
            return Message;
        }
    }

        [Serializable]
        public class AlreadyExistedItemException : Exception
        {
            public AlreadyExistedItemException() : base() { }
            public AlreadyExistedItemException(string message) : base(message) { }
            public AlreadyExistedItemException(string message, Exception inner) : base(message, inner) { }
            public override string ToString()
            {
                return Message;
            }
        }
        [Serializable]
        public class UnlogicalLocation:Exception
        {
            public UnlogicalLocation() : base() { }
            public UnlogicalLocation(string message) : base(message) { }
            public UnlogicalLocation(string message, Exception inner) : base(message, inner) { }
            public override string ToString()
            {
                return Message;
            }
        }
    
}
