using System;
using System.Management.Automation;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;
using Transmission.NetCore.Client.Models;

namespace Transmission.Torrents
{
    [Cmdlet(VerbsCommon.Rename, "TransmissionTorrentsPath", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    [OutputType(typeof(RenamedTorrent))]
    public class RenameTransmissionTorrentPathCmdlet : BaseTransmissionCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public int TorrentId { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string TorrentPath { get; set; }

        [Parameter(Mandatory = true, Position = 2)]
        public string TorrentName { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Json { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="RenameTransmissionTorrentPathCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="RenameTransmissionTorrentPathCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                var torrentSvc = new TorrentService();

                RenamedTorrent renamedTorrents = Task.Run(async () => await torrentSvc.RenameTorrentPath(TorrentId, TorrentPath, TorrentName)).Result;

                if (renamedTorrents == null)
                    ThrowTerminatingError(new ErrorRecord(new Exception("Failed to rename torrent path"), null, ErrorCategory.OperationStopped, null));

                if (Json)
                    WriteObject(JsonConvert.SerializeObject(renamedTorrents));
                else
                    WriteObject(renamedTorrents);
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to rename torrent path, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="RenameTransmissionTorrentPathCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            
        }
    }
}
