using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Walks.API.Data;
using Walks.API.Mapping;
using Walks.API.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Walks.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);


//--------------------------------------------------------------------------------
// --- Log System ----------------------------------------------------------------
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/Walks_Log.txt", rollingInterval: RollingInterval.Minute)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

//--------------------------------------------------------------------------------
// --- Add services to the container ---------------------------------------------
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
//--------------------------------------------------------------------------------

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Walks API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//--------------------------------------------------------------------------------
// --- ADDING DB'S --------------------------------------------------------------
builder.Services.AddDbContext<WalksDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("WalksCS")));
builder.Services.AddDbContext<WalksAuthDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("WalksAuthCS")));
//--------------------------------------------------------------------------------

//--------------------------------------------------------------------------------
builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, SQLTokenRepository>();
builder.Services.AddScoped<IImageRepository, SQLLocalImageRespository>();
//--------------------------------------------------------------------------------

//--------------------------------------------------------------------------------
// --- AUTOMAPPER (DTO<->DB) MODELS ----------------------------------------------
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
//--------------------------------------------------------------------------------

//--------------------------------------------------------------------------------
// --- ADD IDENTITY --------------------------------------------------------------
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Walks")
    .AddEntityFrameworkStores<WalksAuthDbContext>()
    .AddDefaultTokenProviders();

// -- Config identity
builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});
//--------------------------------------------------------------------------------

// --- Adding Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//--------------------------------------------------------------------------------
// --- Add Middleware's ----------------------------------------------------------
app.UseMiddleware<ExceptionHandler>();
//--------------------------------------------------------------------------------


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//--------------------------------------------------------------------------------
// --- ALLOW STATIC FILES --------------------------------------------------------
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.MapControllers();

app.Run();
