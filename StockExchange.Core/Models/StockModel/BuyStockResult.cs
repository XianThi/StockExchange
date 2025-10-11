using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.StockModel
{
    public class BuyStockResult
    {
        public string Result { get; set; } // "Success" || "Error"
        public string ErrorMessage { get; set; } 
        public decimal TotalAmount { get; set; }
        public decimal NewBalance { get; set; }
        public decimal ExecutedPrice { get; set; }
    }
}
