using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmission.Core.Enums;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;

namespace Transmission.Torrents
{
    /// <summary>
    /// <para type="synopsis">
    /// Move torrents in the queue.
    /// </para>
    /// <para type="description">
    /// Move torrents in the queue.
    /// </para>
    /// <para type="description">
    /// One of the following parameters needs to be supplied alongside the TorrentIds parameter: Up, Down, Top, Bottom.
    /// </para>
    /// <para type="description">
    /// Calls the 'queue-move-top', 'queue-move-up', 'queue-move-down' and 'queue-move-bottom' endpoints: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
    /// </para>
    /// <example>
    ///     <para>Example 1: Move torrent up the queue</para>
    ///     <code>PS C:\> Move-TransmissionTorrentsQueue -TorrentIds @(1) -Up</code>
    ///     <remarks>Torrent is moved up the queue.</remarks>
    /// </example>
    /// <example>
    ///     <para>Example 2: Move torrents to the bottom of the queue using pipeline</para>
    ///     <code>PS C:\> @(1,2) | Move-TransmissionTorrentsQueue -Bottom</code>
    ///     <remarks>Torrents are moved to the bottom of the queue.</remarks>
    /// </example>
    /// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
    /// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Move, "TransmissionTorrentsQueue", HelpUri = "https://github.com/trossr32/ps-transmission")]
    public class MoveTransmissionTorrentsQueueCmdlet : BaseTransmissionCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Array of torrent ids.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public List<int> TorrentIds { get; set; }

        /// <summary>
        /// <para type="description">
        /// If supplied the torrent(s) will be moved up the queue.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter Up { get; set; }

        /// <summary>
        /// <para type="description">
        /// If supplied the torrent(s) will be moved down the queue.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter Down { get; set; }

        /// <summary>
        /// <para type="description">
        /// If supplied the torrent(s) will be moved to the top of the queue.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter Top { get; set; }

        /// <summary>
        /// <para type="description">
        /// If supplied the torrent(s) will be moved to the bottom of the queue.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter Bottom { get; set; }

        private List<int> _torrentIds;

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="MoveTransmissionTorrentsQueueCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            _torrentIds = new List<int>();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="MoveTransmissionTorrentsQueueCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            if (TorrentIds != null)
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
                // validate queue move parameter has been supplied
                if (!Up.IsPresent && !Down.IsPresent && !Top.IsPresent && !Bottom.IsPresent)
                    ThrowTerminatingError(new ErrorRecord(new Exception("One of the following parameters must be supplied: Up, Down, Top, Bottom"), null, ErrorCategory.InvalidArgument, null));

                // validate a torrent id has been supplied
                if (!(TorrentIds ?? new List<int>()).Any())
                    ThrowTerminatingError(new ErrorRecord(new Exception("The TorrentIds parameter must be supplied."), null, ErrorCategory.InvalidArgument, null));

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
