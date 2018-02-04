using System;
using Abp.Dependency;

namespace Etupirka.Domain.External.Fsti
{
    /// <summary>
    /// ForthShift接口帮助类
    /// </summary>
    public interface IFSTIHelper : ITransientDependency
    {
        /// <summary>
        /// 登录并开始执行FSTI相关接口
        /// 通过using方式使用，自动注销FSTI
        /// </summary>
        FstiContext BeginFsti();

        /// <summary>
        /// 登录FS系统
        /// </summary>
        FstiToken FSLogin(string username, string password);

        /// <summary>
        /// 使用系统默认用户名密码登录
        /// </summary>
        FstiToken FSLogin();
        
        /// <summary>
        /// 注销FS系统
        /// </summary>
        void FSLogout(FstiToken fstiToken);
    }
}