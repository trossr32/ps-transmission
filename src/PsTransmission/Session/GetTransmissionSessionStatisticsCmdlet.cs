using System.Management.Automation;
using Newtonsoft.Json;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;
using Transmission.NetCore.Client.Models;

namespace Transmission.Session;

/// <summary>
/// <para type="synopsis">
/// Gets Transmission session statistics.
/// </para>
/// <para type="description">
/// Gets Transmission session statistics.
/// </para>
/// <para type="description">
/// Calls the 'session-stats' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
/// </para>
/// <example>
///     <para>Example 1: Get session statistics</para>
///     <code>PS C:\> Get-TransmissionSessionStatistics</code>
///     <remarks>Retrieves the session statistics.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Get session statistics as JSON</para>
///     <code>PS C:\> Get-TransmissionSessionStatistics -Json</code>
///     <remarks>Retrieves the session statistics as a JSON string.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
/// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
/// </summary>
[Cmdlet(VerbsCommon.Get, "TransmissionSessionStatistics", HelpUri = "https://github.com/trossr32/ps-transmission")]
[OutputType(typeof(Statistic))]
public class GetTransmissionSessionStatisticsCmdlet : BaseTransmissionCmdlet
{
    /// <summary>
    /// <para type="description">
    /// If supplied the data will be output as a JSON string.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Json { get; set; }

    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="GetTransmissionSessionStatisticsCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing() => base.BeginProcessing();

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="GetTransmissionSessionStatisticsCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
        try
        {
            var sessionSvc = new SessionService();

            var sessionStats = Task.Run(sessionSvc.GetStats).Result;

            if (Json)
                WriteObject(JsonConvert.SerializeObject(sessionStats));
            else
                WriteObject(sessionStats);
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to retrieve session statistics with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="GetTransmissionSessionStatisticsCmdlet"/>.
    /// Retrieve all torrents
    /// </summary>
    protected override void EndProcessing()
    {
            
    }
}