using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Etupirka.Application.Portal;

namespace Etupirka.Application.Manufacture.Cooperate.Dto
{
    /// <summary>
    /// 取得SAP订单道序及外协信息（分页）
    /// </summary>
    public class GetSapOrderProcessWithCooperaterPagerInput : GetSapOrderProcessWithCooperaterInput, IPagedResultRequest
    {
        /// <summary>
        /// 最大返回记录数
        /// </summary>
        [Range(1, EtupirkaAppConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        /// <summary>
        /// 跳过记录数
        /// </summary>
        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        public GetSapOrderProcessWithCooperaterPagerInput()
        {
            MaxResultCount = EtupirkaAppConsts.DefaultPageSize;
        }
    }
}
