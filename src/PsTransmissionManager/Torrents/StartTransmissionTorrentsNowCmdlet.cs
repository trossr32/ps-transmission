using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Services.Transmission;

namespace TransmissionManager.Torrents
{
    [Cmdlet(VerbsLifecycle.Start, "TransmissionTorrentsNow", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class StartTransmissionTorrentsNowCmdlet : Cmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Array of torrent ids.")]
        public List<int> TorrentIds { get; set; }
        
        [Parameter(Mandatory = false, HelpMessage = "Apply to all torrents.")]
        public SwitchParameter All { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Apply to completed torrents.")]
        public SwitchParameter Completed { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Apply to incomplete torrents.")]
        public SwitchParameter Incomplete { get; set; }

        private List<int> _torrentIds;

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="StartTransmissionTorrentsNowCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            if (!All.IsPresent && !Completed.IsPresent && !Incomplete.IsPresent && !(TorrentIds ?? new List<int>()).Any())
                ThrowTerminatingError(new ErrorRecord(new Exception("One of the following parameters must be supplied: All, Completed, Incomplete, TorrentIds"), null, ErrorCategory.InvalidArgument, null));

            _torrentIds = new List<int>();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="StartTransmissionTorrentsNowCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            _torrentIds.AddRange(TorrentIds);
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="StartTransmissionTorrentsNowCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            try
            {
                var torrentSvc = new TorrentService();

                (bool success, int torrentCount) response;
                string info = "";

                if (All.IsPresent)
                    response = Task.Run(async () => await torrentSvc.StartTorrentsNow()).Result;
                else if (Completed.IsPresent)
                {
                    response = Task.Run(async () => await torrentSvc.StartCompletedTorrentsNow()).Result;

                    info = "completed ";
                }
                else if (Incomplete.IsPresent)
                {
                    response = Task.Run(async () => await torrentSvc.StartIncompleteTorrentsNow()).Result;

                    info = "incomplete ";
                }
                else
                    response = Task.Run(async () => await torrentSvc.StartTorrentsNow(_torrentIds)).Result;

                if (!response.success)
                    ThrowTerminatingError(new ErrorRecord(new Exception("Failed to start torrents using 'start now'"), null, ErrorCategory.OperationStopped, null));

                if (response.torrentCount == 0)
                    WriteWarning("No torrents found.");
                else
                    WriteObject($"{response.torrentCount} {info}torrent{(response.torrentCount > 1 ? "s" : "")} started using 'start now'.");
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to start torrents using 'start now', see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
