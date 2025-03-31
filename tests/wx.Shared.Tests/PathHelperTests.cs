using wx.Shared.Tools;

namespace wx.Shared.Tests;

public class PathHelperTests
{
    [Theory]
    [InlineData(@"xxx/wwa/www.resx", "zh-hans", @"xxx/wwa/www.zh-hans.resx")]
    [InlineData(@"www.resx", "zh-hans", @"www.zh-hans.resx")]
    [InlineData(@"c:\test\res.resx", "zh-hans", @"c:\test\res.zh-hans.resx")]
    public void CombineLanguageExt_ShouldHandleLanguageCorrectly(
       string input, string languageExt, string expected)
    {
        // Arrange & Act
        var result = PathHelper.CombineLanguageExt(input, languageExt);

        // Assert - 规范化路径后比较
        Assert.Equal(
            expected.Replace('\\', Path.DirectorySeparatorChar)
                   .Replace('/', Path.DirectorySeparatorChar),
            result.Replace('\\', Path.DirectorySeparatorChar)
                 .Replace('/', Path.DirectorySeparatorChar));
    }

    [Theory]
    [InlineData(@"xxx/wwa/www.resx", "zh-hans", @"xxx/wwa/www.zh-hans.resx")]
    [InlineData(@"www.resx", "zh-hans", @"www.zh-hans.resx")]
    [InlineData(@"c:\test\res.resx", "zh-hans", @"c:\test\res.zh-hans.resx")]
    public void CombineLanguageExt_ShouldHandleLanguageCorrectly2(
       string input, string languageExt, string expected)
    {
        // Arrange & Act
        var result = PathHelper.CombineLanguageExt2(input, languageExt);

        // Assert - 规范化路径后比较
        Assert.Equal(
            expected.Replace('\\', Path.DirectorySeparatorChar)
                   .Replace('/', Path.DirectorySeparatorChar),
            result.Replace('\\', Path.DirectorySeparatorChar)
                 .Replace('/', Path.DirectorySeparatorChar));
    }


}
