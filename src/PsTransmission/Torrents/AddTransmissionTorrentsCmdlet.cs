using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;
using Transmission.NetCore.Client.Models;

namespace Transmission.Torrents
{
    [Cmdlet(VerbsCommon.Add, "TransmissionTorrents", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    [OutputType(typeof(AddTorrentsResponse))]
    public class AddTransmissionTorrentsCmdlet : BaseTransmissionCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public List<string> Urls { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, Position = 1)]
        public List<string> Files { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, Position = 2)]
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

        [Parameter(Mandatory = false)]
        public SwitchParameter Json { get; set; }

        private List<string> _urls;
        private List<string> _files;
        private List<string> _metas;

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="AddTransmissionTorrentsCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            _urls = new List<string>();
            _files = new List<string>();
            _metas = new List<string>();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="AddTransmissionTorrentsCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            if (Urls != null)
                _urls.AddRange(Urls);

            if (Files != null)
                _files.AddRange(Files);

            if (MetaInfos != null)
                _metas.AddRange(MetaInfos);
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="AddTransmissionTorrentsCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            try
            {
                // build an array of all supplied torrents to validate against
                var allTorrents = _urls
                    .Union(_files)
                    .Union(_metas)
                    .ToList();

                // validate files, urls or metainfos have been supplied
                if (!allTorrents.Any())
                    ThrowTerminatingError(new ErrorRecord(new Exception("At least one of the following parameters must be supplied: Urls, Files, MetaInfos"), null, ErrorCategory.InvalidArgument, null));

                // validate FilesWanted, FilesUnwanted, PriorityHigh, PriorityLow & PriorityNormal haven't been supplied if multiple torrents are to be added
                if (allTorrents.Count > 1 &&
                    (FilesWanted ?? new List<int>()).Any() ||
                    (FilesUnwanted ?? new List<int>()).Any() ||
                    (PriorityHigh ?? new List<int>()).Any() ||
                    (PriorityLow ?? new List<int>()).Any() ||
                    (PriorityNormal ?? new List<int>()).Any())
                    ThrowTerminatingError(new ErrorRecord(new Exception("If multiple torrents are supplied then none of the following parameters should be supplied: FilesWanted, FilesUnwanted, PriorityHigh, PriorityLow, PriorityNormal"), null, ErrorCategory.InvalidArgument, null));

                var torrentSvc = new TorrentService();

                List<NewTorrent> newTorrents = _urls
                    .Union(_files)
                    .Select(t => new NewTorrent
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

                newTorrents.AddRange(_metas.Select(m => new NewTorrent
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
                    })
                    .ToList());

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
