using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Etupirka.Domain.External.Entities.Winchill
{
    public class GetByPartItemInput
    {
        [Required]
        public string PartNumber { get; set; }

        public string PartVersion { get; set; }

        public string DocVersion { get; set; }
    }
}
