using System.Reflection;
using Newtonsoft.Json;
using PsTransmission.Core.Models;

namespace PsTransmission.Core.Services;

public static class AuthService
{
    private static readonly string ConfigFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ps_transmission_manager_config.json");
        
    /// <summary>
    /// Attempt to retrieve credentials from a locally stored file.
    /// </summary>
    /// <returns></returns>
    public static async Task<TransmissionCredentials> GetConfig()
    {
        // if no config file exists then we have no credentials
        if (!File.Exists(ConfigFile))
            return null;

        var encrypted = await File.ReadAllTextAsync(ConfigFile);

        var decrypted = await EncryptionService.Decrypt(encrypted);

        return JsonConvert.DeserializeObject<TransmissionCredentials>(decrypted);
    }

    /// <summary>
    /// Create a local config file in the module directory with encrypted auth credentials.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static async Task SetConfig(TransmissionCredentials config)
    {
        var encrypted = await EncryptionService.Encrypt(JsonConvert.SerializeObject(config));

        await File.WriteAllTextAsync(ConfigFile, encrypted);
    }

    /// <summary>
    /// Remove config file if it exists and suppress any errors if it doesn't exist.
    /// </summary>
    public static void RemoveConfig()
    {
        try
        {
            File.Delete(ConfigFile);
        }
        catch (Exception) { }
    }
}