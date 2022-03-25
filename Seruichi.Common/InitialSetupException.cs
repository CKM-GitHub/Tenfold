using System;

namespace Seruichi.Common
{
    public class InitialSetupException : Exception
    {
        public InitialSetupException() : base() { }

        public InitialSetupException(string message) : base(message) {}
    }
}
