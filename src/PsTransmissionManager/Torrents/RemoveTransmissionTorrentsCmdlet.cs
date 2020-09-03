using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Services.Transmission;
using TransmissionManager.Base;

namespace TransmissionManager.Torrents
{
    [Cmdlet(VerbsCommon.Remove, "TransmissionTorrents", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class RemoveTransmissionTorrentsCmdlet : BaseTransmissionCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public List<int> TorrentIds { get; set; }
        
        [Parameter(Mandatory = false)]
        public SwitchParameter All { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Completed { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Incomplete { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter DeleteData { get; set; }

        private List<int> _torrentIds;

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="RemoveTransmissionTorrentsCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            _torrentIds = new List<int>();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="RemoveTransmissionTorrentsCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            if (TorrentIds != null)
                _torrentIds.AddRange(TorrentIds);
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="RemoveTransmissionTorrentsCmdlet"/>.
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
                    response = Task.Run(async () => await torrentSvc.RemoveTorrents(null, DeleteData.IsPresent)).Result;
                else if (Completed.IsPresent)
                {
                    response = Task.Run(async () => await torrentSvc.RemoveCompletedTorrents(DeleteData.IsPresent)).Result;

                    info = "completed ";
                }
                else if (Incomplete.IsPresent)
                {
                    response = Task.Run(async () => await torrentSvc.RemoveIncompleteTorrents(DeleteData.IsPresent)).Result;

                    info = "incomplete ";
                }
                else
                    response = Task.Run(async () => await torrentSvc.RemoveTorrents(_torrentIds, DeleteData.IsPresent)).Result;

                if (!response.success)
                    ThrowTerminatingError(new ErrorRecord(new Exception("Failed to remove torrents"), null, ErrorCategory.OperationStopped, null));

                if (response.torrentCount == 0)
                    WriteWarning("No torrents found.");
                else
                    WriteObject($"{response.torrentCount} {info}torrent{(response.torrentCount > 1 ? "s" : "")} removed.");
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to remove torrents, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
