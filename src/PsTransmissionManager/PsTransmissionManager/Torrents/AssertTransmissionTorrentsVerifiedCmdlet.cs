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
    [Cmdlet(VerbsLifecycle.Assert, "TransmissionTorrentsVerified", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class AssertTransmissionTorrentsVerifiedCmdlet : Cmdlet
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
        public List<int> TorrentIds { get; set; }

        [Parameter(Mandatory = false)]
        public int? TorrentId { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter AsBool { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="AssertTransmissionTorrentsVerifiedCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            // validate a torrent id has been supplied
            if (!(TorrentIds ?? new List<int>()).Any() && !TorrentId.HasValue)
                ThrowTerminatingError(new ErrorRecord(new Exception("One of the following parameters must be supplied: TorrentIds, TorrentId"), null, ErrorCategory.InvalidArgument, null));
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="AssertTransmissionTorrentsVerifiedCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {

        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="AssertTransmissionTorrentsVerifiedCmdlet"/>.
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
                    : new List<int> { TorrentId.Value };

                bool success = Task.Run(async () => await torrentSvc.VerifyTorrents(torrentIds)).Result;

                if (AsBool.IsPresent)
                {
                    WriteObject(success);

                    return;
                }

                if (!success)
                    ThrowTerminatingError(new ErrorRecord(new Exception("Torrent(s) verification failed"), null, ErrorCategory.OperationStopped, null));

                WriteObject("Torrent(s) verification successful");
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to verify torrent(s), see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
