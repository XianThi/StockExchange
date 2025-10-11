using StockExchange.Core.Models;
using StockExchange.Core.Models.OrderModel;
using StockExchange.Core.Models.PortfolioModel;
using StockExchange.Core.Models.StockModel;
using StockExchange.Core.Models.TransactionModel;
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

        Task<IEnumerable<StockPriceHistory>> GetPriceHistoryWithSymbol(string symbol);
        Task<bool> UpdatePricesAsync();
        Task<List<PortfolioItem>> GetUserPortfolioAsync(int userId);
        Task<PortfolioSummary> GetPortfolioSummaryAsync(int userId);
        Task<List<TransactionHistory>> GetUserTransactionsAsync(int userId);
        Task<SellStockResponse> SellStockAsync(int userId, int stockId, int quantity, decimal currentPrice);
        Task<BuyStockResponse> BuyStockAsync(int userId, int stockId, int quantity);
        Task<OrderResult> AddBuyOrderAsync(int userId, int stockId, int quantity, decimal price);
        Task<OrderResult> AddSellOrderAsync(int userId, int stockId, int quantity, decimal price);
        Task<List<PurchaseDetail>> GetUserStockPurchaseHistoryAsync(int userId, string stockSymbol);
    }
}
