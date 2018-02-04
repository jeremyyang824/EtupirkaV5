using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.Manufacture.Entities
{
    public class SapMOrderProcessDispatchPrepareStepTransTypes
    {
        /// <summary>
        ///  全部
        /// </summary>
        public const string ALL = "ALL";

        /// <summary>
        ///  NC程序 准备工作
        /// </summary>
        public const string NC_CN = "NC程序准备";


        /// <summary>
        ///  NC程序 准备工作
        /// </summary>
        public const string NC = "NC";

        /// <summary>
        ///  NC程序 准备工作
        /// </summary>
        public const string NC_Start = "NC Started";

        /// <summary>
        ///  NC程序 准备工作
        /// </summary>
        public const string NC_Finished = "NC Finished";

        /// <summary>
        ///  刀具配刀 准备工作
        /// </summary>
        public const string TL_CN = "刀具配刀准备";


        /// <summary>
        ///  刀具配刀 准备工作
        /// </summary>
        public const string TL = "TL";

        /// <summary>
        ///  NC程序 准备工作
        /// </summary>
        public const string TL_Start = "TL Started";

        /// <summary>
        ///  NC程序 准备工作
        /// </summary>
        public const string TL_Finished = "TL Finished";


        /// <summary>
        ///  模夹具 准备工作
        /// </summary>
        public const string Mould_CN = "模夹具准备";

        /// <summary>
        ///  特殊工位器 准备工作
        /// </summary>
        public const string Special_CN = "特殊工位器准备";

    }

    public enum SapMOrderProcessDispatchPrepareStepStatus : short
    {
        未准备 = 0,
        准备中 = 1,
        已完成 = 2
    }
}
