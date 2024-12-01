using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using InventoryManager.Models.Services;
using InventoryManager.Models.Types;
using InventoryManager.ViewModels;
using InventoryManager.Views;
using System.Diagnostics;

namespace InventoryManager;

public partial class App : Application
{
    #region FIELDS
    /// <summary>
    /// An <see cref="ISettings"/> onject meant to load the saved settings from 
    /// a file and put them in memory.
    /// </summary>
    private readonly ISettings _settingsViewModel = new ApplicationSettingsViewModel();

    /// <summary>
    /// A <see cref="ISecurity"/> object meant to be used and created to
    /// manage the user verfication for the server.
    /// </summary>
    private readonly ISecurity _security = new SecurityManager();
    #endregion

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        this._settingsViewModel.ReadFromFileAsync();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(this._settingsViewModel, this._security)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
