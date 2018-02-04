using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.External.Wintool
{
    /// <summary>
    /// FSTI接口执行情况
    /// </summary>
    public class WinToolResult<TData> : WinToolResult
    {
        /// <summary>
        /// 额外的反馈数据
        /// </summary>
        public TData ExtensionData { get; set; }

        protected WinToolResult(bool isSuccess, string message)
            : base(isSuccess, message)
        {
        }

        public static WinToolResult<TData> Build(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return Success;
            else
                return new WinToolResult<TData>(false, "HttpStatusCode:" + ((int)response.StatusCode).ToString());
        }

        public new static readonly WinToolResult<TData> Success = new WinToolResult<TData>(true, string.Empty);
        public new static readonly WinToolResult<TData> Failure = new WinToolResult<TData>(false, string.Empty);
    }


    public class WinToolResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 反馈消息
        /// </summary>
        public string Message { get; private set; }

        public WinToolResult(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        public static WinToolResult Build(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return Success;
            else
                return new WinToolResult(false, "HttpStatusCode:" + ((int)response.StatusCode).ToString());
        }

        public static readonly WinToolResult Success = new WinToolResult(true, string.Empty);
        public static readonly WinToolResult Failure = new WinToolResult(false, string.Empty);
    }
}
