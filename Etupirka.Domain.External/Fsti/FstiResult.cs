using System;

namespace Etupirka.Domain.External.Fsti
{
    /// <summary>
    /// FSTI接口执行情况
    /// </summary>
    public class FstiResult<TData> : FstiResult
    {
        /// <summary>
        /// 额外的反馈数据
        /// </summary>
        public TData ExtensionData { get; set; }

        protected FstiResult(bool isSuccess, string message)
            : base(isSuccess, message)
        { }

        public static FstiResult<TData> Build(string transactionResult)
        {
            if (string.IsNullOrWhiteSpace(transactionResult))
                return Failure;

            bool isSuccess = FstiResultParser.IsTransactionSuccess(transactionResult);
            if (isSuccess)
                return Success;
            else
                return new FstiResult<TData>(false, transactionResult);
        }

        public new static readonly FstiResult<TData> Success = new FstiResult<TData>(true, string.Empty);
        public new static readonly FstiResult<TData> Failure = new FstiResult<TData>(false, string.Empty);
    }

    public class FstiResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 反馈消息
        /// </summary>
        public string Message { get; private set; }

        protected FstiResult(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        public static FstiResult Build(string transactionResult)
        {
            if (string.IsNullOrWhiteSpace(transactionResult))
                return Failure;

            bool isSuccess = FstiResultParser.IsTransactionSuccess(transactionResult);
            if (isSuccess)
                return Success;
            else
                return new FstiResult(false, transactionResult);
        }

        public static readonly FstiResult Success = new FstiResult(true, string.Empty);
        public static readonly FstiResult Failure = new FstiResult(false, string.Empty);
    }
}
