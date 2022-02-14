using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorseWebApi.Requests
{
    public class OrderRequest
    {
        public int UserId { get; set; }
        public string Note { get; set; }        
        public int RegionId { get; set; }
        public List<OrderItemRequest> ItemIds { get; set; }
    }
}
