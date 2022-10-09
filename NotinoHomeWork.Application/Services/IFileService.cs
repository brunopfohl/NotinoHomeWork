namespace NotinoHomeWork.Application.Services;

public interface IFileService
{
	FileStream OpenRead(string path);
	FileStream OpenWrite(string path);
	bool Exists(string path);
}