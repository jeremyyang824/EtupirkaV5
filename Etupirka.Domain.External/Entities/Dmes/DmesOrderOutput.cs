using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.External.Entities.Dmes
{
    /// <summary>
    /// 电气MES订单输出
    /// </summary>
    public class DmesOrderOutput
    {
        /// <summary>
        /// 派工单和工单的关联表ID
        /// </summary>
        public int DispatchWorKTicketID { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        ///// <summary>
        ///// 生产工厂(WERKS)
        ///// </summary>
        //public string ProductionPlant { get; set; }

        /// <summary>
        /// MRP控制者
        /// (100:制造订单; 200装配订单)
        /// </summary>
        public string MRPController { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialNumber { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RoutingNumber { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ActualReleaseDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OMesStatus { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public decimal TargetQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Quantity { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string OprCntrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OWorkCenter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AFVV_VGW01 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AFVV_VGE01 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AFVV_VGW02 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AFVV_VGE02 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AFVV_VGW03 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AFVV_VGE03 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime SchedStartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime SchedFinishDate { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public DateTime ActualStartDate { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public DateTime ActualFinishDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ActualWorkCenterID { get; set; }

        /// <summary>
        /// 派工单号
        /// </summary>
        public string DispatchTicketID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ActualDispatchDate { get; set; }
                

        /// <summary>
        /// 
        /// </summary>
        public string DispatchMesStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DispatchManufStatus { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ActualWorkID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ActualWorkName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ManufNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DW_ID { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int ReachStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ToolStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime RequireDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DiStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TechStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int NCStatus { get; set; }
    }
}
