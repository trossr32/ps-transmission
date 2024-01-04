using System.Management.Automation;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;

namespace Transmission.System;

/// <summary>
/// <para type="synopsis">
/// Updates blocklists.
/// </para>
/// <para type="description">
/// Updates blocklists.
/// </para>
/// <para type="description">
/// Calls the 'blocklist-update' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
/// </para>
/// <example>
///     <para>Example 1: Update blocklists</para>
///     <code>PS C:\> Update-TransmissionBlockLists</code>
///     <remarks>Update blocklists and return a success message or throw a terminating error.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Update blocklists and return a boolean</para>
///     <code>PS C:\> Update-TransmissionBlockLists -AsBool</code>
///     <remarks>Update blocklists and return a boolean.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
/// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
/// </summary>
[Cmdlet(VerbsData.Update, "TransmissionBlockLists", HelpUri = "https://github.com/trossr32/ps-transmission")]
public class UpdateTransmissionBlockListsCmdlet : BaseTransmissionCmdlet
{
    /// <summary>
    /// <para type="description">
    /// If supplied the response will be a boolean, otherwise a success message or a terminating error.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter AsBool { get; set; }

    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="UpdateTransmissionBlockListsCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing() => base.BeginProcessing();

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="UpdateTransmissionBlockListsCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
        try
        {
            var systemSvc = new SystemService();

            var (success, error) = Task.Run(systemSvc.UpdateBlockList).Result;

            if (AsBool.IsPresent)
            {
                WriteObject(success);

                return;
            }

            if (!success)
                ThrowTerminatingError(new ErrorRecord(new Exception($"Updating blocklists failed - {error}"), null, ErrorCategory.OperationStopped, null));

            WriteObject("Updating blocklists successful");
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to test port with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="UpdateTransmissionBlockListsCmdlet"/>.
    /// Retrieve all torrents
    /// </summary>
    protected override void EndProcessing()
    {
            
    }
}