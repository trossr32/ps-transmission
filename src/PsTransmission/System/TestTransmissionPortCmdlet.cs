using System.Management.Automation;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;

namespace Transmission.System;

/// <summary>
/// <para type="synopsis">
/// Tests to see if your incoming peer port is accessible from the outside world.
/// </para>
/// <para type="description">
/// Tests to see if your incoming peer port is accessible from the outside world.
/// </para>
/// <para type="description">
/// Calls the 'port-test' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
/// </para>
/// <example>
///     <para>Example 1: Test port</para>
///     <code>PS C:\> Test-TransmissionPort</code>
///     <remarks>Test port and return a success message or throw a terminating error.</remarks>
/// </example>
/// <example>
///     <para></para>
///     <code>PS C:\> Test-TransmissionPort -AsBool</code>
///     <remarks>Test port and return a boolean.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
/// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
/// </summary>
[Cmdlet(VerbsDiagnostic.Test, "TransmissionPort", HelpUri = "https://github.com/trossr32/ps-transmission")]
public class TestTransmissionPortCmdlet : BaseTransmissionCmdlet
{
    /// <summary>
    /// <para type="description">
    /// If supplied the response will be a boolean, otherwise a success message or a terminating error.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter AsBool { get; set; }

    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="TestTransmissionPortCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing() => base.BeginProcessing();

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="TestTransmissionPortCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
        try
        {
            var systemSvc = new SystemService();

            var success = Task.Run(systemSvc.TestPort).Result;

            if (AsBool.IsPresent)
            {
                WriteObject(success);

                return;
            }

            if (!success)
                ThrowTerminatingError(new ErrorRecord(new Exception("Port test failed"), null, ErrorCategory.OperationStopped, null));

            WriteObject("Port test successful");
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to test port with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="TestTransmissionPortCmdlet"/>.
    /// Retrieve all torrents
    /// </summary>
    protected override void EndProcessing()
    {
            
    }
}