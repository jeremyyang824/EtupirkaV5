using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abp.Application.Services.Dto;
using Etupirka.Application.Manufacture.HandOver.Dto;

namespace Etupirka.Web.Models.HandOvers
{
    public class PrintHandOverViewModel
    {
        public HandOverBillOutput HandOverBill { get; set; }

        public IReadOnlyList<HandOverBillLineOutput> HandOverBillLines { get; set; }
    }
}