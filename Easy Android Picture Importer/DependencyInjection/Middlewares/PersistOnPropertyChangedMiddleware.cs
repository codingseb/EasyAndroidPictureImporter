using Autofac.Core.Resolving.Pipeline;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
/// <param name="doAfterObjectPopulated">if specified the action is call just after object has been populate with the last save at init</c></param>
public class PersistOnPropertyChangedMiddleware(string fileName = null, Action<object> doAfterObjectPopulated = null)
    : IResolveMiddleware
{
    private static readonly JsonSerializerSettings jsonSerializerSettings = new()
    {
        ObjectCreationHandling = ObjectCreationHandling.Replace,
        Converters = [new StringEnumConverter()],
        Formatting = Formatting.Indented,
    };

    private readonly Dictionary<string, bool> hasJsonIgnoreAttributeCache = [];

    private readonly Action<object> _doAfterObjectPopulated = doAfterObjectPopulated;

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
            if (FileName == null)
            {
                string roamingDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assembly.GetExecutingAssembly().GetName().Name);
                FileName = Path.Combine(roamingDir, obj.GetType().Name + ".json");
            }

            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);
                JsonConvert.PopulateObject(json, obj, jsonSerializerSettings);
            }

            _doAfterObjectPopulated?.Invoke(obj);

            obj.PropertyChanged += Obj_PropertyChanged;
        }
    }

    private void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        bool hasJsonIgnoreAttribute = false;

        if (e.PropertyName != null && !hasJsonIgnoreAttributeCache.TryGetValue(e.PropertyName, out hasJsonIgnoreAttribute))
        {
            hasJsonIgnoreAttribute=sender.GetType().GetProperty(e.PropertyName)?.GetCustomAttributes(true).OfType<JsonIgnoreAttribute>().Any() == true;
            hasJsonIgnoreAttributeCache[e.PropertyName] = hasJsonIgnoreAttribute;
        }

        if (!hasJsonIgnoreAttribute)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FileName)!);

            File.WriteAllText(FileName, JsonConvert.SerializeObject(sender, jsonSerializerSettings));
        }
    }
}