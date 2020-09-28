using System;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;

namespace Transmission.Session
{
    /// <summary>
    /// <para type="synopsis">
    /// Tells the Transmission session to shut down.
    /// </para>
    /// <para type="description">
    /// Tells the Transmission session to shut down.
    /// </para>
    /// <para type="description">
    /// Calls the 'session-close' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
    /// </para>
    /// <para type="description">
    /// Once shut down there is no way to restart Transmission via the API.
    /// </para>
    /// <example>
    ///     <para>Example 1: Shut down Transmission</para>
    ///     <code>PS C:\> Close-TransmissionSession</code>
    ///     <remarks>Transmission will be shut down.</remarks>
    /// </example>
    /// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
    /// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Close, "TransmissionSession", HelpUri = "https://github.com/trossr32/ps-transmission")]
    public class CloseTransmissionSessionCmdlet : BaseTransmissionCmdlet
    {
        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="CloseTransmissionSessionCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="CloseTransmissionSessionCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                var sessionSvc = new SessionService();
            
                bool success = Task.Run(async () => await sessionSvc.Close()).Result;

                if (success)
                    WriteObject("Session closed");
                else
                    ThrowTerminatingError(new ErrorRecord(new Exception("Failed to close session"), null, ErrorCategory.OperationStopped, null));
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to close session with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
            }
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="CloseTransmissionSessionCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            
        }
    }
}
