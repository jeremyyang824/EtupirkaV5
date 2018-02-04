using System.Linq;
using Abp.Organizations;

namespace Etupirka.EntityFramework.Portal.Migrations.SeedData
{
    public class CtmcOrganizationUnitsCreator
    {
        private readonly EtupirkaPortalDbContext _context;
        private readonly int _tenantId;

        public CtmcOrganizationUnitsCreator(EtupirkaPortalDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            var orgsLv1 = new OrganizationUnit[]
            {
                new OrganizationUnit(_tenantId, "上海烟草机械有限责任公司", null) { Code = "00" },
                new OrganizationUnit(_tenantId, "新场铸造公司", null) { Code = "21" },
                new OrganizationUnit(_tenantId, "鑫隆烟机厂", null) { Code = "22" },
                new OrganizationUnit(_tenantId, "兴松配件厂", null) { Code = "23" },
                new OrganizationUnit(_tenantId, "借中烟机械技术中心", null) { Code = "28" }
            };
            foreach (var org in orgsLv1)
            {
                _context.OrganizationUnits.Add(org);
            }
            _context.SaveChanges();


            var ctmcOrgId = _context.OrganizationUnits.First(o => o.DisplayName == "上海烟草机械有限责任公司").Id;
            var orgsLv2 = new OrganizationUnit[]
            {
                new OrganizationUnit(_tenantId, "总经理室", null) { Code = "01", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "党委书记室", null) { Code = "02", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "公司办公室", null) { Code = "03", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "政治工作部", null) { Code = "04", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "工会", null) { Code = "05", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "综合管理部", null) { Code = "06", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "信息技术部", null) { Code = "07", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "技术开发部", null) { Code = "08", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "质量检验部", null) { Code = "09", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "客户中心", null) { Code = "10", ParentId = ctmcOrgId},

                new OrganizationUnit(_tenantId, "生产管理部", null) { Code = "11", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "财务会计部", null) { Code = "13", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "人力资源部", null) { Code = "14", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "安全保卫部", null) { Code = "15", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "行政事业部", null) { Code = "16", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "服务中心", null) { Code = "17", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "设备动力部", null) { Code = "18", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "制造一部", null) { Code = "19", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "制造二部", null) { Code = "20", ParentId = ctmcOrgId},

                new OrganizationUnit(_tenantId, "烟草门市部", null) { Code = "24", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "其他", null) { Code = "25", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "中臣配件公司", null) { Code = "26", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "退管会办公室", null) { Code = "27", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "技能培训部", null) { Code = "30", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "中臣数控公司", null) { Code = "31", ParentId = ctmcOrgId},

                new OrganizationUnit(_tenantId, "技术改造办公室", null) { Code = "32", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "综合计划部门", null) { Code = "33", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "采购中心", null) { Code = "34", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "数字化制造基地（筹）", null) { Code = "35", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "招投标管理和核价办公室", null) { Code = "38", ParentId = ctmcOrgId},
                new OrganizationUnit(_tenantId, "战略发展部", null) { Code = "39", ParentId = ctmcOrgId},
            };
            foreach (var org in orgsLv2)
            {
                _context.OrganizationUnits.Add(org);
            }
            _context.SaveChanges();
        }

    }
}
