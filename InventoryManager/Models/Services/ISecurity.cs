using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;


namespace InventoryManager.Models.Services;

/// <summary>
/// An interface meant to be used for user verification of
/// users and admin priveleges
/// </summary>
public interface ISecurity
{
    /// <summary>
    /// A method meant to be used to check if a user exist.
    /// </summary>
    /// <param name="connectionString">
    /// The <see cref="FbConnectionStringBuilder"/> meant to be used as a way to store
    /// and use the connection string for the server.
    /// </param>
    /// <returns>
    /// Returns a <see cref="bool"/> so the GUI can listen to this result
    /// </returns>
    Task<bool> VerifyCredentialsAsync(FbConnectionStringBuilder connectionString);

    /// <summary>
    /// A method meant to be used to check if a user has admin
    /// priveleges.
    /// </summary>
    /// <param name="connectionString">
    /// The <see cref="FbConnectionStringBuilder"/> meant to be used as a way to store
    /// and use the connection string for the server.
    /// </param>
    /// <returns>
    /// Returns an <see cref="IObservable{bool}"/> so the GUI can listen to this result
    /// </returns>
    Task<bool> VerifyAdminAsync(FbConnectionStringBuilder connectionString);
}
