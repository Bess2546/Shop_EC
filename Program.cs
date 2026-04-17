using Shop_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Shop_Backend.StoreServices;
using Shop_Backend.UserService;
using Shop_Backend.Repositories;
using Shop_Backend.ProductService;
using Shop_Backend.CartService;
using Shop_Backend.OrdersService;
using Shop_Backend.UploadService;
using Shop_Backend.Auth;          
using Shop_Backend.Upload;        
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Shop_Backend.middleware;

var builder = WebApplication.CreateBuilder(args);

// ==================== Database ====================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==================== Framework ====================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==================== Configuration (Options Pattern) ====================
builder.Services
    .AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection(JwtSettings.SectionName))
    .Validate(s => !string.IsNullOrWhiteSpace(s.Key),
        "JWT Key is missing. Set via user-secrets or environment variable.")
    .Validate(s => Encoding.UTF8.GetByteCount(s.Key) >= 32,
        "JWT Key must be at least 32 bytes (256 bits).")
    .Validate(s => !string.IsNullOrWhiteSpace(s.Issuer), "JWT Issuer is missing.")
    .Validate(s => !string.IsNullOrWhiteSpace(s.Audience), "JWT Audience is missing.")
    .ValidateOnStart();

builder.Services
    .AddOptions<SupabaseSettings>()
    .Bind(builder.Configuration.GetSection(SupabaseSettings.SectionName))
    .Validate(s => !string.IsNullOrWhiteSpace(s.Url), "Supabase URL is missing.")
    .Validate(s => !string.IsNullOrWhiteSpace(s.Key), "Supabase Key is missing.")
    .ValidateOnStart();

// ==================== Repositories ====================
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// ==================== Services ====================
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpClient<IUploadService, UploadService>();

// ==================== Authentication ====================
var jwtSection = builder.Configuration.GetSection(JwtSettings.SectionName);
var jwtKey = jwtSection["Key"]
    ?? throw new InvalidOperationException("JWT Key is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,                    
            ClockSkew = TimeSpan.FromMinutes(1),        
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// ==================== CORS ====================
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ==================== Middleware Pipeline ====================
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();