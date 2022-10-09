using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NotinoHomeWork.Application.Services;

namespace NotinoHomeWork.Application;

public static class ConfigureServices
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		// Add converters
		services.AddSingleton<JsonToMessagePackConverter>();
		services.AddSingleton<JsonToXmlConverter>();

		return services;
	}
}