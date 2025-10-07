using StockExchange.Core.Models.StockModel;
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
        Task<BuyStockResponse> BuyStock(BuyStockRequest request);
    }
}
