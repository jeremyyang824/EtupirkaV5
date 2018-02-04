using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Manufacture.Cooperate.Dto;
using Etupirka.Application.Portal.Dto;

namespace Etupirka.Application.Manufacture.Cooperate
{
    /// <summary>
    /// 工艺管理
    /// </summary>
    public interface IProcessManageAppService : IApplicationService
    {
        /// <summary>
        /// 获取SAP订单工艺及外协信息
        /// 用于维护外协使用点、价格
        /// </summary>
        Task<FileDto> GetSapOrderProcessWithCooperaterExcel(GetSapOrderProcessWithCooperaterInput input);

        /// <summary>
        /// 将工艺Excel文件中的外协类型、供应商代码、供应商名称、外协价格写入本地SAP工艺记录
        /// </summary>
        Task ImportSapOrderProcessWithCooperater(FileDto importFile);

        /// <summary>
        /// 取得一条外协工艺
        /// </summary>
        /// <param name="cooperateId">外协ID</param>
        Task<GetSapOrderProcessWithCooperaterOutput> GetSapOrderProcessCooperater(int cooperateId);

        /// <summary>
        /// 更新一个工艺外协信息
        /// </summary>
        Task UpdateSapOrderProcessCooperate(UpdateSapOrderProcessCooperateInput input);

        /// <summary>
        /// 获取SAP订单工艺及外协信息
        /// 用于维护外协使用点、价格
        /// </summary>
        Task<PagedResultDto<GetSapOrderProcessWithCooperaterOutput>>
            GetSapOrderProcessWithCooperaterPager(GetSapOrderProcessWithCooperaterPagerInput input);
    }
}