using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.UserModel
{
    [DataContract]
    public class CreateUserResponse
    {
        [DataMember]
        public int Success { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public User CreatedUser { get; set; }
    }
}
