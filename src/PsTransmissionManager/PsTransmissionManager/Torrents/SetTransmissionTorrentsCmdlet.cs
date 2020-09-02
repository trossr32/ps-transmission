using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Constants;
using PsTransmissionManager.Core.Models;
using PsTransmissionManager.Core.Services;
using PsTransmissionManager.Core.Services.Transmission;
using Transmission.NetCore.Client.Models;

namespace TransmissionManager.Torrents
{
    [Cmdlet(VerbsCommon.Set, "TransmissionTorrents", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class SetTransmissionTorrentsCmdlet : Cmdlet
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

        /// <summary>
        /// This torrent's bandwidth tr_priority_t
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? BandwidthPriority { get; set; }

        /// <summary>
        /// Maximum download speed (KBps)
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? DownloadLimit { get; set; }

        /// <summary>
        /// Download limit is honoured
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? DownloadLimited { get; set; }

        /// <summary>
        /// Session upload limits are honoured
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? HonoursSessionLimits { get; set; }

        /// <summary>
        /// New location of the torrent's content
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Location { get; set; }

        /// <summary>
        /// Maximum number of peers
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? PeerLimit { get; set; }

        /// <summary>
        /// Position of this torrent in its queue [0...n)
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? QueuePosition { get; set; }

        /// <summary>
        /// Torrent-level number of minutes of seeding inactivity
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? SeedIdleLimit { get; set; }

        /// <summary>
        /// Which seeding inactivity to use
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? SeedIdleMode { get; set; }

        /// <summary>
        /// Torrent-level seeding ratio
        /// </summary>
        [Parameter(Mandatory = false)]
        public double? SeedRatioLimit { get; set; }

        /// <summary>
        /// Which ratio to use. 
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? SeedRatioMode { get; set; }

        /// <summary>
        /// Maximum upload speed (KBps)
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? UploadLimit { get; set; }

        /// <summary>
        /// Upload limit is honored
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? UploadLimited { get; set; }

        /// <summary>
        /// Strings of announce URLs to add
        /// </summary>
        [Parameter(Mandatory = false)]
        public List<string> TrackerAdd { get; set; }

        /// <summary>
        /// Ids of trackers to remove
        /// </summary>
        [Parameter(Mandatory = false)]
        public List<int> TrackerRemove { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter AsBool { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="SetTransmissionTorrentsCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            // validate a torrent id has been supplied
            if (!(TorrentIds ?? new List<int>()).Any() && !TorrentId.HasValue)
                ThrowTerminatingError(new ErrorRecord(new Exception("One of the following parameters must be supplied: TorrentIds, TorrentId"), null, ErrorCategory.InvalidArgument, null));
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="SetTransmissionTorrentsCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {

        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="SetTransmissionTorrentsCmdlet"/>.
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

                var request = new TorrentSettings
                {
                    BandwidthPriority = BandwidthPriority,
                    DownloadLimit = DownloadLimit,
                    DownloadLimited = DownloadLimited,
                    HonorsSessionLimits = HonoursSessionLimits,
                    Ids = torrentIds.ToArray(),
                    Location = Location,
                    PeerLimit = PeerLimit,
                    QueuePosition = QueuePosition,
                    SeedIdleLimit = SeedIdleLimit,
                    SeedIdleMode = SeedIdleMode,
                    SeedRatioLimit = SeedRatioLimit,
                    SeedRatioMode = SeedRatioMode,
                    UploadLimit = UploadLimit,
                    UploadLimited = UploadLimited,
                    TrackerAdd = (TrackerAdd ?? new List<string>()).Any()
                        ? TrackerAdd.ToArray()
                        : null,
                    TrackerRemove = (TrackerRemove ?? new List<int>()).Any()
                        ? TrackerRemove.ToArray()
                        : null
                };

                bool success = Task.Run(async () => await torrentSvc.SetTorrents(request)).Result;

                if (AsBool.IsPresent)
                {
                    WriteObject(success);

                    return;
                }

                if (!success)
                    ThrowTerminatingError(new ErrorRecord(new Exception("Set torrent(s) failed"), null, ErrorCategory.OperationStopped, null));

                WriteObject("Set torrent(s) successful");
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to set torrent(s), see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
