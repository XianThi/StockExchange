using StockExchange.Core.Interfaces.Repositories;
using StockExchange.Core.Interfaces.Services;
using StockExchange.Core.Models.StockModel;
using StockExchange.Core.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.BAL.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        public StockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<BuyStockResponse> BuyStock(BuyStockRequest request)
        {
            return await _stockRepository.BuyStock(request);
        }

        public async Task<Stock> GetStockAsync(string stockSymbol)
        {
            return await _stockRepository.GetStockBySymbolAsync(stockSymbol);
        }

        public async Task<decimal> GetStockPriceAsync(string stockSymbol)
        {
            var stock = await _stockRepository.GetStockBySymbolAsync(stockSymbol);
            return stock.Price;
        }

        public async Task<List<Stock>> GetStocksAsync()
        {
            var stocks = await _stockRepository.GetAllStocksAsync();
            return stocks.ToList();
        }

        public async Task<StockResponse> GetStocksPagedAsync(int pageNumber = 1, int pageSize = 100)
        {
            return await _stockRepository.GetStocksPagedAsync(pageNumber, pageSize);
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            return await _stockRepository.CreateUser(request);
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            return await _stockRepository.Login(request);
        }
    }
}
