using System;

namespace DO
{
    [Serializable]
    public class ItemNotExistException: Exception
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

    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString()
        {
            return base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
        }
    }
    public class AskRecoverException : Exception
    {
        public string xmlFilePath;
        public AskRecoverException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public AskRecoverException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public AskRecoverException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString()
        {
            return base.ToString() + $", does have been removed, Sure you want to recover? {xmlFilePath}";
        }
    }
}
