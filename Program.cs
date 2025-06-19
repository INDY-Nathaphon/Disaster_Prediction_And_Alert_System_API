using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.ExternalApi;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.RedisCache;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.TransactionManager;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterPredictionAndAlert.Interface;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterPredictionAndAlert.Service;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterRiskReport.Faacade;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterRiskReport.Interface;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterRiskReport.Service;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Region.Facade;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Region.Interface;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Region.Service;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Scheduler;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.User.Facade;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.User.Interface;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.User.Service;
using Disaster_Prediction_And_Alert_System_API.Domain;
using Disaster_Prediction_And_Alert_System_API.Middleware;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

#region Add Serilog log

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

#endregion

// ตรวจสอบว่า builder.Configuration มีค่าไหม
if (builder.Configuration == null)
{
    throw new Exception("Configuration is null");
}

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

builder.Services.AddScoped<IAlertSettingFacadeService, AlertSettingFacadeService>();
builder.Services.AddScoped<IAlertSettingService, AlertSettingService>();

builder.Services.AddScoped<IUserFacadeService, UserFacadeService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IDisasterRiskReportFacadeService, DisasterRiskReportFacadeService>();
builder.Services.AddScoped<IDisasterRiskReportService, DisasterRiskReportService>();

builder.Services.AddScoped<IRegionFacadeService, RegionFacadeService>();
builder.Services.AddScoped<IRegionService, RegionService>();

builder.Services.AddScoped<IExternalApiService, ExternalApiService>();

#endregion

#region Job

builder.Services.AddHostedService<SmsJobService>();
builder.Services.AddHostedService<AlertJobService>();

#endregion

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();

    var policy = Policy
    .Handle<SqlException>()
    .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(5));

    policy.Execute(() =>
    {
        db.Database.Migrate();
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Serilog Middleware 
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
