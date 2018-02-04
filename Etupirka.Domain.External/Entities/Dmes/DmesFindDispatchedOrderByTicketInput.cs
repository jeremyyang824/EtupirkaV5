using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Etupirka.Domain.External.Entities.Dmes
{
    public class DmesFindDispatchedOrderByTicketInput// : IPagedResultRequest
    {
        [Required]
        public IList<string> DispatchWorkIDs { get; set; }

        //public int SkipCount { get; set; }

        //public int MaxResultCount { get; set; }
    }
}