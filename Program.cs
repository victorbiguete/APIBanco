using APIBanco.Services;
using APIBanco.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using APIBanco.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlite(defaultConnectionString);
});
builder.Services.AddScoped<IdentityDbContext>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<BankAccountService>();
builder.Services.AddScoped<TransactionsService>();
builder.Services.AddScoped<AdressService>();

// autoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
