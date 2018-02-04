using System;
using Abp.Domain.Values;

namespace Etupirka.Domain.Manufacture.Entities
{
    /// <summary>
    /// 工序信息
    /// </summary>
    public class OrderProcess : ValueObject<OrderProcess>
    {
        /// <summary>
        /// 工序号
        /// </summary>
        public string ProcessNumber { get; set; }

        /// <summary>
        /// 工艺代码
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

        /// <summary>
        /// 是否为空信息
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(this.ProcessNumber);
        }

        /// <summary>
        /// 获取空信息
        /// </summary>
        public static OrderProcess Empty()
        {
            return new OrderProcess
            {
                ProcessNumber = string.Empty,
                ProcessCode = string.Empty,
                ProcessName = string.Empty
            };
        }
        
        /// <summary>
        /// 根据SAP工序创建工序信息
        /// </summary>
        /// <param name="sapProcess">SAP工序</param>
        /// <param name="sapProcessCooperate">SAP工艺外协信息（非外协工序传入null）</param>
        /// <returns>工序信息</returns>
        public static OrderProcess CreateFromSap(SapMOrderProcess sapProcess, SapMOrderProcessCooperate sapProcessCooperate)
        {
            if (sapProcess == null)
                throw new ArgumentNullException("sapProcess");

            OrderProcess process = new OrderProcess
            {
                ProcessNumber = sapProcess.OperationNumber,
                ProcessCode = sapProcess.WorkCenterCode,
                ProcessName = sapProcess.WorkCenterName,
                PointOfUseId = sapProcessCooperate?.CooperaterFsPointOfUse,
                PointOfUseName = sapProcessCooperate?.CooperaterName
            };
            return process;
        }

    }
}
