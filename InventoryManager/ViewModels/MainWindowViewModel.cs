namespace InventoryManager.ViewModels;

/// <summary>
/// A class meant to contain all the ViewModels that the <see cref="MainWindow"/>
/// is meant to know about.
/// </summary>
public class MainWindowViewModel : WindowViewModelBase
{
    #region PROPERTIES
    /// <summary>
    /// The ViewModel meant for the <see cref="LoginView"/>.
    /// </summary>
    public LoginViewModel LoginViewModel { get; }

    /// <summary>
    /// The ViewModel meant for the <see cref="DashboardView"/>.
    /// </summary>
    public DashboardViewModel DashboardViewModel { get; }
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// The default constructor that makes the <see cref="MainWindowViewModel"/> with 
    /// the ViewModels it needs to know about.
    /// </summary>
    public MainWindowViewModel()
    {
        this.LoginViewModel = new LoginViewModel();
        this.DashboardViewModel = new DashboardViewModel();

        Router.Navigate.Execute(LoginViewModel);
    }

    /// <summary>
    /// A constructor for <see cref="MainWindowViewModel"/> that allows for
    /// dependency injection.
    /// </summary>
    /// <param name="settingsProvider">
    /// The service meant for loading, reading, and editing application settings
    /// </param>
    /// <param name="securityProvider">
    /// The services meant to provide the ability to validate users and confirm 
    /// admin priveleges
    /// </param>
    public MainWindowViewModel(ISettings settingsProvider, ISecurity securityProvider)
    {
        this.DashboardViewModel = new DashboardViewModel(this);
        this.LoginViewModel = new LoginViewModel(settingsProvider, this, securityProvider, this.DashboardViewModel);

        Router.Navigate.Execute(LoginViewModel);
    }
    #endregion
}
