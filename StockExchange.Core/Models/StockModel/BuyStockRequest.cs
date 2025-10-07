using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.StockModel
{
    [DataContract]
    public class BuyStockRequest
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Symbol { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}
