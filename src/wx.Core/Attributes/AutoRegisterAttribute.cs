using Microsoft.Extensions.DependencyInjection;

namespace wx.Core.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class AutoRegisterAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; }

    public bool AsSelf { get; }

    public AutoRegisterAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped,bool asSelf = false)
    {
        Lifetime = lifetime;
        AsSelf = asSelf;
    }

}