using InventoryManager.ViewModels;
using InventoryManager.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Models.Types;

/// <summary>
/// A basic View Locator to find view for multiple
/// pages for the app.
/// </summary>
public class AppViewLocator : IViewLocator
{
    /// <inheritdoc/>
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null) => viewModel switch
    {
        LoginViewModel context => new LoginView { DataContext = context },
        DashboardViewModel context => new DashboardView { DataContext = context },
        _ => throw new NotImplementedException()
    };
}
