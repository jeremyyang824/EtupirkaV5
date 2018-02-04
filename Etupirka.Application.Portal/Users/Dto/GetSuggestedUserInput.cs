using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;

namespace Etupirka.Application.Portal.Users.Dto
{
    /// <summary>
    /// 用户名自动完成
    /// </summary>
    public class GetSuggestedUserInput : IShouldNormalize
    {
        [Required]
        public string UserNamePrefix { get; set; }

        /// <summary>
        /// 返回记录数量
        /// </summary>
        [Range(1, 20)]
        public int ReturnCount { get; set; } = 10;

        public void Normalize()
        {
            this.UserNamePrefix = this.UserNamePrefix.Trim();
        }
    }
}
