using Dapper;
using StockExchange.Core.Interfaces.Repositories;
using StockExchange.Core.Models.StockModel;
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

        public async Task<BuyStockResponse> BuyStock(BuyStockRequest request)
        {
            var user = await _dbConnection.QueryFirstOrDefaultAsync("select *from Users where Id=@Id", new { Id = request.UserId });
            if (user == null)
            {
                return new BuyStockResponse { Success = 0, Message = "Kullanıcı bulunamadı." };
            }
            var stock = await _dbConnection.QueryFirstOrDefaultAsync("select *from Stocks where Symbol=@Symbol", new { Symbol = request.Symbol });
            if (stock == null)
            {
                return new BuyStockResponse { Success = 0, Message = "Hisse senedi bulunamadı." };
            }
            var totalPrice = stock.Price * request.Quantity;
            if (user.Balance < totalPrice)
            {
                return new BuyStockResponse { Success = 0, Message = "Yetersiz bakiye." };
            }
            if (stock.Quantity < request.Quantity)
            {
                return new BuyStockResponse { Success = 0, Message = "Yetersiz hisse senedi miktarı." };
            }
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    var updateUserSql = "update Users set Balance=Balance-@Amount where Id=@Id";
                    await _dbConnection.ExecuteAsync(updateUserSql, new { Amount = totalPrice, Id = user.Id }, transaction);
                    var updateStockSql = "update Stocks set Quantity=Quantity-@Quantity where Id=@Id";
                    await _dbConnection.ExecuteAsync(updateStockSql, new { Quantity = request.Quantity, Id = stock.Id }, transaction);
                    var insertTransactionSql = "insert into UserStock (UserId, StockId, Quantity, PurchasePrice, PurchasedAt) values (@UserId, @StockId, @Quantity, @Price, @TransactionDate)";
                    await _dbConnection.ExecuteAsync(insertTransactionSql, new
                    {
                        UserId = user.Id,
                        StockId = stock.Id,
                        Quantity = request.Quantity,
                        Price = stock.Price,
                        TransactionDate = DateTime.UtcNow
                    }, transaction);
                    transaction.Commit();
                    var updatedUser = await _dbConnection.QueryFirstOrDefaultAsync<User>("select *from Users where Id=@Id", new { Id = user.Id });
                    _dbConnection.Close();
                    return new BuyStockResponse { Success = 1, Message = "Hisse senedi başarıyla satın alındı.", UpdatedUser = updatedUser };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _dbConnection.Close();
                    return new BuyStockResponse { Success = 0, Message = "Hata oluştu: " + ex.Message };
                }
            }
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

        public Task UpdateStockAsync(Stock stock)
        {
            throw new NotImplementedException();
        }
    }
}
