namespace InventoryManager.ViewModels;

/// <summary>
/// The base class for any Routable ViewModels.... allow for
/// navigation.
/// </summary>
public class RoutableViewModelBase : ReactiveObject, IActivatableViewModel, IRoutableViewModel
{
    #region PROPERTIES
    /// <inheritdoc/>
    public IScreen HostScreen { get; protected set; }

    /// <inheritdoc/>
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    /// <inheritdoc/>
    public ViewModelActivator Activator { get; } = new ViewModelActivator();
    #endregion
}
