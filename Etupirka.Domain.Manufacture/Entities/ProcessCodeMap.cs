using System;
using Abp.Domain.Entities.Auditing;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// FS/SAP工艺名称映射
    /// </summary>
    public class ProcessCodeMap : AuditedEntity
    {
        /// <summary>
        /// SAP工艺代码
        /// </summary>
        public string SapProcessCode { get; set; }

        /// <summary>
        /// SAP工艺名
        /// </summary>
        public string SapProcessName { get; set; }


        /// <summary>
        /// FS准备工艺代码
        /// </summary>
        public string FsAuxiProcessCode { get; set; }

        /// <summary>
        /// FS操作工艺代码
        /// </summary>
        public string FsWorkProcessCode { get; set; }

        /// <summary>
        /// FS工艺名称
        /// </summary>
        public string FsProcessName { get; set; }
    }
}
