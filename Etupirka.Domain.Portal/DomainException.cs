using System;
using System.Runtime.InteropServices;

namespace Etupirka.Domain.Portal
{
    [Serializable]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Exception))]
    public class DomainException : EtupirkaException
    {
        public DomainException() : base() { }

        public DomainException(string message) : base(message) { }

        public DomainException(string message, Exception innerException) : base(message, innerException) { }

        public DomainException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
