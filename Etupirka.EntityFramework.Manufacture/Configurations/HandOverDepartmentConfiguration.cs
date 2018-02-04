using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.EntityFramework.Manufacture.Configurations
{
    /// <summary>
    /// 交接部门配置
    /// </summary>
    public class HandOverDepartmentConfiguration : ComplexTypeConfiguration<HandOverDepartment>
    {
        public HandOverDepartmentConfiguration()
        {
            Property(t => t.OrganizationUnitId).IsOptional();
            Property(t => t.OrganizationUnitCode).IsOptional();
            Property(t => t.OrganizationUnitName).IsOptional().HasMaxLength(50);
        }
    }
}
