using Autofac.Core.Resolving.Pipeline;

namespace EasyAndroidPictureImporter.DependencyInjection.Middlewares;

/// <summary>
/// A configurable middleware to specify the phase and action to execute at DI Build time
/// </summary>
/// <param name="phase">The pipeline phase at which to execute the middleware</param>
/// <param name="executeAction">The action to execute</param>
public class ConfigurableResolveMiddleware(PipelinePhase phase, Action<ResolveRequestContext> executeAction) : IResolveMiddleware
{
    /// <summary>
    /// The action that will be execute when middle ware is executed
    /// </summary>
    public Action<ResolveRequestContext> ExecuteAction { get; } = executeAction;

    /// <inheritdoc/>
    public PipelinePhase Phase { get; } = phase;

    /// <inheritdoc/>
    public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
    {
        next(context);
        ExecuteAction?.Invoke(context);
    }
}