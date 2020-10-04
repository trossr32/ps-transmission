using System;
using System.Management.Automation;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;
using Transmission.NetCore.Client.Models;

namespace Transmission.Torrents
{
    /// <summary>
    /// <para type="synopsis">
    /// Rename a torrent's path.
    /// </para>
    /// <para type="description">
    /// Rename a torrent's path.
    /// </para>
    /// <para type="description">
    /// For more information on the use of this function, see the transmission.h documentation of tr_torrentRenamePath().
    /// In particular, note that if this call succeeds you'll want to update the torrent's "files" and "name" field with torrent-get.
    /// </para>
    /// <para type="description">
    /// Calls the 'torrent-rename-path' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
    /// </para>
    /// <example>
    ///     <para>Example 1: Rename torrent path</para>
    ///     <code>PS C:\> Rename-TransmissionTorrentsPath -TorrentId 1 -TorrentPath "/some_path" -TorrentName "some_name"</code>
    ///     <remarks>Rename torrent path.</remarks>
    /// </example>
    /// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
    /// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Rename, "TransmissionTorrentsPath", HelpUri = "https://github.com/trossr32/ps-transmission")]
    [OutputType(typeof(RenamedTorrent))]
    public class RenameTransmissionTorrentPathCmdlet : BaseTransmissionCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Torrent id.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        public int TorrentId { get; set; }

        /// <summary>
        /// <para type="description">
        /// Torrent path.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 1)]
        public string TorrentPath { get; set; }

        /// <summary>
        /// <para type="description">
        /// Torrent name.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 2)]
        public string TorrentName { get; set; }

        /// <summary>
        /// <para type="description">
        /// If supplied the data will be output as a JSON string.
        /// </para>
        /// </summary>
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
                ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to rename torrent path with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
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
