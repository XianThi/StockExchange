using StockExchange.Core.Models.StockModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Models.ApiModel
{
    public class ApiResponse
    {
        public string code { get; set; }
        public List<ApiData> data { get; set; }
    }
}
