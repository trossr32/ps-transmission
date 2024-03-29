﻿using PsTransmission.Core.Components;
using Transmission.NetCore.Client;

namespace PsTransmission.Core.Services.Transmission;

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
    public async Task<bool> TestPort() => await _client.PortTestAsync();

    /// <summary>
    /// Update blocklists
    /// </summary>
    /// <returns>success flag and error message, if applicable</returns>
    public async Task<(bool success, string error)> UpdateBlockList() => await _client.UpdateBlockListAsync();
}