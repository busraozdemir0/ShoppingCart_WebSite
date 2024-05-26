using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataModels.CustomModels
{
    public class OrdersModel
    {
        public int Id { get; set; }
        public string? OrderId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? Quantity { get; set; }
        public int? Price { get; set; }
        public int? SubTotal { get; set; }
        public string? CreatedOn { get; set; }


    }
}
