using ECommerenceAPI.API.Configrations.ColumnWriters;
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
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.HttpLogging;
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
Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration["Seq:ServerUrl"])
    .WriteTo.File("logs/log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"),"logs",
    needAutoCreateTable:true,
    columnOptions:new Dictionary<string, ColumnWriterBase>
    {
        {"message",new RenderedMessageColumnWriter() },
        {"message_template",new MessageTemplateColumnWriter() },
        {"level", new LevelColumnWriter() },
        {"time_stamp", new TimestampColumnWriter() },
        {"exception", new ExceptionColumnWriter() },
        {"log_event",new LogEventSerializedColumnWriter() },
        {"user_name",new UserNameColumnWriter() }
    }
    )
    .Enrich.FromLogContext() // harici propları kullanmak için
    .MinimumLevel.Information()
    .CreateLogger();
builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

            NameClaimType = ClaimTypes.Name //jwt üzerinde name claimine karşılık gelene değeri user.identiy.name propudan elde edebiliriz
        };
    });
    

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseSerilogRequestLogging();
app.UseHttpLogging();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//auth ve autherize dan sonra user_name'i alıp user_name prop'una atıcak mid ekledik
app.Use(async (context, next) =>
{ 
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name",username);
    await next();
});

app.MapControllers();

app.Run();
