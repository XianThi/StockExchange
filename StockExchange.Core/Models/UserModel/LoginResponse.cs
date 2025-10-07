using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.UserModel
{
    [DataContract]
    public class LoginResponse
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public decimal Balance { get; set; } = 0;

        [DataMember]
        public int Success { get; set; } = 0;
    }
}
