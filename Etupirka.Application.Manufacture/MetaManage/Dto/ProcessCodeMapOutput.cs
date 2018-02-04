using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Application.Manufacture.MetaManage.Dto
{
    /// <summary>
    /// FS/SAP工艺映射
    /// </summary>
    [AutoMapFrom(typeof(ProcessCodeMap))]
    public class ProcessCodeMapOutput : AuditedEntityDto
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
