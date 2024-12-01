using InventoryManager.Models.Services;
using InventoryManager.Models.Types;
using InventoryManager.Views;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FirebirdSql.Data.FirebirdClient;

namespace InventoryManager.ViewModels;

/// <summary>
/// A <see cref="ViewModelBase"/> made for the login screen of the
/// application.
/// </summary>
public class LoginViewModel : ViewModelBase, IRoutableViewModel
{
    #region PROPERTIES
    /// <summary>
    /// A <see cref="Interaction{LoginSettingsWindowViewModel, ApplicationSettingsViewModel}"/> meant to handle 
    /// the creation of a <see cref="LoginSettingsWindowViewModel"/> to allow for a dialog window to pop
    /// up.
    /// </summary>
    public Interaction<LoginSettingsWindowViewModel, ApplicationSettingsViewModel> LoginSettingsInteraction { get; }

    /// <summary>
    /// A <see cref="Interaction{LoginErrorWindowViewModel, string}"/> meant to be used to open
    /// the error dialog window whenever there is a problem during loginning in.
    /// 
    /// For the time beinng this will return a string but the ideal thing is to 
    /// make it just return a <see cref="Unit"/>.
    /// </summary>
    public Interaction<LoginErrorWindowViewModel, string> LoginErrorInteraction { get; }

    /// <inheritdoc/>
    public IScreen HostScreen { get; }

    /// <summary>
    /// A <see cref="ISettings"/> used to get application settings and whathave you.
    /// </summary>
    public ISettings SettingsService { get; private set; }

    /// <inheritdoc/>
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    /// <summary>
    /// A <see cref="DashboardViewModel"/> used to load the 
    /// application's main dashboard.
    /// </summary>
    public DashboardViewModel DashboardView { get; private set; }

    /// <summary>
    /// The Applications security manager to provide
    /// a way to verfiy login and admin status.
    /// </summary>
    public ISecurity SecurityManager { get; }

    /// <summary>
    /// The connection string meant to be used to connect to
    /// the Firebird SQL Server.
    /// </summary>
    public FbConnectionStringBuilder ConnectionString { get; private set; }

    /// <summary>
    /// The observable property to be used to allow
    /// users to change it via the UI.
    /// </summary>
    public string Username
    {
        get => this._username;
        set => this.RaiseAndSetIfChanged(ref _username, value);
    }

    /// <summary>
    /// The observable property to be used to allow
    /// users to change it via the UI.
    /// </summary>
    public string Password
    {
        get => this._password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }
    #endregion

    #region FIELDS
    /// <summary>
    /// The backing fields for the <see cref="LoginViewModel.Username"/>
    /// observable property.
    /// </summary>
    private string _username;

    /// <summary>
    /// The backing fields for the <see cref="LoginViewModel.Password"/>
    /// observable property.
    /// </summary>
    private string _password;
    #endregion

    #region COMMANDS
    /// <summary>
    /// A <see cref="ReactiveCommand"/> meant to be used in the UI to open settings.
    /// </summary>
    public ReactiveCommand<Unit, Unit> OpenLoginSettingsCommand { get; }

    /// <summary>
    /// A <see cref="ReactiveCommand"/> meant to run the verifcation
    /// service and load the dashboard if verifcation is successful.
    /// </summary>
    public ReactiveCommand<Unit, IRoutableViewModel> LoginCommand { get; }
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// A constructor that that makes the ViewModel's <see cref="Interaction{LoginSettingsWindowViewModel, ApplicationSettingsViewModel}"/>
    /// and creates the <see cref="ReactiveCommand"/>.
    /// </summary>
    public LoginViewModel()
    {
        this.LoginSettingsInteraction = new Interaction<LoginSettingsWindowViewModel, ApplicationSettingsViewModel>();
        this.LoginErrorInteraction = new Interaction<LoginErrorWindowViewModel, string>();
        this.OpenLoginSettingsCommand = ReactiveCommand.CreateFromTask(OpenLoginSettingsDialogAsync);
        this.SettingsService = new ApplicationSettingsViewModel();
        this.SecurityManager = new SecurityManager();
        this.DashboardView = new DashboardViewModel();
        this.ConnectionString = new FbConnectionStringBuilder();
        this._password = string.Empty;
        this._username = string.Empty;
        this.LoginCommand = ReactiveCommand.CreateFromTask<IRoutableViewModel>(Login);
    }

    /// <summary>
    /// A <see cref="LoginViewModel"/> constructor that allows for injection of a settings
    /// provider.
    /// </summary>
    /// <param name="settingsProvider">
    /// An <see cref="ISettings"/> meant to allow the ability to edit, save, and read settings.
    /// </param>
    /// <param name="screen">
    /// The <see cref="MainWindow"/> that is hosting the view to allow for navigate to the
    /// next view.
    /// </param>
    /// <param name="securityProvider">
    /// The security service meant to be used to allow the application to 
    /// verfiy users and admin priveleges.
    /// </param>
    /// <param name="dashboardViewModel">
    /// The <see cref="DashboardViewModel"/> passed from the <see cref="MainWindow"/>.
    /// </param>
    public LoginViewModel(ISettings settingsProvider,
                          IScreen screen,
                          ISecurity securityProvider,
                          DashboardViewModel dashboardViewModel)
    {
        this.LoginSettingsInteraction = new Interaction<LoginSettingsWindowViewModel, ApplicationSettingsViewModel>();
        this.LoginErrorInteraction = new Interaction<LoginErrorWindowViewModel, string>();
        this.DashboardView = dashboardViewModel;
        this.SettingsService = settingsProvider;
        this.HostScreen = screen;
        this.SecurityManager = securityProvider;
        this.ConnectionString = new FbConnectionStringBuilder();
        this._username = string.Empty;
        this._password = string.Empty;

        //making the connection string
        this.ConnectionString.UserID = this._username;
        this.ConnectionString.Password = this.Password;
        this.ConnectionString.Charset = this.SettingsService.Settings.CharacterSet;
        this.ConnectionString.DataSource = this.SettingsService.Settings.IpAddress;
        this.ConnectionString.Database = this.SettingsService.Settings.DatabaseSource;

        this.LoginCommand = ReactiveCommand.CreateFromTask<IRoutableViewModel>(Login);
        this.OpenLoginSettingsCommand = ReactiveCommand.CreateFromTask(OpenLoginSettingsDialogAsync);
    }
    #endregion

    #region METHODS
    /// <summary>
    /// A method used to make an <see cref="async"/> <see cref="ICommand"/> that makes <see cref="LoginSettingsWindowViewModel"/> 
    /// to handle the <see cref="Interaction{LoginSettingsWindowViewModel, ApplicationSettingsViewModel}"/> to allow 
    /// for the creation of the dialog window.
    /// </summary>
    /// <returns>
    /// Returns a <see cref="Task"/> to allow for <see cref="async"/> compute.
    /// </returns>
    private async Task OpenLoginSettingsDialogAsync()
    {
        var dialog = new LoginSettingsWindowViewModel();

        await dialog.GetCharacterSetsAsync();
        await dialog.ReadSettingsFromFileAsync();

        this.SettingsService = await LoginSettingsInteraction.Handle(dialog);
        this.ConnectionString = new FbConnectionStringBuilder();
        this.ConnectionString.Charset = this.SettingsService.Settings.CharacterSet;
        this.ConnectionString.DataSource = this.SettingsService.Settings.IpAddress;
        this.ConnectionString.Database = this.SettingsService.Settings.DatabaseSource;
    }

    /// <summary>
    /// The method that will verify a user's credentials and load the next view
    /// if they are verified.
    /// </summary>
    /// <returns>
    /// A <see cref="IObservable{DashboardViewModel}"/> meant to be the
    /// applications main dashboard.
    /// </returns>
    private async Task<IRoutableViewModel> Login()
    {
        this.ConnectionString.UserID = this.Username;
        this.ConnectionString.Password = this.Password;

        if (!await SecurityManager.VerifyCredentialsAsync(this.ConnectionString))
        {
            var dialog = new LoginErrorWindowViewModel();

            // this is technically a string but we do not care about
            // it. this is just a work around.
            await this.LoginErrorInteraction.Handle(dialog);

            return await this.HostScreen.Router.Navigate.Execute(this);
        }

        this.DashboardView.ConnectionString = this.ConnectionString;

        return await this.HostScreen.Router.Navigate.Execute(this.DashboardView);
    }
    #endregion
}