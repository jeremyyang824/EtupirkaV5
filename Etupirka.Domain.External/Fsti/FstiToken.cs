using System;

namespace Etupirka.Domain.External.Fsti
{
    /// <summary>
    /// 缓存FS用户名、密码的登录Token
    /// </summary>
    [Serializable]
    public sealed class FstiToken
    {
        public string UserName { get;  }
        public string Password { get;  }
        public Guid Token { get; }

        public FstiToken(string userName, string password, Guid token)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("userName");
            if (password == null)
                throw new ArgumentNullException("password");

            this.UserName = userName.Trim();
            this.Password = password.Trim();
            this.Token = token;
        }

        /// <summary>
        /// 缓存键
        /// </summary>
        public string CacheItemKey
        {
            get { return $"{UserName}@{Password}"; }
        }

        /// <summary>
        /// 缓存键
        /// </summary>
        public static string GetCacheItemKey(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("userName");
            if (password == null)
                throw new ArgumentNullException("password");

            return $"{userName.Trim()}@{password.Trim()}";
        }

        public override string ToString()
        {
            return $"{UserName}@{Password}@{Token}";
        }
    }
}
