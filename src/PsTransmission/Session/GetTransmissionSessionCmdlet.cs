using System.Management.Automation;
using Newtonsoft.Json;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;
using Transmission.NetCore.Client.Models;

namespace Transmission.Session;

/// <summary>
/// <para type="synopsis">
/// Gets Transmission session information.
/// </para>
/// <para type="description">
/// Gets Transmission session information.
/// </para>
/// <para type="description">
/// Calls the 'session-get' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
/// </para>
/// <example>
///     <para>Example 1: Get session information</para>
///     <code>PS C:\> Get-TransmissionSession</code>
///     <remarks>Retrieves the session information.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Get session information as JSON</para>
///     <code>PS C:\> Get-TransmissionSession -Json</code>
///     <remarks>Retrieves the session information as a JSON string.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
/// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
/// </summary>
[Cmdlet(VerbsCommon.Get, "TransmissionSession", HelpUri = "https://github.com/trossr32/ps-transmission")]
[OutputType(typeof(SessionInformation))]
public class GetTransmissionSessionCmdlet : BaseTransmissionCmdlet
{
    /// <summary>
    /// <para type="description">
    /// If supplied the data will be output as a JSON string.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Json { get; set; }

    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="GetTransmissionSessionCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing()
    {
        base.BeginProcessing();
    }

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="GetTransmissionSessionCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
        try
        {
            var sessionSvc = new SessionService();

            SessionInformation sessionInfo = Task.Run(async () => await sessionSvc.Get()).Result;

            if (Json)
                WriteObject(JsonConvert.SerializeObject(sessionInfo));
            else
                WriteObject(sessionInfo);
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to retrieve session information with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="GetTransmissionSessionCmdlet"/>.
    /// Retrieve all torrents
    /// </summary>
    protected override void EndProcessing()
    {
            
    }
}