using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Services.Transmission;

namespace TransmissionManager.Torrents
{
    [Cmdlet(VerbsLifecycle.Assert, "TransmissionTorrentsVerified", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class AssertTransmissionTorrentsVerifiedCmdlet : Cmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Array of torrent ids.")]
        public List<int> TorrentIds { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "If supplied the response will be output as a boolean, otherwise a success message or terminating error will be output.")]
        public SwitchParameter AsBool { get; set; }

        private List<int> _torrentIds;

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="AssertTransmissionTorrentsVerifiedCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            // validate a torrent id has been supplied
            if (!(TorrentIds ?? new List<int>()).Any())
                ThrowTerminatingError(new ErrorRecord(new Exception("The TorrentIds parameter must be supplied."), null, ErrorCategory.InvalidArgument, null));

            _torrentIds = new List<int>();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="AssertTransmissionTorrentsVerifiedCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
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
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to verify torrent(s), see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
