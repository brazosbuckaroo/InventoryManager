using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using InventoryManager.ViewModels;
using ReactiveUI;

namespace InventoryManager.Views;

/// <summary>
/// 
/// </summary>
public partial class DashboardView : ReactiveUserControl<DashboardViewModel>
{
    #region CONSTRUCTORS
    /// <summary>
    /// 
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