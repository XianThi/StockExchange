using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.UserModel
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Username { get; set; } = string.Empty;

        [DataMember]
        public string Email { get; set; } = string.Empty;
        [DataMember]
        public string PasswordHash { get; set; } = string.Empty;
        [DataMember]
        public decimal Balance { get; set; } // bakiye (ör: 1000 TL)
        [DataMember]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [DataMember]
        public List<UserStock> UserStocks { get; set; } = new();
    }
}
