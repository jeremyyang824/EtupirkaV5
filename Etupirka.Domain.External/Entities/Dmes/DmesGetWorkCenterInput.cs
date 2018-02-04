using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Etupirka.Domain.External.Entities.Dmes
{
    public class DmesGetWorkCenterInput : IPagedResultRequest
    {
        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WorkCenterName { get; set; }

        public int SkipCount { get; set; }

        public int MaxResultCount { get; set; }

    }
}
