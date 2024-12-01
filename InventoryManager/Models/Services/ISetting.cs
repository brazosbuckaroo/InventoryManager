namespace InventoryManager.Models.Services;

/// <summary>
/// An empty marker interface for anything related to 
/// the application settings... may not be empty for long.
/// </summary>
public interface ISettings
{
    #region FIELDS
    /// <summary>
    /// A <see cref="ApplicationSettings"/> that is used to track the
    /// applications settings to use.
    /// </summary>
    ApplicationSettings Settings { get; set; }
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
    Task SaveToFileAsync(CancellationToken cancellation = default);

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
    Task ReadFromFileAsync(CancellationToken cancellation = default);
    #endregion
}
