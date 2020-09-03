using System;
using System.Management.Automation;
using PsTransmission.Core.Components;
using PsTransmission.Core.Services;

namespace Transmission.Credentials
{
    [Cmdlet(VerbsCommon.Remove, "TransmissionCredentials", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class RemoveTransmissionCredentialsCmdlet : Cmdlet
    {
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
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to remove credentials, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="RemoveTransmissionCredentialsCmdlet"/>.
        /// </summary>
        protected override void EndProcessing()
        {
            
        }
    }
}
