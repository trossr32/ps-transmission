﻿using PsTransmission.Core.Components;
using Transmission.NetCore.Client;
using Transmission.NetCore.Client.Models;

namespace PsTransmission.Core.Services.Transmission;

public class SessionService
{
    private readonly TransmissionClient _client;

    public SessionService()
    {
        _client = new TransmissionClient(TransmissionContext.Credentials?.Host, null, TransmissionContext.Credentials?.User, TransmissionContext.Credentials?.Password);
    }

    /// <summary>
    /// Get session information
    /// </summary>
    /// <returns></returns>
    public async Task<SessionInformation> Get() => await _client.SessionGetAsync();

    /// <summary>
    /// Get session statistics
    /// </summary>
    /// <returns></returns>
    public async Task<Statistic> GetStats() => await _client.SessionGetStatisticAsync();

    /// <summary>
    /// Set session settings
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Set(SessionSettings request) => await _client.SessionSetAsync(request);

    /// <summary>
    /// Close session <br />
    /// Careful with this one; it essentially shuts transmission down and will need to be restarted to bring it back.
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Close() => await _client.SessionCloseAsync();
}