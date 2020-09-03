using System.Threading.Tasks;
using PsTransmissionManager.Core.Components;
using Transmission.NetCore.Client;

namespace PsTransmissionManager.Core.Services.Transmission
{
    public class SystemService
    {
        private readonly TransmissionClient _client;

        public SystemService()
        {
            _client = new TransmissionClient(TransmissionContext.Credentials?.Host, null, TransmissionContext.Credentials?.User, TransmissionContext.Credentials?.Password);
        }

        /// <summary>
        /// Port test
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TestPort()
        {
            return await _client.PortTestAsync();
        }

        /// <summary>
        /// Update blocklists
        /// </summary>
        /// <returns>success flag and error message, if applicable</returns>
        public async Task<(bool success, string error)> UpdateBlockList()
        {
            return await _client.UpdateBlockListAsync();
        }
    }
}
