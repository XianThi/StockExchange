using Dapper;
using StockExchange.Core.Interfaces.Services;
using StockExchange.Core.Models.ApiModel;
using StockExchange.Core.Models.StockModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockExchange.DAL
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IDbConnection _dbConnection;
        public DatabaseService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<bool> SeedDataAsync()
        {
            try
            {
                var dataExists = await _dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM Stocks");
                if (dataExists > 0)
                {
                    //("Veritabanında zaten veri var, seed işlemi atlandı.");
                    return true;
                }
                var data = await FetchDataFromApiAsync();
                return await InsertDataAsync(data);
            }
            catch (Exception ex)
            {
                //(ex, "Seed data işlemi sırasında hata oluştu");
                return false;
            }
        }
        private async Task<List<ApiData>> FetchDataFromApiAsync()
        {
            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync("https://bigpara.hurriyet.com.tr/api/v1/hisse/list");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Hata kodu: {response.StatusCode}");
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return apiResponse.data ?? new List<ApiData>();
        }

        private async Task<ApiStockResponse> FetchStockDataFromApiAsync(string symbol)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://bigpara.hurriyet.com.tr/api/v1/borsa/hisseyuzeysel/"+symbol);
            if (!response.IsSuccessStatusCode)
            {
                return new ApiStockResponse();
            }
            var jsonContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiStockResponse>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return apiResponse ?? new ApiStockResponse();
        }

        public async Task<bool> InsertDataAsync(List<ApiData> data)
        {
            var packet = new List<Stock>();
            foreach (var item in data)
            {
                //var stockData = await FetchStockDataFromApiAsync(item.kod);
                var stock = new Stock
                {
                    Symbol = item.kod,
                    Name = item.ad,
                    Description = "",
                    Price =  0,
                    TotalShares = 0,
                    LastUpdated = DateTime.UtcNow
                };
                packet.Add(stock);
            }
            const string sql = @"
            INSERT INTO Stocks (Symbol, Name, Description, Price, TotalShares, LastUpdated)
            VALUES (@Symbol, @Name, @Description, @Price, @TotalShares, @LastUpdated)";
            var affectedRows = await _dbConnection.ExecuteAsync(sql, packet);

            //("{Count} ürün veritabanına eklendi", affectedRows);

            return affectedRows > 0;
        }
    }
}
