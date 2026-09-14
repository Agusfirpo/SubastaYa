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

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SubastaYaConnection"))
);

// Repositorios + Unit of Work
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

// Worker
builder.Services.AddHostedService<AuctionWorker>();

// SignalR
builder.Services.AddSignalR();

// CORS flexible para desarrollo en localhost
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 1. Enrutamiento
app.UseRouting();

// 2. CORS (debe ir inmediatamente después de UseRouting)
app.UseCors("Frontend");

// 3. Manejo de excepciones y seguridad
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();

// 4. Endpoints y Hubs
app.MapControllers();
app.MapHub<AuctionHub>("/hubs/auctionHub");
app.MapHub<AuctionHub>("/hubs/subastas");

app.Run();