using Aplicacion.Interfaces.Handlers;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Billetera.Handler;
using Aplicacion.UseCases.Categoria.Handler;
using Aplicacion.UseCases.Puja.Handler;
using Aplicacion.UseCases.Subasta.Handler;
using Aplicacion.UseCases.Transaccion.Handler;
using Aplicacion.UseCases.Usuario.Handler;
using Infraestructura.Persistence;
using Infraestructura.Repositories;
using Microsoft.EntityFrameworkCore;
using SubastaYa.Hubs;
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
builder.Services.AddScoped<INotificadorSubasta, NotificadorSubasta>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<LoginHandler>();
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
builder.Services.AddScoped<RealizarPujaHandler>();
builder.Services.AddScoped<ProcesarSubastasProgramadasHandler>();


builder.Services.AddHostedService<SubastaWorker>();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontSubastaYa", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



var passwordHash = BCrypt.Net.BCrypt.HashPassword("1234");
Console.WriteLine($"PASSWORD HASH: {passwordHash}");


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

app.UseCors("FrontSubastaYa");

app.MapHub<SubastaHub>("/hubs/subastas");

app.Run();
