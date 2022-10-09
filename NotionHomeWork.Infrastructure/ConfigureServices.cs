using Microsoft.Extensions.DependencyInjection;
using NotinoHomeWork.Application.Services;
using NotinoHomeWork.Services;
using NotionHomeWork.Infrastructure.Services;

namespace NotionHomeWork.Infrastructure;

public static class ConfigureServices
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		// Add services
		services.AddSingleton<IFileService, LocalDiskFileService>();

		// Add repos
		services.AddSingleton<IDocumentRepository, DocumentLocalDiskRepository>();

		return services;
	}
}