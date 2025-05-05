namespace wx.Shared.Images;

public interface IFileService
{
    Task<string> SaveImageAsync(Stream fileStream, string fileName);
}
