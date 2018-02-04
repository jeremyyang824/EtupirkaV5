using System;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Application.Manufacture.HandOver.Dto
{
    /// <summary>
    /// 交接部门信息
    /// </summary>
    [AutoMap(typeof(HandOverDepartment))]
    public class HandOverDepartmentDto
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public long? OrganizationUnitId { get; set; }

        /// <summary>
        /// 部门（HR）编码
        /// </summary>
        public long? OrganizationUnitCode { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string OrganizationUnitName { get; set; }
    }
}
