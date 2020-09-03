using System;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Services.Transmission;
using TransmissionManager.Base;

namespace TransmissionManager.System
{
    [Cmdlet(VerbsDiagnostic.Test, "TransmissionPort", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class TestTransmissionPortCmdlet : BaseTransmissionCmdlet
    {
        [Parameter(Mandatory = false)]
        public SwitchParameter AsBool { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="TestTransmissionPortCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="TestTransmissionPortCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                var systemSvc = new SystemService();

                bool success = Task.Run(async () => await systemSvc.TestPort()).Result;

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
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to test port, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
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
}
