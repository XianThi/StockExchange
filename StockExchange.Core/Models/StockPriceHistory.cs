using StockExchange.Core.Models.StockModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models
{
    public class StockPriceHistory
    {
        public int Id { get; set; }

        public int StockId { get; set; }
        public Stock Stock { get; set; }

        public DateTime RecordedAt { get; set; }
        public decimal Price { get; set; }
    }
}
