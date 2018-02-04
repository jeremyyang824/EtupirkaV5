using Abp;
using Abp.Domain.Services;
using Abp.Events.Bus;

namespace Etupirka.Domain.Portal
{
    /// <summary>
    /// 领域服务基类
    /// </summary>
    public abstract class EtupirkaDomainServiceBase : DomainService
    {
        /// <summary>
        /// 事件总线
        /// </summary>
        public IEventBus EventBus { get; set; }

        /// <summary>
        /// GUID生成器
        /// </summary>
        public IGuidGenerator GuidGenerator { get; set; }

        protected EtupirkaDomainServiceBase()
        {
            LocalizationSourceName = EtupirkaPortalConsts.LocalizationSourceName;
            EventBus = NullEventBus.Instance;
        }
    }
}
