using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PsTransmissionManager.Core.Services.Transmission;
using Transmission.NetCore.Client.Models;

namespace TransmissionManager.Torrents
{
    [Cmdlet(VerbsCommon.Get, "TransmissionTorrents", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class GetTransmissionTorrentsCmdlet : Cmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Array of torrent ids.")]
        public List<int> TorrentIds { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Completed { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Incomplete { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "If supplied the response will be output as JSON.")]
        public SwitchParameter Json { get; set; }

        private List<int> _torrentIds;

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="GetTransmissionTorrentsCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            _torrentIds = new List<int>();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="GetTransmissionTorrentsCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            _torrentIds.AddRange(TorrentIds);
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="GetTransmissionTorrentsCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            try
            {
                var torrentSvc = new TorrentService();
            
                Torrent[] torrents;

                if (Completed.IsPresent)
                    torrents = Task.Run(async () => await torrentSvc.GetCompletedTorrents()).Result;
                else if (Incomplete.IsPresent)
                    torrents = Task.Run(async () => await torrentSvc.GetIncompleteTorrents()).Result;
                else if (_torrentIds.Any())
                    torrents = Task.Run(async () => await torrentSvc.GetTorrents(_torrentIds)).Result;
                else
                    torrents = Task.Run(async () => await torrentSvc.GetTorrents()).Result;

                if (Json)
                    WriteObject(JsonConvert.SerializeObject(torrents));
                else
                    WriteObject(torrents);
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to retrieve torrents, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
