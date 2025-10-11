using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.PortfolioModel
{
    [DataContract]
    public class PortfolioSummary
    {
        public decimal TotalPortfolioValue { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal TotalProfitLoss { get; set; }
        public decimal TotalProfitLossPercent { get; set; }
        public decimal DailyProfitLoss { get; set; }
        public List<PortfolioItem> Items { get; set; } = new List<PortfolioItem>();
    }
}
