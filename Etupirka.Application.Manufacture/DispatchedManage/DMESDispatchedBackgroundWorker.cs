using Abp.Threading.BackgroundWorkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Threading.Timers;
using Etupirka.Domain.Manufacture.Entities;
using Abp.Domain.Repositories;
using Etupirka.Domain.External.Repositories;
using Etupirka.Domain.External.Entities.Dmes;

namespace Etupirka.Application.Manufacture.DispatchedManage
{
    /// <summary>
    /// 定时任务
    /// 拉取MES新下发的工票，更新本地齐备性流程状态
    /// </summary>
    public class DMESDispatchedBackgroundWorker : PeriodicBackgroundWorkerBase
    {
        public readonly IDispatchedPrepareAppService _dispatchedPreapareAppService;

        public DMESDispatchedBackgroundWorker(AbpTimer timer, IDispatchedPrepareAppService dispatchedPreapareAppService) : base(timer)
        {
            this._dispatchedPreapareAppService = dispatchedPreapareAppService;

            timer.Period = 5000; //5 seconds (good for tests, but normally will be more)            
        }

        protected override void DoWork()
        {
            //1、获取DMESDispatchedWorker最新数据
            //2、根据最新的同步完成时间，去获取（最新的同步完成时间-当前）的所有realeased派工单
            //3、根据获取的派工单，进行本地齐备性流程状态创建
            //4、如果派工单已经有状态信息，不重复创建

            this._dispatchedPreapareAppService.DoWorkForDispatched(new Dto.DispatchedWorkerInput() {
                    WorkerType = DMESDispatchedWorkerType.TimeTask
            });
        }
    }
}
