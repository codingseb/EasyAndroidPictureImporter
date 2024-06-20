using Autofac.Core.Resolving.Pipeline;

namespace EasyAndroidPictureImporter.DependencyInjection.Middlewares;

/// <summary>
/// Call the method Init of a class that implement <see cref="IInitializable"/> when an instance has been activate by the DI
/// </summary>
public class OnActivationCallInitMiddleware : IResolveMiddleware
{
    /// <inheritdoc/>
    public PipelinePhase Phase => PipelinePhase.Activation;

    /// <inheritdoc/>
    public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
    {
        next(context);
        if (context.Instance is IInitializable initializable)
        {
            initializable.Init();
        }
    }
}