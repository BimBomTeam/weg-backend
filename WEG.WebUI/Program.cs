using WEG.Infrastructure.Services;
using WEG.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WEG.Application;
using WEG.Domain.Entities;
using Hangfire;
using Hangfire.Redis.StackExchange;
using WEG.Infrastructure.Queries;
using WEG.Application.Queries;
using WEG.Infrastructure.Commands;
using WEG.Application.Commands;
using HangfireBasicAuthenticationFilter;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

configuration.AddJsonFile("secrets.json", optional: true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Postgres
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure();
    }
));
//Hangfire
builder.Services.AddHangfire(cfg =>
    cfg.UseRedisStorage(
        builder.Configuration.GetConnectionString("HangfireConnection")
    ));

builder.Services.AddHangfireServer();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IAuthService, AuthService123>();
builder.Services.AddTransient<ILevelChangeService, LevelChangeService>();
builder.Services.AddTransient<IRolesService, RolesService>();
builder.Services.AddTransient<IAiCommunicationService, AiCommunicationService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
builder.Services.AddSingleton<IWordService, WordService>();
builder.Services.AddSingleton<IDialogService, DialogService>();
builder.Services.AddSingleton<IGameDayService, GameDayService>();
builder.Services.AddSingleton<IUserDailyProgressService, UserDailyProgressService>();

builder.Services.AddTransient<IGameDayQuery, GameDayQuery>();
builder.Services.AddTransient<INpcRolesQuery, NpcRoleQuery>();
builder.Services.AddTransient<IDailyProgressStatsQuery, DailyProgressStatsQuery>();
builder.Services.AddTransient<IWordsQuery, WordsQuery>();

builder.Services.AddTransient<IWordsCommand, WordsCommand>();
builder.Services.AddTransient<IDailyProgressStatsCommand, DailyProgressStatsCommand>();
builder.Services.AddTransient<IGameDayCommand, GameDayCommand>();
builder.Services.AddTransient<INpcRoleCommand, NpcRoleCommand>();

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var app = builder.Build();

var loggerFactory = app.Services.GetService<ILoggerFactory>();
loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());


app.UseCors(x => x
   .AllowAnyMethod()
   .AllowAnyHeader()
   .SetIsOriginAllowed(origin => true) // allow any origin
                                       //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
   .AllowCredentials()); // allow credentials

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHangfireDashboard("/api/hangfire", new DashboardOptions
{
    //AppPath = "" //The path for the Back To Site link. Set to null in order to hide the Back To  Site link.
    DashboardTitle = "Aicademia",
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter{
            User = configuration.GetSection("HangfireSettings:UserName").Value,
            Pass = configuration.GetSection("HangfireSettings:Password").Value
        }
    }
});
app.MapControllers();

RecurringJob.AddOrUpdate<IRolesService>("ChangeRole",
    x => x.GenerateNewWordsAsync(),
    Cron.Daily(22, 01)); //Czas UTC (2h do tyłu. U nas będzie 00:01));

app.Run();
