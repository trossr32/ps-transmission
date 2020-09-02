using System;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Constants;
using PsTransmissionManager.Core.Models;
using PsTransmissionManager.Core.Services;
using PsTransmissionManager.Core.Services.Transmission;

namespace TransmissionManager.Session
{
    [Cmdlet(VerbsCommon.Close, "TransmissionSession", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class CloseTransmissionSessionCmdlet : Cmdlet
    {
        [Parameter(Mandatory = false)]
        [Alias("H")]
        public string Host { get; set; }

        [Parameter(Mandatory = false)]
        [Alias("U")]
        public string User { get; set; }

        [Parameter(Mandatory = false)]
        [Alias("P")]
        public string Password { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="CloseTransmissionSessionCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {

        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="CloseTransmissionSessionCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {

        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="CloseTransmissionSessionCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            try
            {
                TransmissionCredentials config = Task.Run(async () => await AuthService.GetConfig(Host, User, Password)).Result;

                if (config == null)
                    ThrowTerminatingError(new ErrorRecord(new Exception(ErrorMessages.ConfigMissing), null, ErrorCategory.InvalidArgument, null));

                var transmissionSvc = new SessionService(config);
            
                bool success = Task.Run(async () => await transmissionSvc.Close()).Result;

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
    }
}
