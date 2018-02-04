using System;
using Abp.AutoMapper;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Application.Manufacture.HandOver.Dto
{
    /// <summary>
    /// 工序信息
    /// </summary>
    [AutoMap(typeof(OrderProcess))]
    public class OrderProcessDto
    {
        /// <summary>
        /// 工序号
        /// </summary>
        public string ProcessNumber { get; set; }

        /// <summary>
        /// 工艺名
        /// </summary>
        public string ProcessCode { get; set; }

        /// <summary>
        /// 工艺名
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 使用点ID
        /// </summary>
        public string PointOfUseId { get; set; }

        /// <summary>
        /// 使用点名称
        /// </summary>
        public string PointOfUseName { get; set; }
    }
}
