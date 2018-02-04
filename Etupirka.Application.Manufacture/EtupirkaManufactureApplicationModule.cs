using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Etupirka.Application.Manufacture.Configuration;
using Etupirka.Application.Manufacture.HandOver.Dto;
using Etupirka.Application.Manufacture.SapMOrderManage.Dto;
using Etupirka.Application.Portal;
using Etupirka.Domain.External;
using Etupirka.Domain.External.Entities.Bapi;
using Etupirka.Domain.Manufacture;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.Application.Manufacture.DispatchedManage.Dto;
using Etupirka.Domain.External.Entities.Dmes;
using Abp.Threading.BackgroundWorkers;
using Etupirka.Application.Manufacture.DispatchedManage;

namespace Etupirka.Application.Manufacture
{
    [DependsOn(typeof(AbpAutoMapperModule), typeof(EtupirkaPortalApplicationModule),
        typeof(EtupirkaManufactureDomainModule), typeof(EtupirkaExternalDomainModule))]
    public class EtupirkaManufactureApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding setting providers
            Configuration.Settings.Providers.Add<ManufactureSettingProvider>();

            //Adding AutoMappers
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                mapper.CreateMap<HandOverBill, HandOverBillOutput>()
                    .Include<HandOverBill, HandOverBillWithLineStatisticsOutput>();

                mapper.CreateMap<SapMOrderProcess, SapMOrderProcessOutput>()
                    .Include<SapMOrderProcess, SapMOrderProcessWithCooperateOutput>();

                mapper.CreateMap<BapiOrderProcessOutput, SapMOrderProcess>();
                mapper.CreateMap<BapiOrderOutput, SapMOrder>();

                ////mapper.CreateMap<SapWorkCenter, WorkCenterOutput>();
                //mapper.CreateMap<DmesWorkCenterOutput, WorkCenterOutput>();
                //mapper.CreateMap<FindWorkCentersInput, DmesGetWorkCenterInput>();
                //mapper.CreateMap<DmesOrderOutput, DispatchedOrderOutput>();
            });

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            //Configuration.IocManager.Resolve<IBackgroundWorkerManager>()
            //   .Add(Configuration.IocManager.Resolve<DMESDispatchedBackgroundWorker>());

            //var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            //workManager.Add(IocManager.Resolve<IDMESDispatchedBackgroundWorker>());

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
                workManager.Start();
                //workManager.Add(IocManager.Resolve<DMESDispatchedBackgroundWorker>());
            }
        }
    }
}
