using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Manufacture.Arragement.Dto;
using Etupirka.Domain.External.Entities.Bapi;
using Etupirka.Domain.External.Entities.Wintool;

namespace Etupirka.Application.Manufacture.Arragement
{
    public interface IArragementAppService : IApplicationService
    {
        /// <summary>
        /// 获取图纸信息
        /// </summary>
        /// <param name="partNumber">零件编码</param>
        /// <param name="partVersion">零件版本</param>
        /// <returns></returns>
        Task<PartDrawingDto> GetPartDrawing(string partNumber, string partVersion);

        /// <summary>
        /// 获取零件最新版本图纸信息
        /// </summary>
        /// <param name="partNumber">零件编码</param>
        /// <returns></returns>
        Task<PartDrawingDto> GetPartDrawingLastVersion(string partNumber);

        /// <summary>
        /// 获取图纸信息(所有零件版本)
        /// </summary>
        /// <param name="partNumber">零件编码</param>
        Task<ListResultDto<PartDrawingDto>> GetPartDrawingAll(string partNumber);

        /// <summary>
        /// 取得所有零件图纸
        /// </summary>
        Task<ListResultDto<PartDrawingDto>> GetAllDrawings();

        Task<ListResultDto<ArchiveBeanOutput>> GetCncArchives(string partNumber, string archiveType);

        /// <summary>
        /// 取得SAP订单及工序信息
        /// </summary>
        /// <param name="orderNumber">订单号</param>
        Task<BapiOrderOutput> GetSapOrder(string orderNumber);
    }
}
