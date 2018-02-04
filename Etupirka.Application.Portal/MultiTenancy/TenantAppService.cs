using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Abp.UI;
using Etupirka.Application.Portal.MultiTenancy.Dto;
using Etupirka.Domain.Portal.Authorization.Roles;
using Etupirka.Domain.Portal.Editions;
using Etupirka.Domain.Portal.MultiTenancy;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Application.Portal.MultiTenancy
{
    /// <summary>
    /// �⻧����
    /// </summary>
    //[AbpAuthorize(AppPermissions.Pages_Tenants)]
    public class TenantAppService : EtupirkaAppServiceBase, ITenantAppService
    {
        private readonly SysRoleManager _roleManager;
        private readonly EditionManager _editionManager;

        public TenantAppService(
            SysRoleManager roleManager,
            EditionManager editionManager)
        {
            _roleManager = roleManager;
            _editionManager = editionManager;
        }

        /// <summary>
        /// ȡ��ָ���⻧
        /// </summary>
        /// <param name="id">�⻧ID</param>
        public async Task<TenantOutput> GetTenant(int id)
        {
            var t = await TenantManager.GetByIdAsync(id);
            return (t).MapTo<TenantOutput>();
        }

        /// <summary>
        /// ��ȡ��Ч��[�⻧����,�⻧��]
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<List<NameValueDto>> GetActiveTenancyNames()
        {
            var query = TenantManager.Tenants
                .Where(t => !t.IsDeleted && t.IsActive)
                .OrderBy(t => t.TenancyName)
                .Select(t => new NameValueDto { Name = t.Name, Value = t.TenancyName });

            var tenants = await query.ToListAsync();
            return tenants;
        }

        /// <summary>
        /// ��ȡ�⻧��ҳ�б�
        /// </summary>
        public async Task<PagedResultDto<TenantOutput>> GetTenants(GetTenantsInput input)
        {
            var query = TenantManager.Tenants
                .Include(t => t.Edition)
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    t =>
                        t.Name.Contains(input.Filter) ||
                        t.TenancyName.Contains(input.Filter)
                )
                .WhereIf(
                    input.IsActive.HasValue,
                    t => t.IsActive == input.IsActive.Value
                );

            var tenantCount = await query.CountAsync();
            var tenants = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<TenantOutput>(
                tenantCount,
                tenants.MapTo<List<TenantOutput>>()
                );
        }

        /// <summary>
        /// �����⻧
        /// </summary>
        public async Task CreateTenant(CreateTenantInput input)
        {
            //Create tenant
            var tenant = input.MapTo<SysTenant>();
            tenant.ConnectionString = null; //�����⻧�������ݿ�
            //SimpleStringCipher.Instance.Encrypt(input.ConnectionString)

            var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
            if (defaultEdition != null)
            {
                tenant.EditionId = defaultEdition.Id;
            }

            CheckErrors(await TenantManager.CreateAsync(tenant));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new tenant's id.

            //Create tenant database
            //_abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

            //We are working entities of new tenant, so changing tenant filter
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                //Create static roles for new tenant
                CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

                await CurrentUnitOfWork.SaveChangesAsync(); //To get static role ids

                //grant all permissions to admin role
                var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                await _roleManager.GrantAllPermissionsAsync(adminRole);

                //Create admin user for the tenant
                var adminUser = SysUser.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress, SysUser.DefaultPassword);
                CheckErrors(await UserManager.CreateAsync(adminUser));
                await CurrentUnitOfWork.SaveChangesAsync(); //To get admin user's id

                //Assign admin user to role!
                CheckErrors(await UserManager.AddToRoleAsync(adminUser.Id, adminRole.Name));
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        /// <summary>
        /// ɾ���⻧
        /// </summary>
        /// <param name="id">�⻧ID</param>
        public async Task DeleteTenant(int id)
        {
            var tenant = await TenantManager.GetByIdAsync(id);
            CheckErrors(await TenantManager.DeleteAsync(tenant));
        }

        /// <summary>
        /// �����⻧��Ϣ
        /// </summary>
        /// <param name="input">�⻧������Ϣ</param>
        public async Task UpdateTenant(EditTenantInput input)
        {
            var tenant = await TenantManager.GetByIdAsync(input.TenantId);
            input.MapTo(tenant);
            CheckErrors(await TenantManager.UpdateAsync(tenant));
        }

        /// <summary>
        /// �ı��⻧����״̬
        /// </summary>
        /// <param name="id">�⻧ID</param>
        /// <param name="isActive">�Ƿ�����</param>
        public async Task ToggleTenantState(int id, bool isActive)
        {
            var tenant = await TenantManager.GetByIdAsync(id);
            if (tenant == null)
                throw new UserFriendlyException($"�⻧ID��[{id}]�����ڣ�");

            if (tenant.IsActive != isActive)
            {
                tenant.IsActive = isActive;
            }
        }
    }
}