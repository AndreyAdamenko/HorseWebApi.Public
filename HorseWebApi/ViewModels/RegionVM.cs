using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorseWebApi.ViewModels
{
    public class RegionVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RegionVM> Regions { get; set; }
    }
}
