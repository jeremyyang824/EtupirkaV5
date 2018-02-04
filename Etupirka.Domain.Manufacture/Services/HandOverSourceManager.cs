using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Organizations;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Domain.Manufacture.Services
{
    /// <summary>
    /// 交接部门/供应商管理
    /// </summary>
    public class HandOverSourceManager : DomainService
    {
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;

        //可交接部门hr代码
        private static readonly string[] _handOverDepartmentCodes = new string[]
        {
            "21",   //新场铸造公司
            "11",   //生产管理部
            "19",   //制造一部
            "20",   //制造二部
            "1901", //热处理车间
            "35",   //数字化制造基地（筹）
        };

        /// <summary>
        /// 西厂区部门代码
        /// </summary>
        public static readonly string StmcWestDpeartmentCode = "35";

        /// <summary>
        /// 东厂区相关部门使用点结合
        /// </summary>
        public static readonly string[] StmcEastFsPointCode = new string[]
        {
            "1", //一金
            "2", //一热
            "6", //制二
            "S", //生产部
            "5", //新场
        };

        public HandOverSourceManager(
            IRepository<OrganizationUnit, long> organizationUnitRepository)
        {
            this._organizationUnitRepository = organizationUnitRepository;
        }


        /// <summary>
        /// 取得所有可交接部门
        /// </summary>
        public async Task<List<HandOverDepartment>> GetAllHandOverDepartments()
        {
            var list = await this._organizationUnitRepository.GetAll()
                .Where(ou => _handOverDepartmentCodes.Contains(ou.Code))
                .ToListAsync();
            return list.Select(HandOverDepartment.CreateFromOrganizationUnit).ToList();
        }
    }
}
