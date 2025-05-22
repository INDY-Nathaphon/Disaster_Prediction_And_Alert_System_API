using Disaster_Prediction_And_Alert_System_API.BusinessLogic.DisasterPredictionAndAlert.Interface;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.DisasterPredictionAndAlert.Service;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.ExternalApi;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.RedisCache;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.TransactionManager;
using Disaster_Prediction_And_Alert_System_API.Domain;
using Disaster_Prediction_And_Alert_System_API.Middleware;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureAppSettings(builder.Configuration);
builder.Services.AddHttpClient<IExternalApiService, ExternalApiService>();

var redis = ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false");

builder.Services.AddSingleton<RedisCacheService>();

#region Database

var connectionString = builder.Configuration.GetConnectionString("DbConnection");

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "AuthService_";
});

#endregion

#region Add Service

builder.Services.AddScoped<ITransactionManagerService, TransactionManagerService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<DbContext, AppDBContext>();
builder.Services.AddScoped<IDisasterPredictionAndAlertFacadeService, DisasterPredictionAndAlertFacadeService>();
builder.Services.AddScoped<IDisasterPredictionAndAlertService, DisasterPredictionAndAlertService>();
builder.Services.AddScoped<IExternalApiService, ExternalApiService>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
