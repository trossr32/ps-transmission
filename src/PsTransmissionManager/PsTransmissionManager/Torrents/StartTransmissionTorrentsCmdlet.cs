using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Constants;
using PsTransmissionManager.Core.Models;
using PsTransmissionManager.Core.Services;
using PsTransmissionManager.Core.Services.Transmission;

namespace TransmissionManager.Torrents
{
    [Cmdlet(VerbsLifecycle.Start, "TransmissionTorrents", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class StartTransmissionTorrentsCmdlet : Cmdlet
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
        public SwitchParameter All { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Completed { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Incomplete { get; set; }

        [Parameter(Mandatory = false)]
        public List<int> TorrentIds { get; set; }

        [Parameter(Mandatory = false)]
        public int? TorrentId { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="StartTransmissionTorrentsCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            if (!All.IsPresent && !Completed.IsPresent && !Incomplete.IsPresent && !(TorrentIds ?? new List<int>()).Any() && !TorrentId.HasValue)
                ThrowTerminatingError(new ErrorRecord(new Exception("One of the following parameters must be supplied: All, Completed, Incomplete, TorrentIds, TorrentId"), null, ErrorCategory.InvalidArgument, null));
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
                TransmissionCredentials config = Task.Run(async () => await AuthService.GetConfig(Host, User, Password)).Result;

                if (config == null)
                    ThrowTerminatingError(new ErrorRecord(new Exception(ErrorMessages.ConfigMissing), null, ErrorCategory.InvalidArgument, null));
            
                var torrentSvc = new TorrentService(config);

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
                else if (TorrentId.HasValue)
                    response = Task.Run(async () => await torrentSvc.StartTorrents(new List<int> { TorrentId.Value })).Result;
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
