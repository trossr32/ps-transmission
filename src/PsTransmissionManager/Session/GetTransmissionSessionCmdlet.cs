using System;
using System.Management.Automation;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PsTransmissionManager.Core.Services.Transmission;
using Transmission.NetCore.Client.Models;

namespace TransmissionManager.Session
{
    [Cmdlet(VerbsCommon.Get, "TransmissionSession", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class GetTransmissionSessionCmdlet : Cmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "If supplied the response will be output as JSON.")]
        public SwitchParameter Json { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="GetTransmissionSessionCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {

        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="GetTransmissionSessionCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                var sessionSvc = new SessionService();

                SessionInformation sessionInfo = Task.Run(async () => await sessionSvc.Get()).Result;

                if (Json)
                    WriteObject(JsonConvert.SerializeObject(sessionInfo));
                else
                    WriteObject(sessionInfo);
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to retrieve session information, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="GetTransmissionSessionCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            
        }
    }
}
