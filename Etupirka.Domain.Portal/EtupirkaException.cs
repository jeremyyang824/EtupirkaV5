using System;
using System.Runtime.InteropServices;

namespace Etupirka.Domain.Portal
{
    /// <summary>
    /// 系统异常
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Exception))]
    public class EtupirkaException : Exception
    {
        public EtupirkaException() : base() { }

        public EtupirkaException(string message) : base(message) { }

        public EtupirkaException(string message, Exception innerException) : base(message, innerException) { }

        public EtupirkaException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
