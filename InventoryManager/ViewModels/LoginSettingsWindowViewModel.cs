namespace InventoryManager.ViewModels;

/// <summary>
/// A class that hold the business logic for the <see cref="LoginSettingsWindow"/>.
/// </summary>
public class LoginSettingsWindowViewModel : ViewModelBase
{
    #region PROPERTIES
    /// <summary>
    /// A <see cref="ObservableCollection{string}"/> that is meant to hold the 
    /// Charactersets supported by Firebird SQL.
    /// </summary>
    public ObservableCollection<string> CharacterSets { get; }

    /// <summary>
    /// A <see cref="Interaction{string, string}"/> that is used to select a file 
    /// and returns the file path as a string.
    /// </summary>
    public Interaction<string, string> FileExplorerDialog { get; }

    /// <summary>
    /// A <see cref="Interaction{string, string}"/> that is used to select a folder
    /// and returns the folder path as a string.
    /// </summary>
    public Interaction<string, string> FolderExplorerDialog { get; }

    /// <summary>
    /// A <see cref="string"/> meant to represent an IP address.
    /// </summary>
    public string IpAddress
    { 
        get => _ipAddress;
        set => this.RaiseAndSetIfChanged(ref _ipAddress, value);
    }

    /// <summary>
    /// A <see cref="string"/> meant to represent the databases' file or
    /// datasource.
    /// </summary>
    public string DatabaseFilePath 
    {
        get => _databaseFilePath;
        set => this.RaiseAndSetIfChanged(ref _databaseFilePath, value);
    }

    /// <summary>
    /// A <see cref="string"/> meant to represent the folder path to save image files 
    /// at.
    /// </summary>
    public string ImagesFolderPath
    {
        get => _imagesFolderPath;
        set => this.RaiseAndSetIfChanged(ref _imagesFolderPath, value);
    }

    /// <summary>
    /// A <see cref="string"/> meant to represent the folder path to save items' manuals in the
    /// database.
    /// </summary>
    public string ManualFolderPath
    {
        get => _manualFolderPath;
        set => this.RaiseAndSetIfChanged(ref _manualFolderPath, value);
    }

    /// <summary>
    /// A <see cref="string"/> meant to represent the selected charactersets for FireBirdSQL.
    /// </summary>
    public string SelectedCharacterSet
    {
        get => _selectedCharacterSet;
        set => this.RaiseAndSetIfChanged(ref _selectedCharacterSet, value);
    }
    #endregion

    #region FIELDS
    /// <summary>
    /// A <see cref="string"/> meant to represent an IP Address of the server.
    /// </summary>
    private string _ipAddress;

    /// <summary>
    /// A <see cref="string"/> meant to represent the selected characterset.
    /// </summary>
    private string _selectedCharacterSet;

    /// <summary>
    /// A <see cref="string"/> meant to represent the file path to the database
    /// file.
    /// </summary>
    private string _databaseFilePath;

    /// <summary>
    /// A <see cref="string"/> meant to represent the folder path to the 
    /// storage location of images folder location.
    /// </summary>
    private string _imagesFolderPath;

    /// <summary>
    /// A <see cref="string"/> mwant to represent the folder path for the location 
    /// of manuals folders location.
    /// </summary>
    private string _manualFolderPath;

    /// <summary>
    /// A <see cref="ApplicationSettingsViewModel"/> that encompasses a 
    /// <see cref="ApplicationSettings"/> record to allow for functionality.
    /// </summary>
    private ApplicationSettingsViewModel _settingsViewModel;
    #endregion

    #region COMMANDS
    /// <summary>
    /// A <see cref="ReactiveCommand{Unit, ApplicationSettingsViewModel}"/> used to cancel the settings editing
    /// operation.
    /// </summary>
    public ReactiveCommand<Unit, ApplicationSettingsViewModel> CancelCommand { get; }

    /// <summary>
    /// A <see cref="ReactiveCommand{Unit, ApplicationSettingsViewModel}"/> used to save the updated setting to the setting
    /// JSON file.
    /// </summary>
    public ReactiveCommand<Unit, ApplicationSettingsViewModel> AcceptSettingsCommand { get; }

    /// <summary>
    /// A <see cref="ReactiveCommand{Unit, Unit}"/> used to open a system file dialog.
    /// </summary>
    public ReactiveCommand<Unit, Unit> SelectFileDialogCommand { get; }

    /// <summary>
    /// A <see cref="ReactiveCommand{bool, Unit}"/> used to open a system folder dialog.
    /// </summary>
    public ReactiveCommand<bool, Unit> SelectFolderDialogCommand { get; }
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// A constructor that creates a <see cref="LoginSettingsWindowViewModel"/> with an empty 
    /// Character Set.
    /// </summary>
    public LoginSettingsWindowViewModel()
    {
        this.CharacterSets = new ObservableCollection<string>();
        this.FileExplorerDialog = new Interaction<string, string>();
        this.FolderExplorerDialog = new Interaction<string, string>();
        this._settingsViewModel = new ApplicationSettingsViewModel();
        this.SelectFileDialogCommand = ReactiveCommand.CreateFromTask(SelectFileDialogAsync);
        this.SelectFolderDialogCommand = ReactiveCommand.CreateFromTask<bool>(SelectFolderDialogAsync);
        this.CancelCommand = ReactiveCommand.Create(CancelSettingsEditing);
        this.AcceptSettingsCommand = ReactiveCommand.CreateFromTask(SaveApplicationLoginSettings);
        this._databaseFilePath = string.Empty;
        this._imagesFolderPath = string.Empty;
        this._manualFolderPath = string.Empty;
        this._selectedCharacterSet = string.Empty;
        this._ipAddress = string.Empty;
    }
    #endregion

    #region METHODS
    /// <summary>
    /// This method gets the Character Sets supported by Firebird SQL from 
    /// a resource file.
    /// </summary>
    /// <returns>
    /// Returns an <see cref="async>"/> <see cref="Task"/>.
    /// </returns>
    public async Task GetCharacterSetsAsync()
    {
        await Task.Run(() =>
        {
            foreach (var word in Resource.FirebirdSQLCharSets.Split("\n"))
            {
                CharacterSets.Add(word.Trim());
            }
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// A method that loads the currently saved settings to the ui.
    /// </summary>
    /// <returns>
    /// Returns a <see cref="Task"/> to perform an async task.
    /// </returns>
    public async Task ReadSettingsFromFileAsync()
    {
        await this._settingsViewModel.ReadFromFileAsync().ConfigureAwait(false);

        this.IpAddress = this._settingsViewModel.Settings.IpAddress;
        this.DatabaseFilePath = this._settingsViewModel.Settings.DatabaseSource;
        this.ManualFolderPath = this._settingsViewModel.Settings.ManualsFilePath;
        this.ImagesFolderPath = this._settingsViewModel.Settings.ImagesFilePath;
        this.SelectedCharacterSet = this._settingsViewModel.Settings.CharacterSet;
    }

    /// <summary>
    /// A method that is used as a <see cref="ICommand"/> to allow users to
    /// open a system file picker through a <see cref="Interaction{string, string}"/>
    /// </summary>
    /// <returns>
    /// Returns a <see cref="async"/> <see cref="Task"/> and sets <see cref="LoginSettingsWindowViewModel.DatabaseFilePath"/>
    /// to the <see cref="string"/> returned by the <see cref="Interaction{string, string}"/>
    /// </returns>
    private async Task SelectFileDialogAsync()
    {
        var filePath = await FileExplorerDialog.Handle("Select a Database File");
        this.DatabaseFilePath = string.IsNullOrEmpty(filePath) ? _databaseFilePath : filePath;
    }

    /// <summary>
    /// A method that is used as a <see cref="ICommand"/> to allow users to
    /// open a system file picker through a <see cref="Interaction{string, string}"/>
    /// </summary>
    /// <param name="isManuals"></param>
    /// <returns>
    /// Returns a <see cref="async"/> <see cref="Task"/> and sets <see cref="LoginSettingsWindowViewModel.ManualFolderPath"/>
    /// or <see cref="LoginSettingsWindowViewModel.ImagesFolderPath"/> to the <see cref="string"/> 
    /// returned by the <see cref="Interaction{string, string}"/>
    /// </returns>
    private async Task SelectFolderDialogAsync(bool isManuals = false)
    {
        var folderPath = await FolderExplorerDialog.Handle("Select a Folder");

        // Decide which field or property to edit
        if (isManuals)
        {
            this.ManualFolderPath = string.IsNullOrEmpty(folderPath) ? _manualFolderPath : folderPath;
        }
        else
        {
            this.ImagesFolderPath = string.IsNullOrEmpty(folderPath) ? _imagesFolderPath : folderPath;
        }
    }

    /// <summary>
    /// A method to be use by the <see cref="LoginSettingsWindowViewModel.CancelCommand"/> to ignore any
    /// changes and close the window.
    /// </summary>
    /// <Returns>
    /// Returns a <see cref="ApplicationSettingsViewModel"/> that was loaded from the save file.
    /// </Returns>
    private ApplicationSettingsViewModel CancelSettingsEditing()
    {
        return this._settingsViewModel;
    }

    /// <summary>
    /// A method to be used by the <see cref="LoginSettingsWindowViewModel.AcceptSettingsCommand"/> to 
    /// write any changed settings to the file.
    /// </summary>
    /// <param name="cancellation">
    /// A <see cref="CancellationToken"/> that allows this async task to be cancelled.
    /// </param>
    /// <returns>
    /// A <see cref="Task{ApplicationSettingsViewModel}"/> and saves the applications settings into the settings file.
    /// </returns>
    private async Task<ApplicationSettingsViewModel> SaveApplicationLoginSettings(CancellationToken cancellation = default)
    {
        this._settingsViewModel.Settings = new ApplicationSettings(this.IpAddress.Trim(),
                                                                   this.ImagesFolderPath.Trim(), 
                                                                   this.ManualFolderPath.Trim(), 
                                                                   this.SelectedCharacterSet.Trim(), 
                                                                   this.DatabaseFilePath.Trim());
        
        await this._settingsViewModel.SaveToFileAsync(cancellation).ConfigureAwait(false);

        return this._settingsViewModel;
    }
    #endregion
}
