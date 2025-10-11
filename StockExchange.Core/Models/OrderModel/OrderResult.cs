﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.OrderModel
{
    [DataContract]
    public class OrderResult
    {
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string Message { get; set; }

    }
}
