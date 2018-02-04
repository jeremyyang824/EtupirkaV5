using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Etupirka.Application.Manufacture.Arragement.Dto;
using Etupirka.Application.Manufacture.Arragement.Factory;
using Etupirka.Application.Manufacture.Cooperate;
using Etupirka.Application.Portal;
using Etupirka.Domain.External.Entities.Bapi;
using Etupirka.Domain.External.Entities.Winchill;
using Etupirka.Domain.External.Repositories;
using Etupirka.Domain.External.Entities.Wintool;

namespace Etupirka.Application.Manufacture.Arragement
{
    /// <summary>
    /// 生产齐备性管理
    /// </summary>
    //[AbpAuthorize]
    public class ArragementAppService : EtupirkaAppServiceBase, IArragementAppService
    {
        private readonly IWinchillRepository _winchillRepository;
        private readonly IWintoolApiRepository _wintoolApiRepository;
        private readonly IBAPIRepository _bapiRepository;
        private readonly CooperateConfigurations _cooperateConfigurations;

        public ArragementAppService(
            IWinchillRepository winchillRepository, IWintoolApiRepository wintoolApiRepository, IBAPIRepository bapiRepository,
            CooperateConfigurations cooperateConfigurations)
        {
            this._winchillRepository = winchillRepository;
            this._wintoolApiRepository = wintoolApiRepository;
            this._bapiRepository = bapiRepository;
            this._cooperateConfigurations = cooperateConfigurations;
        }

        /// <summary>
        /// 获取图纸信息
        /// </summary>
        /// <param name="partNumber">零件编码</param>
        /// <param name="partVersion">零件版本</param>
        /// <returns></returns>
        public async Task<PartDrawingDto> GetPartDrawing(string partNumber, string partVersion)
        {
            if (string.IsNullOrWhiteSpace(partNumber))
                throw new ArgumentNullException(nameof(partNumber));
            if (string.IsNullOrWhiteSpace(partVersion))
                throw new ArgumentNullException(nameof(partVersion));

            var sourceList = await this._winchillRepository.GetByPartItem(new GetByPartItemInput
            {
                PartNumber = partNumber,
                PartVersion = partVersion,
            });

            if (sourceList == null || !sourceList.Any())
                return null;

            PartDrawingDto partDrawing = PartDrawingFactory.Create(sourceList).FirstOrDefault();
            return partDrawing;
        }

        /// <summary>
        /// 获取零件最新版本图纸信息
        /// </summary>
        /// <param name="partNumber">零件编码</param>
        /// <returns></returns>
        public async Task<PartDrawingDto> GetPartDrawingLastVersion(string partNumber)
        {
            if (string.IsNullOrWhiteSpace(partNumber))
                throw new ArgumentNullException(nameof(partNumber));

            var sourceList = await this._winchillRepository.GetByPartItem(new GetByPartItemInput
            {
                PartNumber = partNumber
            });

            if (sourceList == null || !sourceList.Any())
                return null;

            var lastPartVersions = sourceList
                .GroupBy(pd => pd.PartVersion)
                .OrderByDescending(g => g.Key)
                .FirstOrDefault();

            PartDrawingDto partDrawing = PartDrawingFactory.Create(lastPartVersions).FirstOrDefault();
            return partDrawing;
        }

        /// <summary>
        /// 获取图纸信息(所有零件版本)
        /// </summary>
        /// <param name="partNumber">零件编码</param>
        public async Task<ListResultDto<PartDrawingDto>> GetPartDrawingAll(string partNumber)
        {
            if (string.IsNullOrWhiteSpace(partNumber))
                throw new ArgumentNullException(nameof(partNumber));

            var sourceList = await this._winchillRepository.GetByPartItem(new GetByPartItemInput
            {
                PartNumber = partNumber
            });

            if (sourceList == null || !sourceList.Any())
                return null;

            List<PartDrawingDto> partDrawings = PartDrawingFactory.Create(sourceList);
            return new ListResultDto<PartDrawingDto>(partDrawings);
        }


        /// <summary>
        /// 取得所有零件图纸
        /// </summary>
        public async Task<ListResultDto<PartDrawingDto>> GetAllDrawings()
        {
            var results = new List<PartItemDoc>();
            var partVersions = await this._winchillRepository.GetAllPartVersions();
            foreach (var pv in partVersions)
            {
                var sourceList = await this._winchillRepository.GetByPartItem(new GetByPartItemInput
                {
                    PartNumber = pv.PartNumber,
                    PartVersion = pv.PartVersion,
                });
                if (sourceList == null || !sourceList.Any())
                    continue;

                results.AddRange(sourceList);
            }

            List<PartDrawingDto> partDrawings = PartDrawingFactory.Create(results);
            return new ListResultDto<PartDrawingDto>(partDrawings);
        }

        public async Task<ListResultDto<ArchiveBeanOutput>> GetCncArchives(string partNumber, string archiveType)
        {
            var fileResult = await _wintoolApiRepository.WintoolGetArchive(new GetArchiveInput()
            {
                ItemNumber = partNumber,
                ArchiveType = Convert.ToInt32(archiveType)
            });

            //if (fileResult.IsSuccess)
            //{
            return new ListResultDto<ArchiveBeanOutput>(fileResult.Files);
            //}

        }

        /// <summary>
        /// 取得SAP订单及工序信息
        /// </summary>
        /// <param name="orderNumber">订单号</param>
        /// <returns></returns>
        public async Task<BapiOrderOutput> GetSapOrder(string orderNumber)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
                throw new ArgumentNullException("orderNumber");

            var sapOrders = this._bapiRepository.GetSapOrders(new GetSapOrdersInput
            {
                Plant = _cooperateConfigurations.Cooperate_SapWestWERKS,
                OrderNumberRangeBegin = orderNumber.Trim(),
                OrderNumberRangeEnd = orderNumber.Trim()
            });
            return sapOrders.OrderList.FirstOrDefault();
        }
    }
}
