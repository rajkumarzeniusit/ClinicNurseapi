

using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Serilog;
using System.Text;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TrudoseAdminPortalAPI.Data;
using TrudoseAdminPortalAPI.Helpers;
using TrudoseAdminPortalAPI.Interface;
using TrudoseAdminPortalAPI.Service;
using TrudoseAdminPortalAPI.Services;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configure Serilog for logging
Log.Debug("Start ConfigureServices (debug)");
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));


// Create a logger factory at the beginning
using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var logger = loggerFactory.CreateLogger<Program>();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var mySqlVersion = builder.Configuration.GetValue<string>("DatabaseSettings:MySQLVersion");
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(mySqlVersion))
    );
});



builder.Services.AddHttpContextAccessor();

//try
//{
//    var serviceProvider = builder.Services.BuildServiceProvider();
//    using (var scope = serviceProvider.CreateScope())
//    {
//        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//        // Fetch Redis config from database
//        var systemSettings = dbContext.system_settings.ToDictionary(s => s.config_name, s => s.config_value);

//        if (!systemSettings.ContainsKey("redis_host") || !systemSettings.ContainsKey("redis_port"))
//        {
//            logger.LogError("❌ Redis configuration is missing in system_settings.");
//            throw new ArgumentNullException("Redis configuration is missing in system_settings.");
//        }

//        // Read Redis config
//        string redisHost = systemSettings["redis_host"];
//        string redisPort = systemSettings["redis_port"];
//        string redisConnectionString = $"{redisHost}:{redisPort}";

//        logger.LogInformation("🔍 Connecting to Redis: {RedisConnectionString}", redisConnectionString);

//        // Register Redis Connection
//        var redis = ConnectionMultiplexer.Connect(redisConnectionString);

//        if (redis.IsConnected)
//        {
//            logger.LogInformation("✅ Redis Connected Successfully!");
//        }
//        else
//        {
//            logger.LogError("❌ Redis Connection Failed.");
//        }

//        // Add Redis service to DI container
//        builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
//    }
//}
//catch (Exception ex)
//{
//    logger.LogError(ex, "❌ Redis Connection Error");
//}







builder.Services.AddHttpContextAccessor();

// Configure JWT Authentication
var jwtSecretKey = builder.Configuration["JwtSettings:SecretKey"];
var key = Encoding.ASCII.GetBytes(jwtSecretKey);



builder.Services.AddScoped<JwtAuthenticationHandler>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtHandler = builder.Services.BuildServiceProvider().GetRequiredService<JwtAuthenticationHandler>();

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = jwtHandler.OnMessageReceived,
            OnTokenValidated = jwtHandler.OnTokenValidated
        };

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var roleNames = dbContext.roles.Select(r => r.role_name).ToList();

    builder.Services.Configure<AuthorizationOptions>(options =>
    {
        foreach (var role in roleNames)
        {
            if (!string.IsNullOrEmpty(role))
            {
                options.AddPolicy(role, policy => policy.RequireRole(role));
            }
        }
    });
}




builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API Documentation", Version = "v1" });

    // Add support for sending cookies in Swagger
    options.AddSecurityDefinition("cookieAuth", new OpenApiSecurityScheme
    {
        Name = "Cookie",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Cookie,
        Description = "JWT Access Token stored in cookies"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "cookieAuth"
                }
            },
            new string[] {}
        }
    });
});


// Add Swagger/OpenAPI service

builder.Services.AddEndpointsApiExplorer();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.SetIsOriginAllowed(origin => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
//    {
//        policy.WithOrigins("http://localhost:3000")
//              .AllowAnyHeader()
//              .AllowAnyMethod()
//              .AllowCredentials(); 
//    });
//});

builder.Services.AddScoped<ISymptomsMasterService, SymptomsMasterService>();
builder.Services.AddScoped<ISurveyMaster, SurveyMasterService>();
builder.Services.AddScoped<IQuestionsMaster, QuestionsMasterService>();
builder.Services.AddScoped<ISurveyQuestionMapService, SurveyQuestionmapService>();
builder.Services.AddScoped<ISurveyQuestionsList, SurveyQuestionsList>();
builder.Services.AddScoped<ISurveyClinicMap, SurveyClinicMapService>();
builder.Services.AddScoped<ISystemSettingsService, SystemSettingsService>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // Development environment configuration
        options.MinimumSameSitePolicy = SameSiteMode.Strict;
        options.Secure = CookieSecurePolicy.SameAsRequest;
        options.CheckConsentNeeded = context => false;
    }
    else
    {
        // Production environment configuration
        options.MinimumSameSitePolicy = SameSiteMode.None;
        options.CheckConsentNeeded = context => true;
    }
});

ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

var app = builder.Build();
app.Lifetime.ApplicationStarted.Register(() =>
{
    var server = app.Services.GetRequiredService<Microsoft.AspNetCore.Hosting.Server.IServer>();
    var addresses = server.Features.Get<IServerAddressesFeature>();

    if (addresses != null)
    {
        foreach (var address in addresses.Addresses)
        {
            Log.Information($"Application is running on {address}");
        }
    }
    else
    {
        Log.Warning("Unable to determine the application URL.");
    }
});



app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Admin Portal API v1"));

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
});

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}



app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

// Start the application
Log.Information("Trudose Admin Starting the application....");
try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Trudose Admin Application startup failed.");
    throw;
}
finally
{
    Log.Information("Trudose Admin Shutting down the application...");
    Log.CloseAndFlush();
}
