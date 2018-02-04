using Abp.Domain.Repositories;
using Etupirka.Domain.External.Entities.Winchill;
using Etupirka.Domain.External.Repositories;
using Etupirka.Domain.External.Wintool;
using Etupirka.Domain.Manufacture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Etupirka.Domain.External.Entities.Wintool;
using System.IO;
using Newtonsoft.Json;

namespace Etupirka.Implement.External.Repositories
{
    public class WintoolApiRepository : EtupirkaExternalRepositoryBase, IWintoolApiRepository
    {
        private readonly IRepository<SapMOrderProcessDispatchPrepare, int> _sapMOrderProcessDispatchPrepareRepository;
        private readonly IRepository<SapMOrderProcessDispatchPrepareStep, int> _sapMOrderProcessDispatchPrepareStepRepository;
        private readonly DMESWorkCenterRepository _dmesWorkCenterRepository;

        public WintoolApiRepository(DMESWorkCenterRepository dmesWorkCenterRepository)
        {
            this._dmesWorkCenterRepository = dmesWorkCenterRepository;
        }

        /// <summary>
        /// 1) 刀具配刀流程
        /// http://172.16.31.51/winToolV2/api/Jobs/CreateJob
        /// 参数：	Identity：{类型}-{机台任务ID}-{物料编码}	jobType：TL	partNr：物料编码	machineType：（WinTool）设备编码
        /// </summary>
        public async Task<WinToolResult> WintoolCreateToolingJob(CreateJobInput input)
        {
            WinToolResult wResult = WinToolResult.Failure;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    //httpClient.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["winToolBaseAddress"]);
                    string baseAddress = System.Configuration.ConfigurationManager.AppSettings["winToolBaseAddress"];
                    string paramAddress = string.Format("{0}/api/Jobs/CreateJob?partNr={1}&machineType={2}&Identity={3}&jobType={4}",
                        baseAddress,
                        input.ItemNumber,
                        input.MachineType,
                        string.Format("{0}-{1}-{2}", input.JobType, input.PrepareInfoId, input.ItemNumber),
                        input.JobType);
                    var result = httpClient.PostAsync(paramAddress, null).Result;
                    var status = result.EnsureSuccessStatusCode();

                    wResult = WinToolResult.Build(status);
                    return await Task.FromResult(wResult);
                }
            }
            catch (Exception e)
            {
                wResult = new WinToolResult(false, e.Message);
                return await Task.FromResult(wResult);
            }
            //finally
            //{
            //    await _sapMOrderProcessDispatchPrepareStepRepository.InsertAsync(new SapMOrderProcessDispatchPrepareStep()
            //    {
            //        IsStepSuccess = wResult.IsSuccess,
            //        SapMOrderProcessDispatchPrepareId = input.PrepareInfoId,
            //        StepTransactionType = SapMOrderProcessDispatchPrepareStepTransTypes.TL.ToString(),
            //        StepName = SapMOrderProcessDispatchPrepareStepTransTypes.TL_Start.ToString(),
            //        StepResultMessage = wResult.Message
            //        //StepStatus = (short)SapMOrderProcessDispatchPrepareStepStatus.准备中,                                     
            //    });
            //}
        }

        /// <summary>
        /// NC程序准备流程
        /// http://172.16.31.51/winToolV2/api/Jobs/CreateJob
        /// 参数：	Identity：{类型}-{机台任务ID}-{物料编码}	jobType：NC	partNr：物料编码	machineType：（WinTool）设备编码
        /// </summary>
        public async Task<WinToolResult> WintoolCreateNCJob(CreateJobInput input)
        {
            WinToolResult wResult = WinToolResult.Failure;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    //httpClient.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["winToolBaseAddress"]);
                    string baseAddress = System.Configuration.ConfigurationManager.AppSettings["winToolBaseAddress"];
                    string paramAddress = string.Format("{0}/api/Jobs/CreateJob?partNr={1}&machineType={2}&Identity={3}&jobType={4}",
                        baseAddress,
                        input.ItemNumber,
                        input.MachineType,
                        string.Format("{0}-{1}-{2}",
                        input.JobType,
                        input.PrepareInfoId,
                        input.ItemNumber),
                        input.JobType);
                    var result = httpClient.PostAsync(paramAddress, null).Result;
                    var status = result.EnsureSuccessStatusCode();

                    wResult = WinToolResult.Build(status);
                    return await Task.FromResult(wResult);
                }
            }
            catch (Exception e)
            {
                wResult = new WinToolResult(false, e.Message);
                return await Task.FromResult(wResult);
            }
            //finally
            //{
            //    await _sapMOrderProcessDispatchPrepareStepRepository.InsertAsync(new SapMOrderProcessDispatchPrepareStep()
            //    {
            //        IsStepSuccess = wResult.IsSuccess,
            //        SapMOrderProcessDispatchPrepareId = input.PrepareInfoId,
            //        StepTransactionType = SapMOrderProcessDispatchPrepareStepTransTypes.NC.ToString(),
            //        StepName = SapMOrderProcessDispatchPrepareStepTransTypes.NC_Start.ToString(),
            //        StepResultMessage = wResult.Message
            //        //StepStatus = (short)SapMOrderProcessDispatchPrepareStepStatus.准备中,                                     
            //    });
            //}
        }

        /// <summary>
        /// 机台资料查看
        /// 
        /// CAPP工艺查看（派工下发后就可以查看）
        /// http://172.16.31.51/winToolV2/api/CNCArchive/GetCAPPs
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<WinToolArchiveResult> WintoolGetArchive_CAPP(string itemNumber)
        {
            List<ArchiveBeanOutput> list = new List<ArchiveBeanOutput>();

            //for (int i = 0; i < 5; i++)
            //{
            //    var url = @"\\SERVERSVN\Shared\分布式事务处理方案.pdf";
            //    ArchiveBeanOutput bean = new ArchiveBeanOutput()
            //    {
            //        UrlPath = url,
            //        FileName = url.Split('\\').Last()
            //    };
            //    list.Add(bean);
            //}
            //var arResult = new WinToolArchiveResult(true, list, ArchiveTypeEnum.GetCAPPs.ToString());
            //return await Task.FromResult(arResult);
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string baseAddress = System.Configuration.ConfigurationManager.AppSettings["winToolBaseAddress"];
                    string paramAddress = string.Format("{0}/api/CNCArchive/GetCAPPs?partNr={1}",
                        baseAddress, itemNumber);
                    var result = httpClient.GetAsync(paramAddress).Result;
                    var status = result.EnsureSuccessStatusCode();
                    string urlGet = await result.Content.ReadAsStringAsync();
                    string[] urlArray = JsonConvert.DeserializeObject<string[]>(urlGet);

                    foreach (string url in urlArray)
                    {
                        // var url = @"\\SERVERSVN\Shared\分布式事务处理方案.pdf";
                        ArchiveBeanOutput bean = new ArchiveBeanOutput()
                        {
                            UrlPath = url,
                            FileName = url.Split('\\').Last()
                        };
                        list.Add(bean);
                    }

                    var arResult = new WinToolArchiveResult(true, list, ArchiveTypeEnum.GetCAPPs.ToString());
                    return await Task.FromResult(arResult);
                }
            }
            catch (Exception e)
            {
                var arResult = new WinToolArchiveResult(false, null, ArchiveTypeEnum.GetCAPPs.ToString());
                return await Task.FromResult(arResult);
            }
        }

        /// <summary>
        /// 机台资料查看
        /// 
        /// CAPP工艺查看（派工下发后就可以查看）
        /// http://172.16.31.51/winToolV2/api/CNCArchive/GetToolPresetPdf
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<WinToolArchiveResult> WintoolGetArchive_TL(string itemNumber)
        {
            List<ArchiveBeanOutput> list = new List<ArchiveBeanOutput>();
            //var url = @"\\SERVERSVN\Shared\分布式事务处理方案.pdf";
            //ArchiveBeanOutput beane = new ArchiveBeanOutput()
            //{
            //    UrlPath = url,
            //    FileName = url.Split('\\').Last()
            //};
            //list.Add(beane);

            //var arResult = new WinToolArchiveResult(true, list, ArchiveTypeEnum.GetToolPresetPdf.ToString());
            //return await Task.FromResult(arResult);
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string baseAddress = System.Configuration.ConfigurationManager.AppSettings["winToolBaseAddress"];
                    string paramAddress = string.Format("{0}/api/CNCArchive/GetToolPresetPdf?partNr={1}",
                        baseAddress, itemNumber);
                    var result = httpClient.GetAsync(paramAddress).Result;
                    var status = result.EnsureSuccessStatusCode();
                    string urlGet = await result.Content.ReadAsStringAsync();

                    //urlGet = @"\\SERVERSVN\Shared\分布式事务处理方案.pdf";
                    ArchiveBeanOutput bean = new ArchiveBeanOutput()
                    {
                        UrlPath = urlGet,
                        FileName = urlGet.Split('\\').Last()
                    };
                    list.Add(bean);

                    var arResult = new WinToolArchiveResult(true, list, ArchiveTypeEnum.GetToolPresetPdf.ToString());
                    return await Task.FromResult(arResult);
                }
            }
            catch (Exception e)
            {
                var arResult = new WinToolArchiveResult(false, null, ArchiveTypeEnum.GetToolPresetPdf.ToString());
                return await Task.FromResult(arResult);
            }
        }

        /// <summary>
        /// 机台资料查看
        /// 
        /// 刀具配刀表查看（准备流程完成后可以查看）
        /// http://172.16.31.51/winToolV2//api/CNCArchive/GetToolPresetPdf
        /// CAPP工艺查看（派工下发后就可以查看）
        /// http://172.16.31.51/winToolV2/api/CNCArchive/GetCAPPs
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WinToolArchiveResult> WintoolGetArchive(GetArchiveInput input)
        {
            WinToolArchiveResult result = new WinToolArchiveResult();
            if (input.ArchiveType == (int)ArchiveTypeEnum.GetToolPresetPdf)
                result = await WintoolGetArchive_TL(input.ItemNumber);
            if (input.ArchiveType == (int)ArchiveTypeEnum.GetCAPPs)
                result = await WintoolGetArchive_CAPP(input.ItemNumber);

            return await Task.FromResult(result);
        }
    }
}
