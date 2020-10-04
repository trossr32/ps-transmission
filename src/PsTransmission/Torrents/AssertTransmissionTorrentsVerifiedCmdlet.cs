using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;

namespace Transmission.Torrents
{
    /// <summary>
    /// <para type="synopsis">
    /// Verify torrents.
    /// </para>
    /// <para type="description">
    /// Verify torrents.
    /// </para>
    /// <para type="description">
    /// Calls the 'torrent-verify' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
    /// </para>
    /// <example>
    ///     <para>Example 1: Verify torrents and return a success message or throw a terminating error</para>
    ///     <code>PS C:\> Assert-TransmissionTorrentsVerified -TorrentIds @(1)</code>
    ///     <remarks>Verifies torrents and returns a success message or throws a terminating error.</remarks>
    /// </example>
    /// <example>
    ///     <para>Example 2: Verify torrents using pipeline and return a boolean</para>
    ///     <code>PS C:\> @(1) | Assert-TransmissionTorrentsVerified -AsBool</code>
    ///     <remarks>Verifies torrents and returns a boolean.</remarks>
    /// </example>
    /// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
    /// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
    /// </summary>
    [Cmdlet(VerbsLifecycle.Assert, "TransmissionTorrentsVerified", HelpUri = "https://github.com/trossr32/ps-transmission")]
    public class AssertTransmissionTorrentsVerifiedCmdlet : BaseTransmissionCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Array of torrent ids.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public List<int> TorrentIds { get; set; }

        /// <summary>
        /// <para type="description">
        /// If supplied the response will be a boolean, otherwise a success message or a terminating error.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter AsBool { get; set; }

        private List<int> _torrentIds;

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="AssertTransmissionTorrentsVerifiedCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            _torrentIds = new List<int>();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="AssertTransmissionTorrentsVerifiedCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            if (TorrentIds != null)
                _torrentIds.AddRange(TorrentIds);
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="AssertTransmissionTorrentsVerifiedCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            try
            {
                // validate a torrent id has been supplied
                if (!_torrentIds.Any())
                    ThrowTerminatingError(new ErrorRecord(new Exception("The TorrentIds parameter must be supplied."), null, ErrorCategory.InvalidArgument, null));

                var torrentSvc = new TorrentService();
                
                bool success = Task.Run(async () => await torrentSvc.VerifyTorrents(_torrentIds)).Result;

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
                ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to verify torrent(s) with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
