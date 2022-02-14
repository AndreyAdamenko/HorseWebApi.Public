using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorseWebApi.Requests
{
    public class OrderItemRequest
    {
        public int ItemId { get; set; }
        public int Count { get; set; }
    }
}
