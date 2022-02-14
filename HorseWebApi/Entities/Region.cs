using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseWebApi.Entities
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Region> Regions { get; set; }
        public List<Order> Orders { get; set; }
        public virtual Region Parent { get; set; }
    }
}
