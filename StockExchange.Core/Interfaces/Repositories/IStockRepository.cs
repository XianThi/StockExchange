using StockExchange.Core.Models.StockModel;
using StockExchange.Core.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Core.Interfaces.Repositories
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stock>> GetAllStocksAsync();
        Task<StockResponse> GetStocksPagedAsync(int pageNumber = 1, int pageSize = 100);
        Task<Stock> GetStockByIdAsync(int id);
        Task<Stock> GetStockBySymbolAsync(string symbol);
        Task AddStockAsync(Stock stock);
        Task UpdateStockAsync(Stock stock);
        Task DeleteStockAsync(int id);

        Task<CreateUserResponse> CreateUser(CreateUserRequest request);
        Task<LoginResponse> Login(LoginRequest request);
        Task<BuyStockResponse> BuyStock(BuyStockRequest request);
    }
}
