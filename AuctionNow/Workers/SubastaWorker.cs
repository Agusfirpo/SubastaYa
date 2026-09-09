using Aplicacion.UseCases.Subasta.Command;
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
                    var ahora = DateTime.UtcNow;

                    await programadas.Handle(
                        new ProcesarSubastasProgramadasCommand
                        {
                            FechaActual = ahora
                        });

                    await finalizar.Handle(
                        new FinalizarSubastasCommand
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
