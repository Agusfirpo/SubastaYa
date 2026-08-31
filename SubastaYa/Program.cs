using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Billetera.Handler;
using Aplicacion.UseCases.Categoria.Handler;
using Aplicacion.UseCases.Puja.Handler;
using Aplicacion.UseCases.Subasta.Handler;
using Aplicacion.UseCases.Transaccion.Handler;
using Infraestructura.Persistence;
using Infraestructura.Repositories;
using Microsoft.EntityFrameworkCore;
using SubastaYa.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(
    builder.Configuration.GetConnectionString(
            "SubastaYaConnection"
        )
    )
);

//Builders

builder.Services.AddScoped<IBilleteraRepository, BilleteraRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ISubastaRepository, SubastaRepository>();
builder.Services.AddScoped<IPujaRepository, PujaRepository>();
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();
builder.Services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
builder.Services.AddScoped<IUnidadTrabajo, UnidadTrabajo>();


builder.Services.AddScoped<ObtenerSubastaPorIdHandler>();
builder.Services.AddScoped<ListarPujasPorSubastaHandler>();
builder.Services.AddScoped<ListarSubastasHandler>();
builder.Services.AddScoped<CrearSubastaHandler>();
builder.Services.AddScoped<ListarCategoriasHandler>();
builder.Services.AddScoped<ObtenerBilleteraHandler>(); 
builder.Services.AddScoped<AcreditarSaldoHandler>(); 
builder.Services.AddScoped<ListarSubastasPorVendedorHandler>();
builder.Services.AddScoped<FinalizarSubastasHandler>();
builder.Services.AddScoped<ListarParticipacionesHandler>();
builder.Services.AddScoped<ListarTransaccionesHandler>();

builder.Services.AddHostedService<SubastaWorker>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
