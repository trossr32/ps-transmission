using System;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Services.Transmission;

namespace TransmissionManager.System
{
    [Cmdlet(VerbsData.Update, "TransmissionBlockLists", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class UpdateTransmissionBlockListsCmdlet : Cmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "If supplied the response will be output as a boolean, otherwise a success message or terminating error will be output.")]
        public SwitchParameter AsBool { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="UpdateTransmissionBlockListsCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {

        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="UpdateTransmissionBlockListsCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                var systemSvc = new SystemService();

                (bool success, string error) response = Task.Run(async () => await systemSvc.UpdateBlockList()).Result;

                if (AsBool.IsPresent)
                {
                    WriteObject(response.success);

                    return;
                }

                if (!response.success)
                    ThrowTerminatingError(new ErrorRecord(new Exception($"Updating blocklists failed - {response.error}"), null, ErrorCategory.OperationStopped, null));

                WriteObject("Updating blocklists successful");
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to test port, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
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
}
