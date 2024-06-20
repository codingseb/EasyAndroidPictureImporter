using Autofac.Core.Resolving.Pipeline;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace EasyAndroidPictureImporter.DependencyInjection.Middlewares;

/// <summary>
/// Add persistence by DI to a type that implement <see cref="INotifyPropertyChanged"/><para/>
/// Populate properties by deserialization at <see cref="PipelinePhase.Activation"/> if the specified file exists<para/>
/// Serialize the object on see event <see cref="INotifyPropertyChanged.PropertyChanged"/> when the property do not have the attribute <see cref="JsonIgnoreAttribute"/>
/// </summary>
/// <param name="fileName">To specify the file path where to persist the object, if not specify use <c>%APPDATA%\{AssemblyName}\{TypeOfObject}.json</c></param>
public class MakeOnPropertyChangedPersistentMiddleware(string fileName = null) : IResolveMiddleware
{
    private readonly Dictionary<string, bool> hasJsonIgnoreAttributeCache = [];

    /// <summary>
    /// The file path where is persisted the object
    /// </summary>
    public string FileName { get; private set; } = fileName;

    /// <inheritdoc/>
    public PipelinePhase Phase => PipelinePhase.Activation;

    /// <inheritdoc/>
    public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
    {
        next(context);
        if (context.Instance is INotifyPropertyChanged obj)
        {
            string roamingDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assembly.GetExecutingAssembly().GetName().Name);
            FileName ??= Path.Combine(roamingDir, obj.GetType().Name + ".json");

            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);
                JsonConvert.PopulateObject(json, obj);
            }

            obj.PropertyChanged += Obj_PropertyChanged;
        }
    }

    private void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (!hasJsonIgnoreAttributeCache.TryGetValue(e.PropertyName, out bool hasJsonIgnoreAttribute))
        {
            hasJsonIgnoreAttribute=sender.GetType().GetProperty(e.PropertyName).GetCustomAttributes(true).OfType<JsonIgnoreAttribute>().Any();
            hasJsonIgnoreAttributeCache[e.PropertyName] = hasJsonIgnoreAttribute;
        }

        if (!hasJsonIgnoreAttribute)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FileName)!);

            File.WriteAllText(FileName, JsonConvert.SerializeObject(sender, Formatting.Indented));
        }
    }
}