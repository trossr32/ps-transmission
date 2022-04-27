using System.Management.Automation;
using PsTransmission.Core.Components;
using PsTransmission.Core.Services;

namespace Transmission.Credentials;

/// <summary>
/// <para type="synopsis">
/// Remove previously registered Transmission credentials.
/// </para>
/// <para type="description">
/// Remove previously registered Transmission credentials for the session.
/// </para>
/// <para type="description">
/// Optionally remove stored credentials from an encrypted local file by supplying the DeletePermanent switch parameter.
/// </para>
/// <example>
///     <para>Example 1: Remove previously registered credentials from session</para>
///     <code>PS C:\> Remove-TransmissionCredentials</code>
///     <remarks>Previously registered credentials will be removed.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Remove previously registered credentials from session and those stored locally</para>
///     <code>PS C:\> Remove-TransmissionCredentials -DeletePermanent</code>
///     <remarks>Previously registered credentials will be removed from the session and from the encrypted local file.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
/// </summary>
[Cmdlet(VerbsCommon.Remove, "TransmissionCredentials", HelpUri = "https://github.com/trossr32/ps-transmission")]
public class RemoveTransmissionCredentialsCmdlet : Cmdlet
{
    /// <summary>
    /// <para type="description">
    /// If supplied any credentials stored permanently in an encrypted local file will be deleted.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    [Alias("D")]
    public SwitchParameter DeletePermanent { get; set; }

    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="RemoveTransmissionCredentialsCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing()
    {
            
    }

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="RemoveTransmissionCredentialsCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
        try
        {
            TransmissionContext.Dispose();

            if (DeletePermanent.IsPresent)
                AuthService.RemoveConfig();
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to remove credentials with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="RemoveTransmissionCredentialsCmdlet"/>.
    /// </summary>
    protected override void EndProcessing()
    {
            
    }
}