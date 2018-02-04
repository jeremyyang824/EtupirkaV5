using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Etupirka.Domain.External.Entities.Vmes;

namespace Etupirka.Domain.External.Repositories
{
    /// <summary>
    /// 可视化MES接口
    /// </summary>
    public interface IVMESRepository : IRepository
    {
        /// <summary>
        /// 从FS同步PICK到可视化MES
        /// </summary>
        void SyncPickToVmes(SyncPickToVmesInput input);

        /// <summary>
        /// 指定生产订单行是否在MES中质检过
        /// </summary>
        bool IsInspected(IsInspectedInput input);
    }
}
