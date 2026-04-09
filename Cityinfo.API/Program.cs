using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
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

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

//app.Run(async (context) => {
//    await context.Response.WriteAsync("Hello World!!");
//});  //Basic hello world code 

app.Run();
