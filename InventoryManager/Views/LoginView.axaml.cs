using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using InventoryManager.ViewModels;
using ReactiveUI;

namespace InventoryManager.Views;

/// <summary>
/// The initial view a user will interact with when they login into
/// the server.
/// </summary>
public partial class LoginView : ReactiveUserControl<LoginViewModel>
{
    /// <summary>
    /// The default constructor for this view. Allowing any special wiring to occur 
    /// the XAML previewer to see this view.
    /// </summary>
    public LoginView()
    {
        this.WhenActivated(disposables => 
        {
        });

        AvaloniaXamlLoader.Load(this);
    }
}