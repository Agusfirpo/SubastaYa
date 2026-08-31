using Aplicacion.UseCases.Subasta.Handler;

namespace SubastaYa.Workers
{
    public class SubastaWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SubastaWorker(
            IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope =
                    _scopeFactory.CreateScope();

                var handler =
                    scope.ServiceProvider
                        .GetRequiredService<
                            FinalizarSubastasHandler>();

                try
                {
                    await handler.Handle();
                }
                catch (Exception)
                {
                    // Más adelante podemos agregar logging.
                }

                await Task.Delay(
                    TimeSpan.FromSeconds(10),
                    stoppingToken);
            }
        }
    }
}
