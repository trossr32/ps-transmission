﻿using System;
using System.Management.Automation;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;
using Transmission.NetCore.Client.Models;

namespace Transmission.Session
{
    [Cmdlet(VerbsCommon.Get, "TransmissionSessionStatistics", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    [OutputType(typeof(Statistic))]
    public class GetTransmissionSessionStatisticsCmdlet : BaseTransmissionCmdlet
    {
        [Parameter(Mandatory = false)]
        public SwitchParameter Json { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="GetTransmissionSessionStatisticsCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="GetTransmissionSessionStatisticsCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                var sessionSvc = new SessionService();

                Statistic sessionStats = Task.Run(async () => await sessionSvc.GetStats()).Result;

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

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="GetTransmissionSessionStatisticsCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            
        }
    }
}
