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
using Asp.Versioning;
using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.ApplicationInsights.Extensibility;

Log.Logger= new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.File("logs/cityinfo.txt", rollingInterval:RollingInterval.Day)
    .WriteTo.ApplicationInsights(new TelemetryConfiguration()
    {
        InstrumentationKey=""
    },
    TelemetryConverter.Traces)
    .CreateLogger();

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
builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);
}).AddMvc()
.AddApiExplorer(setupAction =>
{
    setupAction.SubstituteApiVersionInUrl = true;
}
);

var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

builder.Services.AddSwaggerGen(setupAction =>
{
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        setupAction.SwaggerDoc(
            $"{description.GroupName}",
            new()
            {
                Title = "City info API",
                Version = description.ApiVersion.ToString(),
                Description = "Through this API you can access cities and their points of interest"
            });
    }
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

setupAction.IncludeXmlComments(xmlCommentsFullPath);

setupAction.AddSecurityDefinition("CityInfoApiBearerAuth", new()
{
    Type = SecuritySchemeType.Http,
    Scheme = "Bearer",
    Description = "Input a valid token to access this API"
});
    setupAction.AddSecurityRequirement(new()
    {
        {
            new()
            {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "CityInfoApiBearerAuth"
        }
        },
        new List<string>()
        }
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}
app.UseForwardedHeaders();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())  //the environment variable refers development //production or stagging
//{
    app.UseSwagger();
    app.UseSwaggerUI(setupAction =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach(var description in descriptions)
        {
            setupAction.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
//}

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
