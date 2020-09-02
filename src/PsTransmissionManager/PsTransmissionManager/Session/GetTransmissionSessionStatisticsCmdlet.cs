using System;
using System.Management.Automation;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PsTransmissionManager.Core.Constants;
using PsTransmissionManager.Core.Models;
using PsTransmissionManager.Core.Services;
using PsTransmissionManager.Core.Services.Transmission;
using Transmission.NetCore.Client.Models;

namespace TransmissionManager.Session
{
    [Cmdlet(VerbsCommon.Get, "TransmissionSessionStatistics", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class GetTransmissionSessionStatisticsCmdlet : Cmdlet
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

        [Parameter(Mandatory = false)]
        public SwitchParameter Json { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="GetTransmissionSessionStatisticsCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {

        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="GetTransmissionSessionStatisticsCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {

        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="GetTransmissionSessionStatisticsCmdlet"/>.
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
            
                Statistic sessionStats = Task.Run(async () => await transmissionSvc.GetStats()).Result;

                if (Json)
                    WriteObject(JsonConvert.SerializeObject(sessionStats));
                else
                    WriteObject(sessionStats);
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to retrieve session statistics, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
