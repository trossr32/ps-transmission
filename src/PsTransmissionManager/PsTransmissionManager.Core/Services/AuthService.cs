using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PsTransmissionManager.Core.Models;

namespace PsTransmissionManager.Core.Services
{
    public static class AuthService
    {
        private static readonly string ConfigFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ps_transmission_manager_config.json");
        
        /// <summary>
        /// Get a configuration wrapper for authentication details. <br />
        /// If a valid (not empty) host variable is supplied then a wrapper will be constructed from the supplied parameters. <br />
        /// If parameters are invalid or not supplied then attempt to retrieve the config from a locally stored file.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<TransmissionCredentials> GetConfig(string host = null, string user = null, string password = null)
        {
            // if any values have been supplied then return a new config wrapper
            if (!string.IsNullOrWhiteSpace(host))
                return new TransmissionCredentials
                {
                    Host = !string.IsNullOrWhiteSpace(host) ? host : null,
                    User = !string.IsNullOrWhiteSpace(user) ? user : null,
                    Password = !string.IsNullOrWhiteSpace(password) ? password : null
                };

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
}
