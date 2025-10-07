using StockExchange.Core.Models.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Interfaces.Services
{
    public interface IDatabaseService
    {
        Task<bool> SeedDataAsync();
        Task<bool> InsertDataAsync(List<ApiData> data);
    }
}
