using System.Management.Automation;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;
using Transmission.NetCore.Client.Models;

namespace Transmission.Session;

/// <summary>
/// <para type="synopsis">
/// Set Transmission alt speed state.
/// </para>
/// <para type="description">
/// Set Transmission alt speed state.
/// </para>
/// <para type="description">
/// Calls the 'session-set' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
/// </para>
/// <example>
///     <para>Example 1: Enable the alt speed limit</para>
///     <code>PS C:\> Set-TransmissionAltSpeedLimits -Enable</code>
///     <remarks>Enable the alt speed limit.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Disable the alt speed limit</para>
///     <code>PS C:\> Set-TransmissionAltSpeedLimits -Disable</code>
///     <remarks>Disable the alt speed limit.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
/// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
/// </summary>
[Cmdlet(VerbsCommon.Set, "TransmissionAltSpeedLimits", HelpUri = "https://github.com/trossr32/ps-transmission")]
[OutputType(typeof(SessionInformation))]
public class SetTransmissionAltSpeedLimitsCmdlet : BaseTransmissionCmdlet
{
    /// <summary>
    /// <para type="description">
    /// Enable alt speed limits.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Enable { get; set; }

    /// <summary>
    /// <para type="description">
    /// Disable alt speed limits.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Disable { get; set; }

    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="SetTransmissionAltSpeedLimitsCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing() => base.BeginProcessing();

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="SetTransmissionAltSpeedLimitsCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
        try
        {
            var sessionSvc = new SessionService();

            if (!Enable.IsPresent && !Disable.IsPresent)
                throw new Exception("Either the Enable or Disable parameter must be supplied");

            var request = new SessionSettings {
                AlternativeSpeedEnabled = Enable.IsPresent
            };

            var success = Task.Run(async () => await sessionSvc.Set(request)).Result;

            if (success)
                WriteObject($"Alt speed settings {(Enable.IsPresent ? "enabled" : "disabled")} successfully");
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception(e.Message, e), null, ErrorCategory.OperationStopped, null));
        }
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="SetTransmissionAltSpeedLimitsCmdlet"/>.
    /// Retrieve all torrents
    /// </summary>
    protected override void EndProcessing()
    {
            
    }
}