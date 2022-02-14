using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseWebApi.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public List<OrderItem> Items { get; set; }
        public virtual Region Region { get; set; }
        public User User { get; set; }
    }
}
