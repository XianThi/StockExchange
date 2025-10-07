using StockExchange.Core.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.StockModel
{
    [DataContract]
    public class BuyStockResponse
    {
        [DataMember]
        public int Success { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public User UpdatedUser { get; set; }
    }
}
