using ECommerenceAPI.Application.Validators;
using ECommerenceAPI.Infrastructure.Filters;
using ECommerenceAPI.Persistance;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()

builder.Services.AddPersistanceServices();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
  policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()

));

//kendi filterımızı ekledik
builder.Services.AddControllers(options=>options.Filters.Add<ValidationFilter>())// reflection ile bütün validatorleri ekleyecek otoatik
    .AddFluentValidation(conf=>conf.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options=>options.SuppressModelStateInvalidFilter=true); // mevcut olan default filterları görmezden gelmesi için yazdık. Yani 
// api'de controllere gelmeden oto hatalrı yakalamamsı için
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
