using System;

namespace Etupirka.Domain.External.Fsti
{
    /// <summary>
    /// FSTI接口返回消息解析
    /// </summary>
    public static class FstiResultParser
    {
        /// <summary>
        /// 判断登录Token是否有效
        /// </summary>
        public static bool IsLoginTokenValidate(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return false;
            return !message.Contains("参数错误");
        }

        /// <summary>
        /// 判断业务是否成功
        /// </summary>
        public static bool IsTransactionSuccess(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return false;
            if (message.Trim().ToUpper() == @"TRANSACTION SUCCEEDED.")
                return true;
            return false;
        }
    }
}
