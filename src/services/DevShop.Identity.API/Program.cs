using DevShop.Identity.API.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

#region Configure Services


builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDependencyInjection(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddApiConfiguration(builder.Configuration);

//builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();



var app = builder.Build();

#endregion


#region Configure Pipeline

var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
app.UseSwaggerConfiguration(provider);

app.UseApiConfiguration(builder.Environment, builder.Configuration);

app.Run();
#endregion