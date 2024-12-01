namespace InventoryManager.ViewModels;

/// <summary>
/// The base class for any Window ViewModel that
/// needs to be able to route to other Views.
/// </summary>
public class WindowViewModelBase : ReactiveObject, IScreen
{
    #region PROPERTIES
    /// <inheritdoc/>
    public RoutingState Router { get; } = new RoutingState();
    #endregion
}
