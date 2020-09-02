using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PsTransmissionManager.Core.Constants;
using PsTransmissionManager.Core.Models;
using PsTransmissionManager.Core.Services;
using PsTransmissionManager.Core.Services.Transmission;
using Transmission.NetCore.Client.Models;

namespace TransmissionManager.Torrents
{
    [Cmdlet(VerbsCommon.Add, "TransmissionTorrents", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class AddTransmissionTorrentsCmdlet : Cmdlet
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
        public SwitchParameter Json { get; set; }

        [Parameter(Mandatory = false)]
        public string Url { get; set; }

        [Parameter(Mandatory = false)]
        public List<string> Urls { get; set; }

        [Parameter(Mandatory = false)]
        public string File { get; set; }

        [Parameter(Mandatory = false)]
        public List<string> Files { get; set; }

        [Parameter(Mandatory = false)]
        public string MetaInfo { get; set; }

        [Parameter(Mandatory = false)]
        public List<string> MetaInfos { get; set; }

        [Parameter(Mandatory = false)]
        public string Cookies { get; set; }

        [Parameter(Mandatory = false)]
        public string DownloadDirectory { get; set; }

        [Parameter(Mandatory = false)]
        public bool Paused { get; set; }

        [Parameter(Mandatory = false)]
        public int? PeerLimit { get; set; }

        [Parameter(Mandatory = false)]
        public int? BandwidthPriority { get; set; }

        [Parameter(Mandatory = false)]
        public List<int> FilesWanted { get; set; }

        [Parameter(Mandatory = false)]
        public List<int> FilesUnwanted { get; set; }

        [Parameter(Mandatory = false)]
        public List<int> PriorityHigh { get; set; }

        [Parameter(Mandatory = false)]
        public List<int> PriorityLow { get; set; }

        [Parameter(Mandatory = false)]
        public List<int> PriorityNormal { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="AddTransmissionTorrentsCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            // build an array of all supplied torrents to validate against
            List<string> allTorrents = new List<string>();

            foreach (var torrents in new List<List<string>>
            {
                Urls ?? new List<string>(),
                Files ?? new List<string>(),
                MetaInfos ?? new List<string>(),
                new List<string> { Url, File, MetaInfo }.Where(t => !string.IsNullOrWhiteSpace(t)).ToList()
            })
            {
                allTorrents.AddRange(torrents);
            }

            // validate files, urls or metainfos have been supplied
            if (!allTorrents.Any())
                ThrowTerminatingError(new ErrorRecord(new Exception("At least one of the following parameters must be supplied: Url, Urls, File, Files, MetaInfo, MetaInfos"), null, ErrorCategory.InvalidArgument, null));

            // validate FilesWanted, FilesUnwanted, PriorityHigh, PriorityLow & PriorityNormal haven't been supplied if multiple torrents are to be added
            if (allTorrents.Count > 1 &&
                (FilesWanted ?? new List<int>()).Any() ||
                (FilesUnwanted ?? new List<int>()).Any() ||
                (PriorityHigh ?? new List<int>()).Any() ||
                (PriorityLow ?? new List<int>()).Any() ||
                (PriorityNormal ?? new List<int>()).Any())
                ThrowTerminatingError(new ErrorRecord(new Exception("If multiple torrents are supplied then none of the following parameters should be supplied: FilesWanted, FilesUnwanted, PriorityHigh, PriorityLow, PriorityNormal"), null, ErrorCategory.InvalidArgument, null));
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="AddTransmissionTorrentsCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {

        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="AddTransmissionTorrentsCmdlet"/>.
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

                var files = new List<string>();

                foreach (var t in new List<List<string>>
                {
                    Urls ?? new List<string>(),
                    Files ?? new List<string>(),
                    new List<string> { Url, File }.Where(t => !string.IsNullOrWhiteSpace(t)).ToList()
                })
                {
                    files.AddRange(t);
                }

                var metaInfos = new List<string>();

                foreach (var t in new List<List<string>>
                {
                    MetaInfos ?? new List<string>(),
                    new List<string> { MetaInfo }.Where(t => !string.IsNullOrWhiteSpace(t)).ToList()
                })
                {
                    metaInfos.AddRange(t);
                }

                List<NewTorrent> newTorrents = files.Select(t => new NewTorrent
                    {
                        Cookies = !string.IsNullOrWhiteSpace(Cookies) ? Cookies : null,
                        DownloadDirectory = !string.IsNullOrWhiteSpace(DownloadDirectory) ? DownloadDirectory : null,
                        Filename = t,
                        MetaInfo = null,
                        Paused = Paused,
                        PeerLimit = PeerLimit,
                        BandwidthPriority = BandwidthPriority,
                        FilesWanted = (FilesWanted ?? new List<int>()).Any() ? FilesWanted.ToArray() : null,
                        FilesUnwanted = (FilesUnwanted ?? new List<int>()).Any() ? FilesUnwanted.ToArray() : null,
                        PriorityHigh = (PriorityHigh ?? new List<int>()).Any() ? PriorityHigh.ToArray() : null,
                        PriorityLow = (PriorityLow ?? new List<int>()).Any() ? PriorityLow.ToArray() : null,
                        PriorityNormal = (PriorityNormal ?? new List<int>()).Any() ? PriorityNormal.ToArray() : null
                    })
                    .ToList();

                newTorrents.AddRange(metaInfos.Select(m => new NewTorrent
                {
                    Cookies = !string.IsNullOrWhiteSpace(Cookies) ? Cookies : null,
                    DownloadDirectory = !string.IsNullOrWhiteSpace(DownloadDirectory) ? DownloadDirectory : null,
                    Filename = null,
                    MetaInfo = m,
                    Paused = Paused,
                    PeerLimit = PeerLimit,
                    BandwidthPriority = BandwidthPriority,
                    FilesWanted = (FilesWanted ?? new List<int>()).Any() ? FilesWanted.ToArray() : null,
                    FilesUnwanted = (FilesUnwanted ?? new List<int>()).Any() ? FilesUnwanted.ToArray() : null,
                    PriorityHigh = (PriorityHigh ?? new List<int>()).Any() ? PriorityHigh.ToArray() : null,
                    PriorityLow = (PriorityLow ?? new List<int>()).Any() ? PriorityLow.ToArray() : null,
                    PriorityNormal = (PriorityNormal ?? new List<int>()).Any() ? PriorityNormal.ToArray() : null
                }));

                AddTorrentsResponse response = Task.Run(async () => await torrentSvc.AddTorrents(newTorrents)).Result;

                if (Json)
                    WriteObject(JsonConvert.SerializeObject(response));
                else
                    WriteObject(response);
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to add torrent(s), see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
