using System;
using Abp.Dependency;

namespace Etupirka.Domain.External.Fsti
{
    /// <summary>
    /// ForthShift�ӿڰ�����
    /// </summary>
    public interface IFSTIHelper : ITransientDependency
    {
        /// <summary>
        /// ��¼����ʼִ��FSTI��ؽӿ�
        /// ͨ��using��ʽʹ�ã��Զ�ע��FSTI
        /// </summary>
        FstiContext BeginFsti();

        /// <summary>
        /// ��¼FSϵͳ
        /// </summary>
        FstiToken FSLogin(string username, string password);

        /// <summary>
        /// ʹ��ϵͳĬ���û��������¼
        /// </summary>
        FstiToken FSLogin();
        
        /// <summary>
        /// ע��FSϵͳ
        /// </summary>
        void FSLogout(FstiToken fstiToken);
    }
}