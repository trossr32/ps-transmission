using System;
using System.Management.Automation;
using PsTransmissionManager.Core.Services;

namespace TransmissionManager.Config
{
    [Cmdlet(VerbsCommon.Remove, "TransmissionConfig", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class RemoveTransmissionConfigCmdlet : Cmdlet
    {
        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="RemoveTransmissionConfigCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="RemoveTransmissionConfigCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="RemoveTransmissionConfigCmdlet"/>.
        /// Retrieve the config if present
        /// </summary>
        protected override void EndProcessing()
        {
            try
            {
                AuthService.RemoveConfig();
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to remove local configuration, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
