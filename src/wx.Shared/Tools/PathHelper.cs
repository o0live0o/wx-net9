using System.Text;

namespace wx.Shared.Tools;

public static class PathHelper
{
    public static string CombineLanguageExt(string filePath, string languageExt)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return string.Empty;
        }

        if (string.IsNullOrEmpty(languageExt))
        {
            return filePath;
        }

        var ext = Path.GetExtension(filePath);
        var fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
        var directoryName = Path.GetDirectoryName(filePath);

        var dotLanguageExt = "." + languageExt;
        return Path.Combine(directoryName ?? string.Empty, fileNameWithoutExt + dotLanguageExt + ext);
    }

    public static string CombineLanguageExt2(string filePath, string languageExt)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return string.Empty;
        }

        if (string.IsNullOrEmpty(languageExt))
        {
            return filePath;
        }

        var ext = Path.GetExtension(filePath);
        var fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
        var directoryName = Path.GetDirectoryName(filePath);

        StringBuilder sb = new StringBuilder(filePath.Length + languageExt.Length + 1);
        sb.Append(directoryName ?? string.Empty);
        sb.Append(directoryName?.Length > 0 ? Path.DirectorySeparatorChar : "");
        sb.Append(fileNameWithoutExt);
        sb.Append('.');
        sb.Append(languageExt);
        sb.Append(ext);
        return sb.ToString();
    }
}