using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorseWebApi.Requests
{
    public class RegionRequest
    {
        public string Name { get; set; }
        public int ParentId { get; set; }
    }
}
