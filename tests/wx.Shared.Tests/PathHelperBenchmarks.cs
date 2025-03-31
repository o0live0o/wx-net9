using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using wx.Shared.Tools;

namespace wx.Shared.Tests.Tools;

[MemoryDiagnoser]
public class PathHelperBenchmarks
{
    private static readonly string[] TestPaths = new[]
    {
        @"xxx/wwa/www.resx",
        @"www.resx",
        @"c:\test\res.resx",
        @"d:\folder\subfolder\test.resx"
    };

    private const string LanguageExt = "zh-hans";

    [Benchmark(Baseline = true)]
    internal void PathCombine_Original()
    {
        foreach (var path in TestPaths)
        {
            _ = PathHelper.CombineLanguageExt(path, LanguageExt);
        }
    }

    [Benchmark]
    internal void PathCombine_StringBuilder()
    {
        foreach (var path in TestPaths)
        {
            _ = PathHelper.CombineLanguageExt2(path, LanguageExt);
        }
    }

    // [Fact]
    // public void RunBenchmarks()
    // {

    //     var config = ManualConfig.Create(DefaultConfig.Instance)
    //         .WithOptions(ConfigOptions.DisableOptimizationsValidator)
    //         .WithOptions(ConfigOptions.JoinSummary)  // 添加这行合并报告
    //         .WithOptions(ConfigOptions.StopOnFirstError);

    //     BenchmarkRunner.Run<PathHelperBenchmarks>(config);
    //     // BenchmarkRunner.Run<PathHelperBenchmarks>();
    // }
}