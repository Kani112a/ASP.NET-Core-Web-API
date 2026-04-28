using Cityinfo.API.DbContexts;
using Cityinfo.API.Model;
using Cityinfo.API.Service;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;

Log.Logger= new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.File("logs/cityinfo.txt", rollingInterval:RollingInterval.Day).CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
builder.Host.UseSerilog();
builder.Services.AddControllers(options=>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();  //through postman we need to accept the response in a additional format like xml addcontrollers like this or
                                            //need to send a response like not acceptable remove AddXmlDataContractSerializerFormatters
builder.Services.AddProblemDetails();
//manipulating problem details response
//builder.Services.AddProblemDetails(options =>  
//{
//    options.CustomizeProblemDetails = ctx =>
//    {
//        ctx.ProblemDetails.Extensions.Add("additional info", "additional info example");
//        ctx.ProblemDetails.Extensions.Add("server", Environment.MachineName);
//    };
//});
//output:from 404 response just to add extra content
//{
//    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
//    "title": "Not Found",
//    "status": 404,
//    "traceId": "00-f4b79712366e34997f1946789d109b03-5323e68169bc239e-00",
//    "additional info": "additional info example",
//    "server": "KANIPAVI"
//}


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>(); //to support while calling the file in plain/text content type
builder.Services.AddSingleton<CitiesDataStore>();
builder.Services.AddDbContext<cityInfoContext>(dbContextOptions => dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDbConnectionString"]));
#if DEBUG
builder.Services.AddTransient< IMailService, LocalMailService>(); //AddTransient is lightweight and stateless services
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif
    //builder.Services.AddScoped //created once per request
    //builder.Services.AddSingleton //lifetime services are created the first time they are requested
    builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options=> {
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"]))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeFromDharmapuri", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("city", "Dharmapuri");
    });
});
    var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())  //the environment variable refers development //production or stagging
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

//app.Run(async (context) => {
//    await context.Response.WriteAsync("Hello World!!");
//});  //Basic hello world code 

app.Run();
