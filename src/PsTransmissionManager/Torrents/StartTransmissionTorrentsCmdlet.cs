using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Services.Transmission;
using TransmissionManager.Base;

namespace TransmissionManager.Torrents
{
    [Cmdlet(VerbsLifecycle.Start, "TransmissionTorrents", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class StartTransmissionTorrentsCmdlet : BaseTransmissionCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public List<int> TorrentIds { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter All { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Completed { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Incomplete { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="StartTransmissionTorrentsCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="StartTransmissionTorrentsCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {

        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="StartTransmissionTorrentsCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            try
            {
                if (!All.IsPresent && !Completed.IsPresent && !Incomplete.IsPresent && !(TorrentIds ?? new List<int>()).Any())
                    ThrowTerminatingError(new ErrorRecord(new Exception("One of the following parameters must be supplied: All, Completed, Incomplete, TorrentIds"), null, ErrorCategory.InvalidArgument, null));

                var torrentSvc = new TorrentService();

                (bool success, int torrentCount) response;
                string info = "";

                if (All.IsPresent)
                    response = Task.Run(async () => await torrentSvc.StartTorrents()).Result;
                else if (Completed.IsPresent)
                {
                    response = Task.Run(async () => await torrentSvc.StartCompletedTorrents()).Result;

                    info = "completed ";
                }
                else if (Incomplete.IsPresent)
                {
                    response = Task.Run(async () => await torrentSvc.StartIncompleteTorrents()).Result;

                    info = "incomplete ";
                }
                else
                    response = Task.Run(async () => await torrentSvc.StartTorrents(TorrentIds)).Result;

                if (!response.success)
                    ThrowTerminatingError(new ErrorRecord(new Exception("Failed to start torrents"), null, ErrorCategory.OperationStopped, null));

                if (response.torrentCount == 0)
                    WriteWarning("No torrents found.");
                else
                    WriteObject($"{response.torrentCount} {info}torrent{(response.torrentCount > 1 ? "s" : "")} started.");
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to start torrents, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
