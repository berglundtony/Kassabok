using Kassabok.Data;
using Kassabok.Repository;
using Kassabok.Functions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Kassabok.Functions.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IEntityFunctions,EntityFunctions>();
builder.Services.AddScoped<IFunctions, Functions>();
builder.Services.AddDbContext<TransactionsDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("TransactionsDbContext")));
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
