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
    //public static string CombineLanguageExt3(string filePath, string languageExt)
    //{
    //    if (string.IsNullOrEmpty(filePath))
    //    {
    //        return string.Empty;
    //    }

    //    if (string.IsNullOrEmpty(languageExt))
    //    {
    //        return filePath;
    //    }

    //    var ext = Path.GetExtension(filePath).AsSpan(); // 获取扩展名
    //    var fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath).AsSpan(); // 获取文件名（无扩展名）
    //    var directoryName = Path.GetDirectoryName(filePath); // 获取目录名

    //    // 计算最终字符串的长度
    //    int finalLength = (directoryName?.Length ?? 0) +
    //                      (directoryName?.Length > 0 ? 1 : 0) + // 如果有目录，加上路径分隔符
    //                      fileNameWithoutExt.Length +
    //                      1 + // 加上 '.' 分隔符
    //                      languageExt.Length +
    //                      ext.Length;

    //    // 使用 string.Create 创建字符串
    //    return string.Create(finalLength, (directoryName, fileNameWithoutExt, languageExt, ext), (span, state) =>
    //    {
    //        int offset = 0;

    //        // 添加目录名
    //        if (state.directoryName != null)
    //        {
    //            state.directoryName.AsSpan().CopyTo(span);
    //            offset += state.directoryName.Length;

    //            // 添加路径分隔符
    //            span[offset] = Path.DirectorySeparatorChar;
    //            offset++;
    //        }

    //        // 添加文件名（无扩展名）
    //        state.fileNameWithoutExt.CopyTo(span.Slice(offset));
    //        offset += state.fileNameWithoutExt.Length;

    //        // 添加语言扩展的分隔符 '.'
    //        span[offset] = '.';
    //        offset++;

    //        // 添加语言扩展
    //        state.languageExt.AsSpan().CopyTo(span.Slice(offset));
    //        offset += state.languageExt.Length;

    //        // 添加扩展名
    //        state.ext.CopyTo(span.Slice(offset));
    //    });
    //}
}