using StockExchange.Core.Interfaces.Repositories;
using StockExchange.Core.Interfaces.Services;
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
            return await _stockRepository.BuyStockAsync(request.UserId,request.StockId,request.Quantity);
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

        public async Task<IEnumerable<StockPriceHistory>> GetPriceHistoryWithSymbol(string symbol)
        {
            return await _stockRepository.GetPriceHistoryWithSymbol(symbol);
        }

        public async Task<PortfolioResponse> GetPortfolioAsync(int userId)
        {
            try
            {
                var portfolio = await _stockRepository.GetUserPortfolioAsync(userId);
                var summary = await _stockRepository.GetPortfolioSummaryAsync(userId);

                return new PortfolioResponse
                {
                    Success = true,
                    Message = "Portföy başarıyla getirildi",
                    Summary = summary,
                    Items = portfolio
                };
            }
            catch (Exception ex)
            {
                return new PortfolioResponse
                {
                    Success = false,
                    Message = $"Portföy getirilirken hata: {ex.Message}"
                };
            }
        }

        public async Task<TransactionHistoryResponse> GetTransactionHistoryAsync(int userId)
        {
            try
            {
                var transactions = await _stockRepository.GetUserTransactionsAsync(userId);

                return new TransactionHistoryResponse
                {
                    Success = true,
                    Message = "İşlem geçmişi başarıyla getirildi",
                    Transactions = transactions
                };
            }
            catch (Exception ex)
            {
                return new TransactionHistoryResponse
                {
                    Success = false,
                    Message = $"İşlem geçmişi getirilirken hata: {ex.Message}"
                };
            }
        }

        public async Task<SellStockResponse> SellStockAsync(SellRequest request)
        {
            try
            {
                var stock = await _stockRepository.GetStockBySymbolAsync(request.Symbol);
                if (stock == null)
                {
                    return new SellStockResponse
                    {
                        Success = false,
                        Message = "Hisse bulunamadı"
                    };
                }
                var result = await _stockRepository.SellStockAsync(request.UserId, stock.Id, request.Quantity, stock.Price);

                if (result.Success)
                {
                    return new SellStockResponse
                    {
                        Success = true,
                        Message = "Satış işlemi başarılı",
                        NewBalance = result.NewBalance
                    };
                }
                else
                {
                    return new SellStockResponse
                    {
                        Success = false,
                        Message = result.Message
                    };
                }
            }
            catch (Exception ex)
            {
                return new SellStockResponse
                {
                    Success = false,
                    Message = $"Satış işleminde hata: {ex.Message}"
                };
            }
        }

        public async Task<OrderResult> AddBuyOrderAsync(BuyOrderRequest request)
        {
            return await _stockRepository.AddBuyOrderAsync(request.UserId, request.StockId, request.Quantity, request.MaxPrice);

        }

        public async Task<OrderResult> AddSellOrderAsync(SellOrderRequest request)
        {
            if((request.StockId == null  || request.StockId == 0) && string.IsNullOrEmpty(request.Symbol))
            {
                return new OrderResult
                {
                    Success = false,
                    Message = "Hisse senedi kimliği veya sembolü sağlanmalıdır."
                };
            }
            if((request.StockId == null || request.StockId == 0) && !string.IsNullOrEmpty(request.Symbol))
            {
                var stock = await _stockRepository.GetStockBySymbolAsync(request.Symbol);
                if(stock == null)
                {
                    return new OrderResult
                    {
                        Success = false,
                        Message = "Hisse senedi bulunamadı."
                    };
                }
                request.StockId = stock.Id;
            }
            return await _stockRepository.AddSellOrderAsync(request.UserId, request.StockId, request.Quantity, request.MinPrice);
        }

        public async Task<List<PurchaseDetail>> GetUserStockPurchaseHistoryAsync(int userId, string stockSymbol)
        {
            return await _stockRepository.GetUserStockPurchaseHistoryAsync(userId,stockSymbol);
        }
    }
}
