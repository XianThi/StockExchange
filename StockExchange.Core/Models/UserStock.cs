using StockExchange.Core.Models.StockModel;
using StockExchange.Core.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models
{
    [DataContract]
    public class UserStock
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        public User User { get; set; }
        [DataMember]
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
        [DataMember]
        public decimal PurchasePrice { get; set; }
    }
}
