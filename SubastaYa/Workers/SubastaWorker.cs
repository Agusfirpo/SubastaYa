using Aplicacion.UseCases.Subasta.Handler;

namespace SubastaYa.Workers
{
    public class SubastaWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SubastaWorker(IServiceScopeFactory scopeFactory)
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
                    .GetRequiredService<ProcesarSubastasProgramadasHandler>();

                var finalizar = scope.ServiceProvider
                    .GetRequiredService<FinalizarSubastasHandler>();

                try
                {
                    await programadas.Handle();
                    await finalizar.Handle();
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
