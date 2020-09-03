using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Enums;
using PsTransmissionManager.Core.Services.Transmission;

namespace TransmissionManager.Torrents
{
    [Cmdlet(VerbsCommon.Move, "TransmissionTorrentsQueue", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class MoveTransmissionTorrentsQueueCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Array of torrent ids.")]
        public List<int> TorrentIds { get; set; }
        
        [Parameter(Mandatory = false, HelpMessage = "Move torrent(s) up the queue.")]
        public SwitchParameter Up { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Move torrent(s) down the queue.")]
        public SwitchParameter Down { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Move torrent(s) to the top of the queue.")]
        public SwitchParameter Top { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Move torrent(s) to the bottom of the queue.")]
        public SwitchParameter Bottom { get; set; }

        private List<int> _torrentIds;

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="MoveTransmissionTorrentsQueueCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            // validate queue move parameter has been supplied
            if (!Up.IsPresent && !Down.IsPresent && !Top.IsPresent && !Bottom.IsPresent)
                ThrowTerminatingError(new ErrorRecord(new Exception("One of the following parameters must be supplied: Up, Down, Top, Bottom"), null, ErrorCategory.InvalidArgument, null));

            // validate a torrent id has been supplied
            if (!(TorrentIds ?? new List<int>()).Any())
                ThrowTerminatingError(new ErrorRecord(new Exception("The TorrentIds parameter must be supplied."), null, ErrorCategory.InvalidArgument, null));

            _torrentIds = new List<int>();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="MoveTransmissionTorrentsQueueCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            _torrentIds.AddRange(TorrentIds);
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="MoveTransmissionTorrentsQueueCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            try
            {
                var torrentSvc = new TorrentService();

                if (Up.IsPresent)
                    Task.Run(async () => await torrentSvc.MoveTorrents(MoveInQueue.Up, _torrentIds));
                else if (Down.IsPresent)
                    Task.Run(async () => await torrentSvc.MoveTorrents(MoveInQueue.Down, _torrentIds));
                else if (Top.IsPresent)
                    Task.Run(async () => await torrentSvc.MoveTorrents(MoveInQueue.Top, _torrentIds));
                else if (Bottom.IsPresent)
                    Task.Run(async () => await torrentSvc.MoveTorrents(MoveInQueue.Bottom, _torrentIds));

                WriteObject("Torrent(s) moved.");
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to move torrent(s), see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
