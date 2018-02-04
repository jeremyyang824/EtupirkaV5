using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.External.Entities.Winchill
{
    public class CreateJobInput
    {
        public int PrepareInfoId
        {
            get; set;
        }

        public string ItemNumber
        {
            get; set;
        }

        public string JobType
        {
            get; set;
        }

        public string MachineType
        {
            get; set;
        }
    }
}
