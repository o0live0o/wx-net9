namespace wx.Infrastructure.LLM;
public class LlmProviderFactory
{
    private static readonly Lazy<LlmProviderFactory> _instance = new(() => new LlmProviderFactory());
    private readonly Dictionary<string, ILlmProvider> _providers;
    private LlmConfig _config;

    public static LlmProviderFactory Instance => _instance.Value;
    public LlmProviderFactory()
    {
        _providers = new Dictionary<string, ILlmProvider>();
        _config = new LlmConfig();
    }

    public void Initialize(LlmConfig config)
    {
        _config = config;
    }

    public ILlmProvider GetProvider(string name)
    {
        if (_providers.TryGetValue(name, out var provider))
        {
            return provider;
        }

        provider = name.ToLower() switch
        {
            "spark" => new SparkProvider(_config),
            _ => throw new ArgumentException($"Unsupported provider: {name}")
        };

        _providers[name] = provider;
        return provider;
    }
}