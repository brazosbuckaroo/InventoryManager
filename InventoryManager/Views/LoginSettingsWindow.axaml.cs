namespace InventoryManager.Views;

/// <summary>
/// A class that is meant to represent the program's settings.
/// </summary>
public partial class LoginSettingsWindow : ReactiveWindow<LoginSettingsWindowViewModel>
{
    #region CONSTRUCTORS
    /// <summary>
    /// The <see cref="LoginSettingsWindow"/> main constructor that register's methods
    /// to allow file and folder selection.
    /// </summary>
    public LoginSettingsWindow()
    {
        // Prevents the window from being closed outside of
        // the command.
        this.Closing += (s, e) =>
        {
            if (!e.IsProgrammatic)
            {
                e.Cancel = true;
            };
        };

        this.WhenActivated(disposables =>
        {
            disposables(ViewModel!.FileExplorerDialog.RegisterHandler(this.OpenFileExplorerAsync));
            disposables(ViewModel!.FolderExplorerDialog.RegisterHandler(this.OpenFolderExplorerAsync));
            disposables(ViewModel!.AcceptSettingsCommand.Subscribe(this.Close));
            disposables(ViewModel!.CancelCommand.Subscribe(this.Close));
        });

        AvaloniaXamlLoader.Load(this);
    }
    #endregion

    #region METHODS
    /// <summary>
    /// A method that gets the <see cref="TopLevel"/> for the <see cref="LoginSettingsWindow"/>
    /// class to allow access to the OS's file explorer.
    /// </summary>
    /// <param name="context">
    /// A <see cref="IInteractionContext{string, string}"/> that takes a string as an input
    /// to name the <see cref="StorageProvider"/> winow and return the selected file's 
    /// filepath as a <see cref="string"/>.
    /// </param>
    /// <returns>
    /// Returns the selected file's filepath as a <see cref="string"/>.
    /// </returns>
    private async Task OpenFileExplorerAsync(IInteractionContext<string, string> context)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var file = await topLevel!.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions()
            {
                AllowMultiple = false,
                Title = context.Input
            }
        ).ConfigureAwait(false);

        // if the user didnt select anything, give an empty
        // string
        context.SetOutput((file.Count == 0) ? string.Empty : file.SingleOrDefault()!.TryGetLocalPath()!);
    }

    /// <summary>
    /// A method that gets the <see cref="TopLevel"/> for the <see cref="LoginSettingsWindow"/>
    /// class to allow access to the OS's file explorer.
    /// </summary>
    /// <param name="context">
    /// A <see cref="IInteractionContext{string, string}"/> that takes a string as an input
    /// to name the <see cref="StorageProvider"/> winow and return the selected folder's 
    /// filepath as a <see cref="string"/>.
    /// </param>
    /// <returns>
    /// Returns the selected folder's filepath as a <see cref="string"/>.
    /// </returns>
    private async Task OpenFolderExplorerAsync(IInteractionContext<string, string> context)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var folder = await topLevel!.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions()
            {
                AllowMultiple = false,
                Title = context.Input
            }
        ).ConfigureAwait(false);

        context.SetOutput((folder.Count == 0) ? string.Empty : folder.SingleOrDefault()!.TryGetLocalPath()!);
    }
    #endregion
}