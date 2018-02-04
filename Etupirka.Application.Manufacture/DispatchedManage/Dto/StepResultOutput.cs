using Etupirka.Domain.External.Wintool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Application.Manufacture.DispatchedManage.Dto
{
    public class StepResultOutput
    {
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 执行反馈消息
        /// </summary>
        public string ResultMessage { get; set; }

        public StepResultOutput(bool isSuccess, string message = "")
        {
            this.IsSuccess = isSuccess;
            this.ResultMessage = message;
        }

        public StepResultOutput(WinToolResult wintoolResult)
        {
            this.IsSuccess = wintoolResult.IsSuccess;
            this.ResultMessage = wintoolResult.Message;
        }


        public static readonly StepResultOutput Failure = new StepResultOutput(false);

        public static readonly StepResultOutput Success = new StepResultOutput(true);
    }
}
