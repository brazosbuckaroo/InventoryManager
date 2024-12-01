namespace InventoryManager.Models.Types;

/// <summary>
/// A class meant to manage the security service of the
/// application.
/// </summary>
public class SecurityManager : ISecurity
{
    #region METHODS
    /// <inheritdoc/>
    public async Task<bool> VerifyCredentialsAsync(FbConnectionStringBuilder connectionString)
    {
        if (connectionString.DataSource == string.Empty ||
            !IPAddress.TryParse(connectionString.DataSource, out IPAddress? _))
        {
            return false;
        }

        try
        {
            FbSecurity security = new FbSecurity();
            security.ConnectionString = connectionString.ToString();
            FbUserData userData = await security.DisplayUserAsync(connectionString.UserID).ConfigureAwait(false);
        }
        catch (FbException error)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> VerifyAdminAsync(FbConnectionStringBuilder connectionString)
    {
        bool isAdmin = new bool();

        using (FbConnection connection = new FbConnection(connectionString.ToString()))
        {
            await connection.OpenAsync().ConfigureAwait(false);

            await using (FbTransaction transaction = connection.BeginTransaction())
            {
                FbCommand command = new FbCommand();
                command.CommandText = "SELECT U.SEC$ADMIN FROM SEC$USERS U WHERE U.SEC$USER_NAME = (@A)";
                command.Parameters.Add("@A", FbDbType.VarChar).Value = connectionString.UserID.ToUpper();
                command.Connection = connection;
                command.Transaction = transaction;

                await command.PrepareAsync();

                FbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (await reader.ReadAsync().ConfigureAwait(false))
                {
                    isAdmin = (bool)reader.GetValue(0);
                }
            }
        }

        return isAdmin;
    }
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// The default constructor for the security manager.
    /// </summary>
    public SecurityManager()
    { 
    }
    #endregion
}

