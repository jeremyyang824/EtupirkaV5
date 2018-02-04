using Abp.AutoMapper;
using Etupirka.Domain.External.Entities.Dmes;

namespace Etupirka.Application.Manufacture.DispatchedManage.Dto
{
    /// <summary>
    /// 工作中心
    /// </summary>
    public class WorkCenterOutput 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string WorkCenterId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string WorkCenterName { get; set; }

        /// <summary>
        /// 所属工厂
        /// </summary>
        public string ProductionPlant { get; set; }
    }
}
