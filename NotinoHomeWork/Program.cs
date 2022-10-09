using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using NotinoHomeWork.API.MediaFormatters;
using NotinoHomeWork.Application;
using NotionHomeWork.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add application core services to the container
builder.Services.AddApplicationServices();

// Add infrastructure services to the container
builder.Services.AddInfrastructure();

builder.Services.AddControllers(opts =>
{
	// This API supports content negotiation.
	opts.RespectBrowserAcceptHeader = true;

	// Clear formaters
	opts.OutputFormatters.Clear();

	// Custom formaters
	opts.OutputFormatters.Add(new JsonStreamToJsonOutputFormatter());
	opts.OutputFormatters.Add(new JsonStreamToXmlOutputFormatter());
	opts.OutputFormatters.Add(new JsonStreamToMessagePackOutputFormatter());

	// Default formatters
	opts.OutputFormatters.Add(new HttpNoContentOutputFormatter());
	opts.OutputFormatters.Add(new StringOutputFormatter());
	opts.OutputFormatters.Add(new StreamOutputFormatter());
	opts.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions()));
});

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
