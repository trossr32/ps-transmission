using System.Threading.Tasks;
using PsTransmissionManager.Core.Models;
using Transmission.NetCore.Client;

namespace PsTransmissionManager.Core.Services.Transmission
{
    public class SystemService
    {
        private readonly TransmissionClient _client;

        public SystemService(TransmissionCredentials config)
        {
            config ??= Task.Run(async () => await AuthService.GetConfig()).Result;

            _client = new TransmissionClient(config?.Host, null, config?.User, config?.Password);
        }

        /// <summary>
        /// Port test
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TestPort()
        {
            return (await _client.PortTestAsync());
        }

        /// <summary>
        /// Update blocklists
        /// </summary>
        /// <returns>success flag and error message, if applicable</returns>
        public async Task<(bool success, string error)> UpdateBlockList()
        {
            return (await _client.UpdateBlockListAsync());
        }
    }
}
