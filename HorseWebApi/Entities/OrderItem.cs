using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseWebApi.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public int Count { get; set; }
        public virtual Order Order { get; set; }
    }
}
