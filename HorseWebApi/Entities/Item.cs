using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseWebApi.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public decimal Weight { get; set; }
        public List<OrderItem> Orders { get; set; }
    }
}
