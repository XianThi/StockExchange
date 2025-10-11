using Dapper;
using StockExchange.Core.Interfaces.Repositories;
using StockExchange.Core.Models;
using StockExchange.Core.Models.OrderModel;
using StockExchange.Core.Models.PortfolioModel;
using StockExchange.Core.Models.StockModel;
using StockExchange.Core.Models.TransactionModel;
using StockExchange.Core.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace StockExchange.DAL.Repositories
{
    
    public class StockRepository : IStockRepository
    {
        private readonly IDbConnection _dbConnection;
        public StockRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public Task AddStockAsync(Stock stock)
        {
            throw new NotImplementedException();
        }

      

        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            var existingUser = await _dbConnection.QueryFirstOrDefaultAsync("select *from Users where Username=@Username", new { Username = request.Username });
            if (existingUser != null)
            {
                return new CreateUserResponse { Success = 0, Message = "Kullanıcı adı zaten alınmış." };
            }
            var sql = "insert into Users (Username, Email, PasswordHash, Balance,CreatedAt) values (@Username,@Email, @PasswordHash, @Balance, @CreatedAt); select CAST(SCOPE_IDENTITY() as int)";
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var userId = await _dbConnection.QuerySingleAsync<int>(sql, new
            {
                Username = request.Username,
                Email = "",
                PasswordHash = hashPassword,
                Balance = request.InitialBalance,
                CreatedAt = DateTime.UtcNow
            });
            var newUser = await _dbConnection.QueryFirstOrDefaultAsync("select *from Users where Id=@Id", new { Id = userId });
            return new CreateUserResponse { Success = 1, Message = "Kullanıcı başarıyla oluşturuldu.", CreatedUser = newUser };
        }

        public Task DeleteStockAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Stock>> GetAllStocksAsync()
        {
            var sql = "select *from Stocks";
            return await _dbConnection.QueryAsync<Stock>(sql);
        }

        public async Task<Stock> GetStockByIdAsync(int id)
        {
            var sql = "select *from Stocks where Id=@Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Stock>(sql, new { Id = id });
        }

        public async Task<Stock> GetStockBySymbolAsync(string symbol)
        {
            var sql = "select *from Stocks where Symbol=@Symbol";
            return await _dbConnection.QueryFirstOrDefaultAsync<Stock>(sql, new { Symbol = symbol });
        }

        public async Task<StockResponse> GetStocksPagedAsync(int pageNumber = 1, int pageSize = 100)
        {
            var sql = @"
            SELECT * FROM Stocks 
            ORDER BY Id 
            OFFSET @Offset ROWS 
            FETCH NEXT @PageSize ROWS ONLY";

            var offset = (pageNumber - 1) * pageSize;

            var stocks = await _dbConnection.QueryAsync<Stock>(sql, new
            {
                Offset = offset,
                PageSize = pageSize
            });
            var stockPriceHistorySql = "SELECT TOP 3 * FROM StockPriceHistory WHERE StockId = @StockId ORDER BY RecordedAt DESC";
            foreach (var stock in stocks)
            {
                var priceHistory = await _dbConnection.QueryAsync<StockPriceHistory>(stockPriceHistorySql, new { StockId = stock.Id });
                stock.PriceHistory = priceHistory.ToList();
            }

            var totalCount = await _dbConnection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Stocks");

            return new StockResponse
            {
                Stocks = stocks,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var sql = "SELECT * FROM Users WHERE Username = @Username";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new {Username = request.Username}); 
            var result = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (result)
            {
                return new LoginResponse
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Balance = user.Balance,
                    Success = 1
                };
            }
            return new LoginResponse { };
        }

        public Task<bool> UpdatePricesAsync()
        {
            var random = new Random();
            var stocks = _dbConnection.Query<Stock>("select *from Stocks").ToList();
            foreach (var stock in stocks)
            {
                if(stock.Price <= 0)
                {
                    stock.Price = (decimal)random.NextDouble()*10; // 0-10 arası başlangıç fiyatı
                }
                var changePercent = (decimal)(random.NextDouble() * 0.2 - 0.1); // +-%10
                var newPrice = stock.Price + (stock.Price * changePercent);
                newPrice = Math.Round(newPrice, 2);
                if (newPrice < 0.01m) newPrice = 0.01m;
                var updateSql = "update Stocks set Price=@Price, LastUpdated=@LastUpdated where Symbol=@Symbol";
                _dbConnection.Execute(updateSql, new { Price = newPrice, LastUpdated = DateTime.UtcNow, Symbol = stock.Symbol});
                var insertHistorySql = "insert into StockPriceHistory (StockId, Price, RecordedAt) values (@StockId, @Price, @RecordedAt)";
                _dbConnection.Execute(insertHistorySql, new { StockId = stock.Id, Price = newPrice, RecordedAt = DateTime.UtcNow });
            }
            return Task.FromResult(true);
        }

        public async Task<IEnumerable<StockPriceHistory>> GetPriceHistoryWithSymbol(string symbol)
        {
            var stockSql = "select *from Stocks where Symbol = @Symbol";
            var stock = await _dbConnection.QueryFirstAsync<Stock>(stockSql, new { Symbol = symbol });
            if (stock == null)
            {
                return Enumerable.Empty<StockPriceHistory>();
            }
            var sql = "SELECT * FROM StockPriceHistory WHERE StockId = @StockId ORDER BY RecordedAt DESC";
            var priceHistory = await _dbConnection.QueryAsync<StockPriceHistory>(sql, new { StockId = stock.Id });
            return priceHistory;
        }

        public async Task<List<PortfolioItem>> GetUserPortfolioAsync(int userId)
        {
            try
            {
                var portfolio = await _dbConnection.QueryAsync<PortfolioItem>(
                    "GetUserPortfolio",
                    new { UserId = userId },
                    commandType: CommandType.StoredProcedure
                );
                return portfolio.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Portföy getirme hatası: {ex.Message}");
                return new List<PortfolioItem>();
            }
        }

        public async Task<PortfolioSummary> GetPortfolioSummaryAsync(int userId)
        {
            try
            {
                var summary = await _dbConnection.QueryFirstOrDefaultAsync<PortfolioSummary>(
                    "GetPortfolioSummary",
                    new { UserId = userId },
                    commandType: CommandType.StoredProcedure
                );

                return summary ?? new PortfolioSummary();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Portföy özeti getirme hatası: {ex.Message}");
                return new PortfolioSummary();
            }
        }

        public async Task<List<TransactionHistory>> GetUserTransactionsAsync(int userId)
        {
            try
            {
                var transactions = await _dbConnection.QueryAsync<TransactionHistory>(
                    "GetUserTransactions",
                    new { UserId = userId },
                    commandType: CommandType.StoredProcedure
                );
                return transactions.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"İşlem geçmişi getirme hatası: {ex.Message}");
                return new List<TransactionHistory>();
            }
        }
        public async Task<BuyStockResponse> BuyStockAsync(int userId, int stockId, int quantity)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("UserId", userId);
                parameters.Add("StockId", stockId);
                parameters.Add("Quantity", quantity);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<BuyStockResult>(
                    "BuyStock",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                if (result.Result.Equals("SUCCESS"))
                {
                    return new BuyStockResponse
                    {
                        Success = true,
                        Message = "Hisse senedi başarıyla satın alındı.",
                        NewBalance = result.NewBalance,
                        TotalAmount = result.TotalAmount,
                        ExecutedPrice = result.ExecutedPrice
                    };
                }
                return new BuyStockResponse { Success = false, Message = result.ErrorMessage };
            }
            catch (Exception ex)
            {
                return new BuyStockResponse { Success = false, Message = ex.Message };
            }
        }
        public async Task<SellStockResponse> SellStockAsync(int userId, int stockId, int quantity, decimal currentPrice)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("UserId", userId);
                parameters.Add("StockId", stockId);
                parameters.Add("Quantity", quantity);
                parameters.Add("CurrentPrice", currentPrice);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<SellStockResponse>(
                    "SellStock",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new SellStockResponse { Success = false, Message = "Satış işlemi sonucu alınamadı" };
            }
            catch (Exception ex)
            {
                return new SellStockResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<OrderResult> AddBuyOrderAsync(int userId, int stockId, int quantity, decimal price)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("UserId", userId);
                parameters.Add("StockId", stockId);
                parameters.Add("Quantity", quantity);
                parameters.Add("Price", price);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<OrderResult>(
                    "AddBuyOrder",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new OrderResult { Success = false, Message = "Alım emri eklenemedi" };
            }
            catch (Exception ex)
            {
                return new OrderResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OrderResult> AddSellOrderAsync(int userId, int stockId, int quantity, decimal price)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("UserId", userId);
                parameters.Add("StockId", stockId);
                parameters.Add("Quantity", quantity);
                parameters.Add("Price", price);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<OrderResult>(
                    "AddSellOrder",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new OrderResult { Success = false, Message = "Satış emri eklenemedi" };
            }
            catch (Exception ex)
            {
                return new OrderResult { Success = false, Message = ex.Message };
            }
        }
        public async Task<List<PurchaseDetail>> GetUserStockPurchaseHistoryAsync(int userId, string stockSymbol)
        {
            try
            {
                var purchaseHistory = await _dbConnection.QueryAsync<PurchaseDetail>(
                    "GetUserStockPurchaseHistory",
                    new { UserId = userId, StockSymbol = stockSymbol },
                    commandType: CommandType.StoredProcedure
                );
                return purchaseHistory.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Alım geçmişi getirme hatası: {ex.Message}");
                return new List<PurchaseDetail>();
            }
        }
        public Task UpdateStockAsync(Stock stock)
        {
            throw new NotImplementedException();
        }
    }
}
