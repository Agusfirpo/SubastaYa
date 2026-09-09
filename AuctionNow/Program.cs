using Application.Interfaces.Handlers;
using Application.Interfaces.Repositories;
using Application.UseCases.Billetera.Handler;
using Application.UseCases.Categoria.Handler;
using Application.UseCases.Puja.Handler;
using Application.UseCases.Subasta.Handler;
using Application.UseCases.Transaccion.Handler;
using Application.UseCases.Usuario.Handler;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Api_SubastaYa.Hubs;
using Api_SubastaYa.Workers;
using Api_SubastaYa.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SubastaYaConnection"))
);

// Repositorios y Unit of Work
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IBidRepository, BidRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuctionNotifier, AuctionNotifier>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Handlers
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<GetAuctionByIdHandler>();
builder.Services.AddScoped<GetBidsByAuctionHandler>();
builder.Services.AddScoped<GetAuctionsHandler>();
builder.Services.AddScoped<CreateAuctionHandler>();
builder.Services.AddScoped<GetCategoriesHandler>();
builder.Services.AddScoped<GetWalletHandler>();
builder.Services.AddScoped<CreditBalanceHandler>();
builder.Services.AddScoped<GetAuctionsBySellerHandler>();
builder.Services.AddScoped<FinishAuctionsHandler>();
builder.Services.AddScoped<GetParticipationsHandler>();
builder.Services.AddScoped<GetTransactionsHandler>();
builder.Services.AddScoped<PlaceBidHandler>();
builder.Services.AddScoped<ProcessScheduledAuctionsHandler>();

builder.Services.AddHostedService<AuctionWorker>();
builder.Services.AddSignalR();

// CORS configurado para admitir SignalR desde Blazor WebAssembly
builder.Services.AddCors(options =>
{
    options.AddPolicy("Front_AuctionNow", policy =>
    {
        policy
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

// CORS DEBE IR ANTES de Authorization, Controllers y Hubs
app.UseCors("Front_AuctionNow");

app.UseAuthorization();

app.MapControllers();

// Mapeamos ambas rutas para asegurar compatibilidad inmediata
app.MapHub<AuctionHub>("/hubs/auctionHub");
app.MapHub<AuctionHub>("/hubs/subastas");

app.Run();