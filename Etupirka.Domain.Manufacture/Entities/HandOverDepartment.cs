using System;
using System.Collections.Generic;
using Abp.Domain.Values;
using Abp.Organizations;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// 交接部门
    /// </summary>
    public class HandOverDepartment : ValueObject<HandOverSupplier>
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public long? OrganizationUnitId { get; set; }

        /// <summary>
        /// 部门（HR）编码
        /// </summary>
        public string OrganizationUnitCode { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string OrganizationUnitName { get; set; }

        public void Clear()
        {
            this.OrganizationUnitId = null;
            this.OrganizationUnitCode = null;
            this.OrganizationUnitName = null;
        }

        /// <summary>
        /// 创建OrganizationUnit
        /// </summary>
        public static HandOverDepartment CreateFromOrganizationUnit(OrganizationUnit ou)
        {
            if (ou == null)
                throw new ArgumentNullException("ou");

            var department = new HandOverDepartment
            {
                OrganizationUnitId = ou.Id,
                OrganizationUnitCode = ou.Code,
                OrganizationUnitName = ou.DisplayName
            };
            return department;
        }
    }
}
