using NotinoHomeWork.Application.Services;

namespace NotionHomeWork.Infrastructure.Services;

public class LocalDiskFileService : IFileService
{
	public FileStream OpenRead(string path) => File.OpenRead(path);

	public FileStream OpenWrite(string path) => File.OpenWrite(path);

	public bool Exists(string path) => File.Exists(path);
}