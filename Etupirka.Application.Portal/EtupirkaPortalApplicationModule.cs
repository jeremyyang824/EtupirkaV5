using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Etupirka.Application.Portal.Users.Dto;
using Etupirka.Domain.Portal;
using System.Linq;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Application.Portal
{
    [DependsOn(typeof(EtupirkaPortalDomainModule), typeof(AbpAutoMapperModule))]
    public class EtupirkaPortalApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                mapper.CreateMap<SysUser, UserOutput>()
                    .ForMember(dto => dto.Roles, options => options.MapFrom(s => s.Roles.Select(r => r.RoleId).ToArray()));

            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
