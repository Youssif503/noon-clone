using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using noon.Application;
using noon.Application.Helpers;
using noon.Application.Service.Contract;
using noon.Application.Services.Concrete;
using noon.Domain.Models.Identity;
using noon.Infrastructure;
using noon.Infrastructure.Data;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IConnectionMultiplexer>(op =>
{
    var config = builder.Configuration["Redis:Connection"];
    return ConnectionMultiplexer.Connect(config!);
});

builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(typeof(IUnitOfWork),typeof(UnitOfWork));
builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
builder.Services.AddScoped(typeof(IReviewService), typeof(ReviewService));
builder.Services.AddScoped(typeof(IImageService), typeof(ImageService));
builder.Services.AddScoped(typeof(ICartService), typeof(CartService));
builder.Services.AddScoped(typeof(ICacheService), typeof(CacheService));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<ImageResolver>();
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("JWT"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<User,IdentityRole>(op =>
    {
        op.User.RequireUniqueEmail = true;
        op.Password.RequiredLength = 5;
        op.Password.RequireNonAlphanumeric = false;
        op.Password.RequireLowercase = false;
        op.Password.RequireUppercase = false;
        op.Password.RequireDigit = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op =>
{
    op.RequireHttpsMetadata = false;
    op.SaveToken = true;
    op.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateIssuerSigningKey =  true,
        ValidIssuer =  builder.Configuration["JWT:Issuer"],
        
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        
        ValidateLifetime = true,
        
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]!))
    };
});


builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "images")),
    RequestPath = "/images"
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
