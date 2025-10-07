using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.StockModel
{
    [DataContract]
    public class Stock
    {
        [DataMember]
        public string Symbol { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public DateTime LastUpdated { get; set; }
        [DataMember]
        public List<StockPriceHistory> PriceHistory { get; set; } = new List<StockPriceHistory>();
    }
}
