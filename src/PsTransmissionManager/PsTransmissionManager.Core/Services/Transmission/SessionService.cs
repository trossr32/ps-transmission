﻿using System.Threading.Tasks;
using PsTransmissionManager.Core.Models;
using Transmission.NetCore.Client;
using Transmission.NetCore.Client.Models;

namespace PsTransmissionManager.Core.Services.Transmission
{
    public class SessionService
    {
        private readonly TransmissionClient _client;

        public SessionService(TransmissionCredentials config)
        {
            config ??= Task.Run(async () => await AuthService.GetConfig()).Result;

            _client = new TransmissionClient(config?.Host, null, config?.User, config?.Password);
        }

        /// <summary>
        /// Get session information
        /// </summary>
        /// <returns></returns>
        public async Task<SessionInformation> Get()
        {
            return await _client.SessionGetAsync();
        }

        /// <summary>
        /// Get session statistics
        /// </summary>
        /// <returns></returns>
        public async Task<Statistic> GetStats()
        {
            return await _client.SessionGetStatisticAsync();
        }

        /// <summary>
        /// Set session settings
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Set(SessionSettings request)
        {
            return await _client.SessionSetAsync(request);
        }

        /// <summary>
        /// Close session <br />
        /// Careful with this one; it essentially shuts transmission down and will need to be restarted to bring it back.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Close()
        {
            return await _client.SessionCloseAsync();
        }
    }
}