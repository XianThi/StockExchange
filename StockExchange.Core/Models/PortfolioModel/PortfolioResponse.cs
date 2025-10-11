using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.PortfolioModel
{
    [DataContract]
    public class PortfolioResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public PortfolioSummary Summary { get; set; }
        public List<PortfolioItem> Items { get; set; }
    }
}
