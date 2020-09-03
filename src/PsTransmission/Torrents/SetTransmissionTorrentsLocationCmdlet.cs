using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;

namespace Transmission.Torrents
{
    [Cmdlet(VerbsCommon.Set, "TransmissionTorrentsLocation", HelpUri = "https://github.com/trossr32/ps-transmission-manager")]
    public class SetTransmissionTorrentsLocationCmdlet : BaseTransmissionCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public List<int> TorrentIds { get; set; }
        
        [Parameter(Mandatory = true)]
        public string Location { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Move { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter AsBool { get; set; }

        private List<int> _torrentIds;

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="SetTransmissionTorrentsLocationCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            _torrentIds = new List<int>();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="SetTransmissionTorrentsLocationCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            if (TorrentIds != null)
                _torrentIds.AddRange(TorrentIds);
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="SetTransmissionTorrentsLocationCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            try
            {
                // validate a torrent id has been supplied
                if (!(TorrentIds ?? new List<int>()).Any())
                    ThrowTerminatingError(new ErrorRecord(new Exception("The TorrentIds parameter must be supplied."), null, ErrorCategory.InvalidArgument, null));

                var torrentSvc = new TorrentService();
                
                bool success = Task.Run(async () => await torrentSvc.SetTorrentsLocation(_torrentIds, Location, Move.IsPresent)).Result;

                if (AsBool.IsPresent)
                {
                    WriteObject(success);

                    return;
                }

                if (!success)
                    ThrowTerminatingError(new ErrorRecord(new Exception("Set torrent(s) location failed"), null, ErrorCategory.OperationStopped, null));

                WriteObject("Set torrent(s) location successful");
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to set torrent(s) location, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
