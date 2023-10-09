using Microsoft.EntityFrameworkCore;
using WebApi;
using WebApi.Datos;
using WebApi.Repositorio;
using WebApi.Repositorio.IRepositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AplicationDbContext>(option=>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}

);

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<iProductRepositorio,ProductRepositorio>();

builder.Services.AddScoped<iNumberProductRepositorio,NumberProductRepositorio>();

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
