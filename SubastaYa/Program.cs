using Aplicacion.Interfaces.Repositories;
<<<<<<< HEAD
using Aplicacion.UseCases.Subasta.Handler;
=======
using Aplicacion.UseCases.Billetera.Handler;
using Aplicacion.UseCases.Categoria.Handler;
>>>>>>> f24ca4abb52998d28df5d5a0c2173dd80ec193ab
using Infraestructura.Persistence;
using Infraestructura.Repositories;
using Microsoft.EntityFrameworkCore;

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


builder.Services.AddScoped<ListarSubastasHandler>();
builder.Services.AddScoped<CrearSubastaHandler>();
builder.Services.AddScoped<ListarCategoriasHandler>();
builder.Services.AddScoped<ObtenerBilleteraHandler>();

builder.Services.AddScoped<ISubastaRepository, SubastaRepository>();


builder.Services.AddScoped<ListarSubastasHandler>();
builder.Services.AddScoped<CrearSubastaHandler>();


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
