using System.Windows.Input;

namespace EasyAndroidPictureImporter.Helpers.Commands;

/// <summary>
/// A class for the base of a command implementation
/// </summary>
public abstract class CommandBase : ICommand
{
    /// <inheritdoc/>
    public event EventHandler CanExecuteChanged;

    /// <inheritdoc/>
    public virtual bool CanExecute(object parameter) => true;

    /// <inheritdoc/>
    public abstract void Execute(object parameter);

    /// <summary>
    /// Raise the event CanExecuteChanged to update the command state
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        OnCanExecuteChanged();
    }

    protected virtual void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}