using Avalonia.Controls;
using InventoryManager.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.ViewModels;

/// <summary>
/// A View Model for the <see cref="LoginErrorWindow"/>.
/// </summary>
public class LoginErrorWindowViewModel : ViewModelBase
{
    #region PROPERTIES
    /// <summary>
    /// A simple error message meant to be displayed to the 
    /// user.
    /// </summary>
    public string ErrorMessage { get; } = "There was an error logging in.";
    #endregion

    #region COMMANDS
    /// <summary>
    /// A command just to be used to acknowledge the error.
    /// </summary>
    public ReactiveCommand<Unit, string> AcceptErrorCommand { get; }
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// The default constructor for this class. Has to be used for XAML viewer.
    /// </summary>
    public LoginErrorWindowViewModel()
    {
        // this is meant to do nothing here
        // it is wired in the model itself
        //
        //            :P
        //
        this.AcceptErrorCommand = ReactiveCommand.Create(CloseWindow);
    }
    #endregion

    #region METHODS
    /// <summary>
    /// The method used to return a <see cref="string"/>, for now... also used
    /// to close the window as well.
    /// </summary>
    /// <returns>
    /// Just returns a <see cref="string"/> for the moment.
    /// </returns>
    private string CloseWindow()
    {
        // I wanted to use Unit.Default but 
        // as of right now that is not an option
        //
        // TODO: Maybe make a pull request for avalonia...
        return "bitch";
    }
    #endregion
}
