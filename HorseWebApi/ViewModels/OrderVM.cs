using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorseWebApi.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public List<ItemVM> Items { get; set; }
        public int TotalPrice { get; set; }
    }
}
