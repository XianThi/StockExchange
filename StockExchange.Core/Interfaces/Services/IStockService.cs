using StockExchange.Core.Models;
using StockExchange.Core.Models.OrderModel;
using StockExchange.Core.Models.PortfolioModel;
using StockExchange.Core.Models.StockModel;
using StockExchange.Core.Models.TransactionModel;
using StockExchange.Core.Models.UserModel;
using System.ServiceModel;

namespace StockExchange.Core.Interfaces.Services
{
    [ServiceContract]
    public interface IStockService
    {
        [OperationContract]
        Task<decimal> GetStockPriceAsync(string stockSymbol);
        [OperationContract]
        Task<List<Stock>> GetStocksAsync();

        [OperationContract]
        Task<StockResponse> GetStocksPagedAsync(int pageNumber = 1, int pageSize = 100);

        [OperationContract]
        Task<Stock> GetStockAsync(string stockSymbol);

        [OperationContract]
        Task<CreateUserResponse> CreateUser(CreateUserRequest request);
       
        [OperationContract]
        Task<LoginResponse> Login(LoginRequest request);

        [OperationContract]
        Task<IEnumerable<StockPriceHistory>> GetPriceHistoryWithSymbol(string symbol);

        [OperationContract]
        Task<BuyStockResponse> BuyStock(BuyStockRequest request);
        [OperationContract]
        Task<PortfolioResponse> GetPortfolioAsync(int userId);
        [OperationContract]
        Task<TransactionHistoryResponse> GetTransactionHistoryAsync(int userId);
        [OperationContract]
        Task<SellStockResponse> SellStockAsync(SellRequest request);
        [OperationContract]
        Task<OrderResult> AddBuyOrderAsync(BuyOrderRequest request);
        [OperationContract]
        Task<OrderResult> AddSellOrderAsync(SellOrderRequest request);
        [OperationContract]
        Task<List<PurchaseDetail>> GetUserStockPurchaseHistoryAsync(int userId, string stockSymbol);
    }
}
