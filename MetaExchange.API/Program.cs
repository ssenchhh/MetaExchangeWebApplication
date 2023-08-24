using FluentValidation;
using FluentValidation.AspNetCore;
using MetaExchange.API.Controllers.Requests;
using MetaExchange.API.Data.FileProviders;
using MetaExchange.API.Data.Repositories;
using MetaExchange.API.Models;
using MetaExchange.API.Services.Interfaces;
using MetaExchange.API.Services;
using MetaExchange.API.Validators;
using MetaExchange.API.Data.Providers.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); }); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IDataProvider<OrderBook>, RowDataProvider<OrderBook>>();
builder.Services.AddScoped<IRepository<OrderBook>, OrderBookRepository>();
builder.Services.AddScoped<IMetaExchangeService, MetaExchangeService>();

builder.Services.AddScoped<IValidator<ExchangeRequest>, ExchangeRequestValidator>();

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
