namespace InventoryManager.Views;

/// <summary>
/// An object that represents the programs main window as a <see cref="ReactiveWindow{MainWindowViewModel}"/>
/// </summary>
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    #region CONSTRUCTORS
    /// <summary>
    /// A constructor that makes the main window.
    /// </summary>
    public MainWindow()
    {
        this.WhenActivated(disposables =>
        {
            disposables(ViewModel!.LoginViewModel.LoginSettingsInteraction.RegisterHandler(this.DoShowLoginSettingsDialogAsync));
            disposables(ViewModel!.LoginViewModel.LoginErrorInteraction.RegisterHandler(this.DoShowLoginErrorDialogAsync));
        });

        AvaloniaXamlLoader.Load(this);
    }
    #endregion

    #region METHODS
    /// <summary>
    /// Takes an <see cref="IInteractionContext{LoginSettingsWindowViewModel, ApplicationSettingsViewModel}"/> to open a new 
    /// window as a dialog for login settings.
    /// </summary>
    /// <param name="context">
    /// A <see cref="IInteractionContext{LoginSettingsWindowViewModel, ApplicationSettingsViewModel}"/> that takes a 
    /// <see cref="LoginSettingsWindowViewModel"/> and a <see cref="ApplicationSettingsViewModel"/> to allow for setting changes
    /// </param>
    /// <returns>
    /// Returns a Task to allow for <see cref="async"/> operations.
    /// </returns>
    private async Task DoShowLoginSettingsDialogAsync(IInteractionContext<LoginSettingsWindowViewModel, ApplicationSettingsViewModel> context)
    {
        var dialog = new LoginSettingsWindow();
        dialog.DataContext = context.Input;

        context.SetOutput(await dialog.ShowDialog<ApplicationSettingsViewModel>(this).ConfigureAwait(false));
    }

    /// <summary>
    /// The method used to handle opening the login error window.
    /// </summary>
    /// <param name="context">
    /// Take a <see cref="IInteractionContext{LoginErrorWindowViewModel, string}"/> to help handle the 
    /// <see cref="Interaction{LoginErrorWindowViewModel, string}"/>
    /// </param>
    /// <returns>
    /// Returns a <see cref="string"/> but this will only be until <see cref="Unit"/> can be returned
    /// </returns>
    private async Task DoShowLoginErrorDialogAsync(IInteractionContext<LoginErrorWindowViewModel, string> context)
    {
        var dialog = new LoginErrorWindow();
        dialog.DataContext = context.Input;

        context.SetOutput(await dialog.ShowDialog<string>(this).ConfigureAwait(false));
    }
    #endregion
}
