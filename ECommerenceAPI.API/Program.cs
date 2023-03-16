using ECommerenceAPI.Application;
using ECommerenceAPI.Application.Validators;
using ECommerenceAPI.Infrastructure;
using ECommerenceAPI.Infrastructure.Filters;
using ECommerenceAPI.Infrastructure.Services.Storage.Azure;
using ECommerenceAPI.Infrastructure.Services.Storage.Local;
using ECommerenceAPI.Persistance;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()

builder.Services.AddPersistanceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationService();
builder.Services.AddStorage<AzureStorage>(); // mimari storage yönlendirilmesi
//builder.Services.AddStorage<LocalStorage>(); // mimari storage yönlendirilmesi

//builder.Services.AddStorage(ECommerenceAPI.Infrastructure.Enums.StorageType.Local);
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin",options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, //oluşturulacak token değerini kimlerin/hangi originlerin sitlerein doğrulanacağı =>www.caartcurt.com
            ValidateIssuer = true, //oluşturulan tokenin kimin dağıttığı => www.myapi.com
            ValidateLifetime = true, // tokenın süresini kontrol et
            ValidateIssuerSigningKey = true, // üretilecek token keyinin uygulamaya ait olduğu belli eden keydir

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
        };
    });
    

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
