using System;
using System.Collections.Generic;

namespace Etupirka.Domain.External.Bapi
{
    public class BapiResult<TData> : BapiResult
    {
        /// <summary>
        /// 额外的反馈数据
        /// </summary>
        public TData ExtensionData { get; set; }

        public BapiResult(bool isSuccess, string message)
            : base(isSuccess, message)
        { }

        public new static readonly BapiResult<TData> Success = new BapiResult<TData>(true, string.Empty);
        public new static readonly BapiResult<TData> Failure = new BapiResult<TData>(false, string.Empty);
    }

    public class BapiResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 反馈消息
        /// </summary>
        public string Message { get; private set; }

        public BapiResult(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        public static readonly BapiResult Success = new BapiResult(true, string.Empty);
        public static readonly BapiResult Failure = new BapiResult(false, string.Empty);
    }
}
