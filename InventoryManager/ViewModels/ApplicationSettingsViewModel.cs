using InventoryManager.Models.Types;
using InventoryManager.Models.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryManager.ViewModels;

/// <summary>
/// A class that provides actual methods and shit to the
/// <see cref="ApplicationSettings"/> record.
/// </summary>
public class ApplicationSettingsViewModel : ViewModelBase, ISettings 
{
    #region FIELDS
    /// <summary>
    /// A <see cref="ApplicationSettings"/> that is used to track the
    /// applications settings to use.
    /// </summary>
    public ApplicationSettings Settings { get; set; }
    #endregion

    #region FIELDS
    /// <summary>
    /// A <see cref="string"/> meant to store name of the file
    /// where settings are saved.
    /// </summary>
    private string _fileName = "appsettings.json";

    /// <summary>
    /// A <see cref="JsonSerializerOptions"/> used at the initialization of 
    /// this class, to allow for custom options when writing to the file.
    /// </summary>
    private JsonSerializerOptions _jsonOptions;
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// A basic constructor for this class.
    /// </summary>
    public ApplicationSettingsViewModel()
    {
        this.Settings = new ApplicationSettings();
        this._jsonOptions = new JsonSerializerOptions();

        this._jsonOptions.WriteIndented = true;
    }

    /// <summary>
    /// A constructor meant to take parameters to set the
    /// <see cref="ApplicationSettings"/> with a set of given
    /// data.
    /// </summary>
    /// <param name="ipAddress">
    /// A <see cref="string"/> meant to represent the servers 
    /// Ip Address.
    /// </param>
    /// <param name="imagesFilePath">
    /// A <see cref="string"/> meant to represent the folder path
    /// location of where images are held.
    /// </param>
    /// <param name="characterSet">
    /// A short <see cref="string"/> used to represent the Character Set
    /// supported by Firebird SQL.
    /// </param>
    /// <param name="manualsFilePath">
    /// A <see cref="string"/> used to represent the folder path location of where
    /// manuals are stored.
    /// </param>
    /// <param name="databaseSource">
    /// A <see cref="string"/> meant to represent the file path of the 
    /// server's database file.
    /// </param>
    public ApplicationSettingsViewModel(string ipAddress,
                                        string imagesFilePath,
                                        string characterSet,
                                        string manualsFilePath,
                                        string databaseSource)
    {
        this.Settings = new ApplicationSettings(ipAddress,
                                                imagesFilePath,
                                                manualsFilePath,
                                                characterSet,
                                                databaseSource);
        this._jsonOptions = new JsonSerializerOptions();

        this._jsonOptions.WriteIndented = true;
    }
    #endregion

    #region METHODS
    /// <summary>
    /// An <see cref="async"/> method that saves the current <see cref="ApplicationSettings"/>
    /// record to a json file.
    /// </summary>
    /// <param name="cancellation">
    /// A <see cref="CancellationToken"/> to allow for the ability to
    /// cancel an <see cref="async"/> task.
    /// </param>
    /// <returns>
    /// Returns a <see cref="Task"/>.
    /// </returns>
    public async Task SaveToFileAsync(CancellationToken cancellation = default)
    {
        await using FileStream jsonStream = File.Open(Path.Combine(AppContext.BaseDirectory, _fileName),
                                                      FileMode.Create,
                                                      FileAccess.Write);

        await JsonSerializer.SerializeAsync(jsonStream, this.Settings, this._jsonOptions, cancellation);
    }

    /// <summary>
    /// An <see cref="async"/> method to read from the application settings
    /// file to get the prevoisly saved settings.
    /// </summary>
    /// <param name="cancellation">
    /// A <see cref="CancellationToken"/> that will allow the async task
    /// to be canceled.
    /// </param>
    /// <returns>
    /// Returns a <see cref="Task"/>.
    /// </returns>
    /// <remarks>
    /// If the file does not exist, this function will create it and serialize
    /// the <see cref="ApplicationSettings"/> contained in memory.
    /// </remarks>
    public async Task ReadFromFileAsync(CancellationToken cancellation = default)
    {
        if (!File.Exists(Path.Combine(AppContext.BaseDirectory, this._fileName)))
        {
            await using FileStream creatingFile = File.Create(Path.Combine(AppContext.BaseDirectory, this._fileName));
            await JsonSerializer.SerializeAsync(creatingFile, this.Settings, this._jsonOptions, cancellation).ConfigureAwait(false);

            return;
        }

        await using FileStream jsonStream = File.OpenRead(Path.Combine(AppContext.BaseDirectory, this._fileName));
        ApplicationSettings? loadedSettings = await JsonSerializer.DeserializeAsync<ApplicationSettings>(jsonStream, cancellationToken: cancellation).ConfigureAwait(false);

        // if the json file fails to deserialize, this will just 
        // use default settings.
        if (loadedSettings == null)
        {
            this.Settings = new ApplicationSettings();
        }
        else
        {
            this.Settings = loadedSettings;
        }
    }
    #endregion
}
