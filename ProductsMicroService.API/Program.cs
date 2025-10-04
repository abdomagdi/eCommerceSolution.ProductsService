using BusinessLogicLayer;
using BusinessLogicLayer.Mappers;
using DataAccessLayer;
using FluentValidation.AspNetCore;
using ProductsMicroService.API.Middlewares;
using BusinessLogicLayer.Mappers;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogicLayer.APIEndpints;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBusinessLogic();
builder.Services.AddDataAccessLayer(builder.Configuration);

builder.Services.AddControllers();
//builder.Services.AddAutoMapper(typeof(ApplicationUserMappingProfile).Assembly);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});// For Converting Enum to string in JSON Response

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options=>
{
    options.AddDefaultPolicy( builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapProductsAPIEndpoints();
app.Run();
