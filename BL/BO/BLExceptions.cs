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
    public class WrongIDException : Exception
    {
        public WrongIDException() : base() { }
        public WrongIDException(string message) : base(message) { }
        public WrongIDException(string message, Exception inner) : base(message, inner) { }
        public override string ToString()
        {
            return Message;
        }
    }

    [Serializable]
    public class UnlogicalLocationException : Exception
    {
        public UnlogicalLocationException() : base() { }
        public UnlogicalLocationException(string message) : base(message) { }
        public UnlogicalLocationException(string message, Exception inner) : base(message, inner) { }
        public override string ToString()
        {
            return Message;
        }
    }

    [Serializable]
    public class NotEnoughBatteryException : Exception
    {
        public NotEnoughBatteryException() : base() { }
        public NotEnoughBatteryException(string message) : base(message) { }
        public NotEnoughBatteryException(string message, Exception inner) : base(message, inner) { }
        public override string ToString()
        {
            return Message;
        }
    }

    [Serializable]
    public class NotAvailableException : Exception
    {
        public NotAvailableException() : base() { }
        public NotAvailableException(string message) : base(message) { }
        public NotAvailableException(string message, Exception inner) : base(message, inner) { }
        public override string ToString()
        {
            return Message;
        }
    }
}
