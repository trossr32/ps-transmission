using System;
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
    [Cmdlet(VerbsCommon.Rename, "TransmissionTorrentsPath", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class RenameTransmissionTorrentPathCmdlet : Cmdlet
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

        [Parameter(Mandatory = true)]
        public int TorrentId { get; set; }

        [Parameter(Mandatory = true)]
        public string TorrentPath { get; set; }

        [Parameter(Mandatory = true)]
        public string TorrentName { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Json { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="RenameTransmissionTorrentPathCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {

        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="RenameTransmissionTorrentPathCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {

        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="RenameTransmissionTorrentPathCmdlet"/>.
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
    }
}
