using Application.UseCases.Subasta.Command;
using Application.UseCases.Subasta.Handler;

namespace Api_SubastaYa.Workers
{
    public class AuctionWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AuctionWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var programadas = scope.ServiceProvider
                    .GetRequiredService<ProcessScheduledAuctionsHandler>();

                var finalizar = scope.ServiceProvider
                    .GetRequiredService<FinishAuctionsHandler>();

                try
                {
                    var ahora = DateTime.UtcNow;

                    await programadas.Handle(
                        new ProcessScheduledAuctionsCommand
                        {
                            FechaActual = ahora
                        });

                    await finalizar.Handle(
                        new FinishAuctionsCommand
                        {
                            FechaActual = ahora
                        });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Error procesando subastas: {ex.Message}");
                }

                await Task.Delay(
                    TimeSpan.FromSeconds(1),
                    stoppingToken);
            }
        }
    }
}
