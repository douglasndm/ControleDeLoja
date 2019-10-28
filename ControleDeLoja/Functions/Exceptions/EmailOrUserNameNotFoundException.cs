using System;

namespace ControleDeLoja.Functions.Exceptions
{
    public class EmailOrUserNameNotFoundException : Exception
    {
        public EmailOrUserNameNotFoundException() { }
        public EmailOrUserNameNotFoundException(string message) : base(message) { }
        public EmailOrUserNameNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
