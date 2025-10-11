using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockExchange.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.BAL.Services
{
    public class StockTaskService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<StockTaskService> _logger;

        public StockTaskService(IServiceScopeFactory serviceScopeFactory, ILogger<StockTaskService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var stockRepository = scope.ServiceProvider.GetRequiredService<IStockRepository>();

                        await stockRepository.UpdatePricesAsync(); // Gerçekçi olması için her dakika fiyatları +- %10 arasında günceller.
                        await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Her 5 dakikada güncelleme yap.
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Hisse senedi fiyatlarını güncellerken hata oluştu.");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Hata durumunda 1 dakika bekle.
                }
            }
        }
    }
}
