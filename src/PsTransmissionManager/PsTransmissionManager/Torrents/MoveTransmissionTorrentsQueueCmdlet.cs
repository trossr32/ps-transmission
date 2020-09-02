using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Constants;
using PsTransmissionManager.Core.Enums;
using PsTransmissionManager.Core.Models;
using PsTransmissionManager.Core.Services;
using PsTransmissionManager.Core.Services.Transmission;

namespace TransmissionManager.Torrents
{
    [Cmdlet(VerbsCommon.Move, "TransmissionTorrentsQueue", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class MoveTransmissionTorrentsQueueCmdlet : Cmdlet
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
        public SwitchParameter Up { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Down { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Top { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Bottom { get; set; }

        [Parameter(Mandatory = false)]
        public List<int> TorrentIds { get; set; }

        [Parameter(Mandatory = false)]
        public int? TorrentId { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="MoveTransmissionTorrentsQueueCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            // validate queue move parameter has been supplied
            if (!Up.IsPresent && !Down.IsPresent && !Top.IsPresent && !Bottom.IsPresent)
                ThrowTerminatingError(new ErrorRecord(new Exception("One of the following parameters must be supplied: Up, Down, Top, Bottom"), null, ErrorCategory.InvalidArgument, null));

            // validate a torrent id has been supplied
            if (!(TorrentIds ?? new List<int>()).Any() && !TorrentId.HasValue)
                ThrowTerminatingError(new ErrorRecord(new Exception("One of the following parameters must be supplied: TorrentIds, TorrentId"), null, ErrorCategory.InvalidArgument, null));
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="MoveTransmissionTorrentsQueueCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {

        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="MoveTransmissionTorrentsQueueCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            try
            {
                TransmissionCredentials config = Task.Run(async () => await AuthService.GetConfig(Host, User, Password)).Result;

                if (config == null)
                    ThrowTerminatingError(new ErrorRecord(new Exception(ErrorMessages.ConfigMissing), null, ErrorCategory.InvalidArgument, null));
            
                var torrentSvc = new TorrentService(config);

                List<int> torrentIds = (TorrentIds ?? new List<int>()).Any()
                    ? TorrentIds
                    : new List<int> {TorrentId.Value};

                if (Up.IsPresent)
                    Task.Run(async () => await torrentSvc.MoveTorrents(MoveInQueue.Up, torrentIds));
                else if (Down.IsPresent)
                    Task.Run(async () => await torrentSvc.MoveTorrents(MoveInQueue.Down, torrentIds));
                else if (Top.IsPresent)
                    Task.Run(async () => await torrentSvc.MoveTorrents(MoveInQueue.Top, torrentIds));
                else if (Bottom.IsPresent)
                    Task.Run(async () => await torrentSvc.MoveTorrents(MoveInQueue.Bottom, torrentIds));

                WriteObject("Torrent(s) moved.");
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to move torrent(s), see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
