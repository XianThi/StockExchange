using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.StockModel
{
    [DataContract]
    public class SellStockResponse
    {
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public decimal NewBalance { get; set; }
        [DataMember]
        public decimal TotalAmount { get; set; }
    }
}
