using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.OrderModel
{
    [DataContract]
    public class BuyOrderRequest
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int StockId { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public decimal MaxPrice { get; set; }
    }
}
