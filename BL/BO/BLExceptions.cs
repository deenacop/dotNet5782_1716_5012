using System;

namespace BO
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
    public class ItemAlreadyExistsException : Exception
    {
        public ItemAlreadyExistsException() : base() { }
        public ItemAlreadyExistsException(string message) : base(message) { }
        public ItemAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
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
    public class WorngStatusException : Exception
    {
        public WorngStatusException() : base() { }
        public WorngStatusException(string message) : base(message) { }
        public WorngStatusException(string message, Exception inner) : base(message, inner) { }
        public override string ToString()
        {
            return Message;
        }
    }
    [Serializable]
    public class NegetiveException : Exception
    {
        public NegetiveException() : base() { }
        public NegetiveException(string message) : base(message) { }
        public NegetiveException(string message, Exception inner) : base(message, inner) { }
        public override string ToString()
        {
            return Message;
        }
    }
    [Serializable]
    public class WrongInputException : Exception
    {
        public WrongInputException() : base() { }
        public WrongInputException(string message) : base(message) { }
        public WrongInputException(string message, Exception inner) : base(message, inner) { }
        public override string ToString()
        {
            return Message;
        }
    }
    public class ItemHasBeenAssociated : Exception
    {
        public ItemHasBeenAssociated() : base() { }
        public ItemHasBeenAssociated(string message) : base(message) { }
        public ItemHasBeenAssociated(string message, Exception inner) : base(message, inner) { }
        public override string ToString()
        {
            return Message;
        }
    }
}
