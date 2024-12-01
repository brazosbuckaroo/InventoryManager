using ReactiveUI;

namespace InventoryManager.ViewModels;

public class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    /// <inheritdoc/>
    public ViewModelActivator Activator { get; } = new ViewModelActivator();
}
