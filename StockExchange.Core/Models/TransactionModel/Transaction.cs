using StockExchange.Core.Models.StockModel;
using StockExchange.Core.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.TransactionModel
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StockId { get; set; }
        public string Type { get; set; } // "BUY" veya "SELL"
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public Stock Stock { get; set; }
        public User User { get; set; }
    }
}
