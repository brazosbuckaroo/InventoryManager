namespace InventoryManager.ViewModels;

/// <summary>
/// The base class for ViewModels.
/// </summary>
public class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    /// <inheritdoc/>
    public ViewModelActivator Activator { get; } = new ViewModelActivator();
}
