using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.OrderModel
{
    [DataContract]
    public class PurchaseDetail
    {
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public decimal PurchasePrice { get; set; }
        public DateTime PurchasedAt { get; set; }
        public decimal TotalCost { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal ProfitLoss { get; set; }
    }
}
