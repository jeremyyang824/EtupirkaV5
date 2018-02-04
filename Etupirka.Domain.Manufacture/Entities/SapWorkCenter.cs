using System;
using Abp.Domain.Entities.Auditing;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// SAP工作中心定义
    /// </summary>
    public class SapWorkCenter : AuditedEntity<Guid>
    {
        /// <summary>
        /// 工作中心代码
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心描述
        /// </summary>
        public string WorkCenterName { get; set; }

        /// <summary>
        /// 所属工厂
        /// </summary>
        public string ProductionPlant { get; set; }

        /// <summary>
        /// CIM资源的对象类型
        /// </summary>
        public string OBJTY { get; set; }

        /// <summary>
        /// 资源的对象ID
        /// </summary>
        public string OBJID { get; set; }

        /// <summary>
        /// 任务清单使用码
        /// </summary>
        public string PLANV { get; set; }

        /// <summary>
        /// 工作中心类别
        /// </summary>
        public string VERWE { get; set; }

        /// <summary>
        /// 工作中心锁定
        /// </summary>
        public string XSPRR { get; set; }

        /// <summary>
        /// 工作中心删除
        /// </summary>
        public string LVORM { get; set; }

        /// <summary>
        /// 能力类别
        /// </summary>
        public string KAPAR { get; set; }

        /// <summary>
        /// 能力基本计量单位	
        /// </summary>
        public string MEINS { get; set; }

        /// <summary>
        /// 更改日期
        /// </summary>
        public DateTime? AEDAT_GRND { get; set; }

        /// <summary>
        /// 更改用户名
        /// </summary>
        public string AENAM_GRND { get; set; }
    }
}
