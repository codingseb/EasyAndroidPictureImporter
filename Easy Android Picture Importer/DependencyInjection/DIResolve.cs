using Autofac;
using System.Windows.Markup;

namespace EasyAndroidPictureImporter.DependencyInjection;

/// <summary>
/// A markup extension to resolve a type with DI
/// </summary>
public class DIResolve : MarkupExtension
{
    /// <summary>
    /// The Type to resolve
    /// </summary>
    public Type ResolveType { get; set; }

    /// <summary>
    /// default constructor
    /// </summary>
    public DIResolve()
    { }

    /// <summary>
    /// Constructor for direct type specification
    /// </summary>
    /// <param name="resolveType">The Type to resolve</param>
    public DIResolve(Type resolveType)
    {
        ResolveType=resolveType;
    }

    /// <inheritdoc/>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return DI.Container.Resolve(ResolveType);
    }
}