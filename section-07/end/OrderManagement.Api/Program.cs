using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Data;
using OrderManagement.Api.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;
using OrderManagement.Api;
using OrderManagement.Api.Background;
using OrderManagement.Api.Handlers;
using OrderManagement.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<ICreateOrderRequestHandler, CreateOrderRequestHandler>()
    .AddScoped<ICustomerRepository, CustomerRepository>()
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IOrderRepository, OrderRepository>();

builder.Services
    .AddScoped<IShippingService, ShippingApiClient>();

builder.Services.AddHttpClient("ShippingApi",
    client => client.BaseAddress = 
        new Uri(builder.Configuration["ShippingApi:BaseAddress"]!)
    );

builder.Services
    .AddSingleton<TimeProvider>(TimeProvider.System);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, HttpUserContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Management API", Version = "v1" });
    
    // Add JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add Authentication and Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderDb"))
        .UseSeeding((context, _) => // DEMO ONLY: This shouldn't be part of the startup on a production application
        {
            var customers = context.Set<Customer>();
            if (!customers.Any())
            {
                customers.Add(new Customer()
                {
                    Name = "Gui",
                    Email = "gui@guiferreira.me",
                    Id = 1,
                    PhoneNumber = "1234567890"
                });
                context.SaveChanges();
            }
            
            var products = context.Set<Product>();
            if (!products.Any())
            {
                products.Add(new Product()
                {
                    Name = "Computer",
                    Id = 1,
                    Price = 1000,
                    StockQuantity = 10,
                    Description = "What a great computer!"
                });
                context.SaveChanges();
            }
        });
        
});

builder.Services.AddHostedService<OrderCleanupService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


CreateDatabaseIfNotExists(app); // DEMO ONLY: This shouldn't be part of the startup on a production application
app.Run();
return;


// DEMO ONLY: This shouldn't be part of the startup on a production application
static void CreateDatabaseIfNotExists(WebApplication webApplication)
{
    using var scope = webApplication.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<OrderDbContext>();
    context.Database.EnsureCreated();
}
