namespace InventoryManager.Views;

/// <summary>
/// The class to be used for the Dashboard view after logining in.
/// </summary>
public partial class DashboardView : ReactiveUserControl<DashboardViewModel>
{
    #region CONSTRUCTORS
    /// <summary>
    /// The primary constructor for <see cref="DashboardView"/>.
    /// </summary>
    public DashboardView()
    {
        this.WhenActivated(disposables => 
        { 
        });

        AvaloniaXamlLoader.Load(this);
    }
    #endregion
}