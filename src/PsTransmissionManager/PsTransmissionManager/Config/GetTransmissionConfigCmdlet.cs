using System;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Models;
using PsTransmissionManager.Core.Services;

namespace TransmissionManager.Config
{
    [Cmdlet(VerbsCommon.Get, "TransmissionConfig", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class GetTransmissionConfigCmdlet : Cmdlet
    {
        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="SetTransmissionConfigCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="SetTransmissionConfigCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="SetTransmissionConfigCmdlet"/>.
        /// Retrieve the config if present
        /// </summary>
        protected override void EndProcessing()
        {
            try
            {
                TransmissionCredentials config = Task.Run(async () => await AuthService.GetConfig()).Result;

                if (config == null)
                    WriteWarning("No local configuration found.");
                else
                    WriteObject(config);
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to retrieve local configuration, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
