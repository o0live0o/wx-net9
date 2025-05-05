
using Microsoft.Extensions.Configuration;

namespace wx.Shared.Images;

public class FileService : IFileService
{
    private readonly IConfiguration _configuration;
    private readonly string _imagePath;
    public FileService(IConfiguration configuration)
    {
        _configuration = configuration;
        _imagePath = _configuration["FileStorage:ImagePath"];
    }
    public async Task<string> SaveImageAsync(Stream fileStream, string fileName)
    {
        Directory.CreateDirectory(_imagePath);

        string filePath = Path.Combine(_imagePath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(stream);

        return filePath;
    }
}
