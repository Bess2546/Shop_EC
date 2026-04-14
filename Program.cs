using Shop_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Shop_Backend.StoreServices;
using Shop_Backend.UserService;
using Shop_Backend.Repositories;
using Shop_Backend.ProductService;
using Shop_Backend.CartService;
using Shop_Backend.OrdersService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


