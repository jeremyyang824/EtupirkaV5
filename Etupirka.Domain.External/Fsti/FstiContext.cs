using System;

namespace Etupirka.Domain.External.Fsti
{
    public abstract class FstiContext : IDisposable
    {
        /// <summary>
        /// FSTI登录信息
        /// </summary>
        public FstiToken FstiToken { get; private set; }

        protected FstiContext(FstiToken fstiToken)
        {
            if (fstiToken == null)
                throw new ArgumentNullException("fstiToken");

            this.FstiToken = fstiToken;
        }

        public abstract void Dispose();
    }
}
