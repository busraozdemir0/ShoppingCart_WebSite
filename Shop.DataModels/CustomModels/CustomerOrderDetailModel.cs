using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataModels.CustomModels
{
    public class CustomerOrderDetailModel
    {
        public string? OrderId { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerNameSurname { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerMobileNo { get; set; }
        public int? SubTotal { get; set; }
        public int? Total { get; set; }
        public string? PaymentMode { get; set; }
        public string? ShippingAddress { get; set; }
        public int? ShippingCharges { get; set; }
        public string? ShippingStatus { get; set; }
    }
}
