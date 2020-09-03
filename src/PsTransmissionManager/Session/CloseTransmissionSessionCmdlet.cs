using System;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Services.Transmission;
using TransmissionManager.Base;

namespace TransmissionManager.Session
{
    [Cmdlet(VerbsCommon.Close, "TransmissionSession", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
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
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to close session, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
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
