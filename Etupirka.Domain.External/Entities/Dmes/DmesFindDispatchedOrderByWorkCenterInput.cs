using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Etupirka.Domain.External.Entities.Dmes
{
    public class DmesFindDispatchedOrderByWorkCenterInput : IPagedResultRequest
    {
        [Required]
        public int WorkCenterId { get; set; }

        public int SkipCount { get; set; }

        public int MaxResultCount { get; set; }
    }
}
