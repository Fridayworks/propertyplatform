namespace PropertyPlatform.Core.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string folderName);
        Task DeleteFileAsync(string fileUrl);
    }
}
