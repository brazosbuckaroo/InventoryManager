using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Models.Types;

/// <summary>
/// A record used to store application settings.
/// </summary>
public record ApplicationSettings
{
    #region PROPERTIES
    /// <summary>
    /// A <see cref="string"/> used to store the connection string ip
    /// information for the Firebird SQL server.
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// The name or file location of the SQL database file.
    /// </summary>
    public string DatabaseSource { get; set; }

    /// <summary>
    /// The selected Character Set to use when opening the connection.
    /// </summary>
    public string CharacterSet { get; set; }

    /// <summary>
    /// A <see cref="string"/> used to represent the file path where images should be stored 
    /// for the application.
    /// </summary>
    public string ImagesFilePath { get; set; }

    /// <summary>
    /// A <see cref="string"/> used to represent the file path where item manuals should
    /// be stored for the application.
    /// </summary>
    public string ManualsFilePath { get; set; }
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// A default constructor where the <see cref="ApplicationSettings.ConnectionString"/> is empty and
    /// there are default values set to <see cref="ApplicationSettings.ImagesFilePath"/> and 
    /// <see cref="ApplicationSettings.ManualsFilePath"/>.
    /// </summary>
    public ApplicationSettings() 
    {
        this.IpAddress = string.Empty;
        this.ImagesFilePath = string.Empty;
        this.ManualsFilePath = string.Empty;
        this.CharacterSet = string.Empty;
        this.DatabaseSource = string.Empty;
    }

    /// <summary>
    /// A constructor where the <see cref="ApplicationSettings"/> can be set with custom values.
    /// </summary>
    /// <param name="ipAddress">
    /// A <see cref="string"/> to be passed into the <see cref="ApplicationSettings"/>.
    /// </param>
    /// <param name="imagesFilePath">
    /// A <see cref="string"/> file path to be passed into the <see cref="ApplicationSettings"/>.
    /// </param>
    /// <param name="manualsFilePath">
    /// A <see cref="string"/> file path to be passed into the <see cref="ApplicationSettings"/>.
    /// </param>
    public ApplicationSettings(string ipAddress,
                               string imagesFilePath,
                               string manualsFilePath,
                               string characterSet,
                               string databaseSource)
    {
        this.CharacterSet = characterSet;
        this.IpAddress = ipAddress;
        this.DatabaseSource = databaseSource;
        this.ImagesFilePath = imagesFilePath;
        this.ManualsFilePath = manualsFilePath;
    }
    #endregion
}
