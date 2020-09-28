using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;
using Transmission.NetCore.Client.Models;

namespace Transmission.Session
{
    /// <summary>
    /// <para type="synopsis">
    /// Set Transmission session configuration.
    /// </para>
    /// <para type="description">
    /// Set Transmission session configuration.
    /// </para>
    /// <para type="description">
    /// Calls the 'session-set' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
    /// </para>
    /// <example>
    ///     <para>Example 1: Enable the alt speed limit</para>
    ///     <code>PS C:\> Set-TransmissionSession -AlternativeSpeedEnabled $true</code>
    ///     <remarks>Enable the alt speed limit.</remarks>
    /// </example>
    /// <example>
    ///     <para>Example 2: Enable the alt speed scheduler, set scheduled time as 22:00 to 07:00 and download speed as 3Mb</para>
    ///     <code>PS C:\> Set-TransmissionSession -AlternativeSpeedTimeEnabled $true -AlternativeSpeedTimeBegin 1320 -AlternativeSpeedTimeEnd 420 -AlternativeSpeedDown 3000</code>
    ///     <remarks>Enable the alt speed scheduler, set scheduled time as 22:00 to 07:00 and download speed as 3Mb.</remarks>
    /// </example>
    /// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
    /// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "TransmissionSession", HelpUri = "https://github.com/trossr32/ps-transmission")]
    [OutputType(typeof(SessionInformation))]
    public class SetTransmissionSessionCmdlet : BaseTransmissionCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// Max global download speed (KBps)
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? AlternativeSpeedDown { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means use the alt speeds
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? AlternativeSpeedEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// When to turn on alt speeds (units: minutes after midnight)
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? AlternativeSpeedTimeBegin { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means the scheduled on/off times are used
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? AlternativeSpeedTimeEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// When to turn off alt speeds (units: minutes after midnight)
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? AlternativeSpeedTimeEnd { get; set; }

        /// <summary>
        /// <para type="description">
        /// What day(s) to turn on alt speeds
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? AlternativeSpeedTimeDay { get; set; }

        /// <summary>
        /// <para type="description">
        /// Max global upload speed (KBps)
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? AlternativeSpeedUp { get; set; }

        /// <summary>
        /// <para type="description">
        /// Location of the blocklist to use for "blocklist-update"
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string BlockListUrl { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means enabled
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? BlockListEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// Maximum size of the disk cache (MB)
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? CacheSizeMb { get; set; }

        /// <summary>
        /// <para type="description">
        /// Default path to download torrents
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string DownloadDirectory { get; set; }

        /// <summary>
        /// <para type="description">
        /// Max number of torrents to download at once (see download-queue-enabled)
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? DownloadQueueSize { get; set; }

        /// <summary>
        /// <para type="description">
        /// If true, limit how many torrents can be downloaded at once
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? DownloadQueueEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means allow dht in public torrents
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? DhtEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// "required", "preferred", "tolerated"
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Encryption { get; set; }

        /// <summary>
        /// <para type="description">
        /// Torrents we're seeding will be stopped if they're idle for this long
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? IdleSeedingLimit { get; set; }

        /// <summary>
        /// <para type="description">
        /// True if the seeding inactivity limit is honored by default
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? IdleSeedingLimitEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// Path for incomplete torrents, when enabled
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string IncompleteDirectory { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means keep torrents in incomplete-dir until done
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? IncompleteDirectoryEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means allow Local Peer Discovery in public torrents
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? LpdEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// Maximum global number of peers
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? PeerLimitGlobal { get; set; }

        /// <summary>
        /// <para type="description">
        /// Maximum global number of peers
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? PeerLimitPerTorrent { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means allow pex in public torrents
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? PexEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// Port number
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? PeerPort { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means pick a random peer port on launch
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? PeerPortRandomOnStart { get; set; }

        /// <summary>
        /// <para type="description">
        /// true means enabled
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? PortForwardingEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// Whether or not to consider idle torrents as stalled
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? QueueStalledEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// Torrents that are idle for N minuets aren't counted toward seed-queue-size or download-queue-size
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? QueueStalledMinutes { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means append ".part" to incomplete files
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? RenamePartialFiles { get; set; }

        /// <summary>
        /// <para type="description">
        /// Filename of the script to run
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string ScriptTorrentDoneFilename { get; set; }

        /// <summary>
        /// <para type="description">
        /// Whether or not to call the "done" script
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? ScriptTorrentDoneEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// The default seed ratio for torrents to use
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public double? SeedRatioLimit { get; set; }

        /// <summary>
        /// <para type="description">
        /// True if seedRatioLimit is honored by default
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? SeedRatioLimited { get; set; }

        /// <summary>
        /// <para type="description">
        /// Max number of torrents to uploaded at once (see seed-queue-enabled)
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? SeedQueueSize { get; set; }

        /// <summary>
        /// <para type="description">
        /// If true, limit how many torrents can be uploaded at once
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? SeedQueueEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// Max global download speed (KBps)
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? SpeedLimitDown { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means enabled
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? SpeedLimitDownEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        ///  max global upload speed (KBps)
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public int? SpeedLimitUp { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means enabled
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? SpeedLimitUpEnabled { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means added torrents will be started right away
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? StartAddedTorrents { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means the .torrent file of added torrents will be deleted
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? TrashOriginalTorrentFiles { get; set; }

        /// <summary>
        /// <para type="description">
        /// True means allow utp
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool? UtpEnabled { get; set; }
        
        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="SetTransmissionSessionCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="SetTransmissionSessionCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                // validate at least one parameter has been supplied
                if (!AtLeastOneParameterPresent())
                    throw new Exception("At least one parameter with a valid value must be supplied");

                // if the encryption parameter has been supplied validate the value
                if (!string.IsNullOrWhiteSpace(Encryption) && !(new[]{ "required", "preferred", "tolerated" }.Any(e => e == Encryption.ToLower())))
                    throw new Exception("The encryption parameter has an invalid value. Allowed values are 'required', 'preferred', 'tolerated'.");

                var request = new SessionSettings
                {
                    AlternativeSpeedDown = AlternativeSpeedDown,
                    AlternativeSpeedEnabled = AlternativeSpeedTimeEnabled,
                    AlternativeSpeedTimeBegin = AlternativeSpeedTimeBegin,
                    AlternativeSpeedTimeEnabled = AlternativeSpeedTimeEnabled,
                    AlternativeSpeedTimeEnd = AlternativeSpeedTimeEnd,
                    AlternativeSpeedTimeDay = AlternativeSpeedTimeDay,
                    AlternativeSpeedUp = AlternativeSpeedUp,
                    BlockListUrl = !string.IsNullOrWhiteSpace(BlockListUrl) ? BlockListUrl : null,
                    BlockListEnabled = BlockListEnabled,
                    CacheSizeMb = CacheSizeMb,
                    DownloadDirectory = !string.IsNullOrWhiteSpace(DownloadDirectory) ? DownloadDirectory : null,
                    DownloadQueueSize = DownloadQueueSize,
                    DownloadQueueEnabled = DownloadQueueEnabled,
                    DhtEnabled = DhtEnabled,
                    Encryption = !string.IsNullOrWhiteSpace(Encryption) ? Encryption : null,
                    IdleSeedingLimit = IdleSeedingLimit,
                    IdleSeedingLimitEnabled = IdleSeedingLimitEnabled,
                    IncompleteDirectory = !string.IsNullOrWhiteSpace(IncompleteDirectory) ? IncompleteDirectory : null,
                    IncompleteDirectoryEnabled = IncompleteDirectoryEnabled,
                    LpdEnabled = LpdEnabled,
                    PeerLimitGlobal = PeerLimitGlobal,
                    PeerLimitPerTorrent = PeerLimitPerTorrent,
                    PexEnabled = PexEnabled,
                    PeerPort = PeerPort,
                    PeerPortRandomOnStart = PeerPortRandomOnStart,
                    PortForwardingEnabled = PortForwardingEnabled,
                    QueueStalledEnabled = QueueStalledEnabled,
                    QueueStalledMinutes = QueueStalledMinutes,
                    RenamePartialFiles = RenamePartialFiles,
                    ScriptTorrentDoneFilename = !string.IsNullOrWhiteSpace(ScriptTorrentDoneFilename) ? ScriptTorrentDoneFilename : null,
                    ScriptTorrentDoneEnabled = ScriptTorrentDoneEnabled,
                    SeedRatioLimit = SeedRatioLimit,
                    SeedRatioLimited = SeedRatioLimited,
                    SeedQueueSize = SeedQueueSize,
                    SeedQueueEnabled = SeedQueueEnabled,
                    SpeedLimitDown = SpeedLimitDown,
                    SpeedLimitDownEnabled = SpeedLimitDownEnabled,
                    SpeedLimitUp = SpeedLimitUp,
                    SpeedLimitUpEnabled = SpeedLimitUpEnabled,
                    StartAddedTorrents = StartAddedTorrents,
                    TrashOriginalTorrentFiles = TrashOriginalTorrentFiles,
                    UtpEnabled = UtpEnabled
                };

                var sessionSvc = new SessionService();

                bool success = Task.Run(async () => await sessionSvc.Set(request)).Result;

                if (!success)
                    throw new Exception("Failed to update session");
                
                WriteObject("Session updated successfully");
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception(e.Message, e), null, ErrorCategory.OperationStopped, null));
            }
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="SetTransmissionSessionCmdlet"/>.
        /// Retrieve all torrents
        /// </summary>
        protected override void EndProcessing()
        {
            
        }

        /// <summary>
        /// Check to see if at least one parameter has been supplied
        /// </summary>
        /// <returns></returns>
        private bool AtLeastOneParameterPresent()
        {
            return AlternativeSpeedDown.HasValue ||
                   AlternativeSpeedEnabled.HasValue ||
                   AlternativeSpeedTimeBegin.HasValue ||
                   AlternativeSpeedTimeEnabled.HasValue ||
                   AlternativeSpeedTimeEnd.HasValue ||
                   AlternativeSpeedTimeDay.HasValue ||
                   AlternativeSpeedUp.HasValue ||
                   !string.IsNullOrWhiteSpace(BlockListUrl) ||
                   BlockListEnabled.HasValue ||
                   CacheSizeMb.HasValue ||
                   !string.IsNullOrWhiteSpace(DownloadDirectory) ||
                   DownloadQueueSize.HasValue ||
                   DownloadQueueEnabled.HasValue ||
                   DhtEnabled.HasValue ||
                   !string.IsNullOrWhiteSpace(Encryption) ||
                   IdleSeedingLimit.HasValue ||
                   IdleSeedingLimitEnabled.HasValue ||
                   !string.IsNullOrWhiteSpace(IncompleteDirectory) ||
                   IncompleteDirectoryEnabled.HasValue ||
                   LpdEnabled.HasValue ||
                   PeerLimitGlobal.HasValue ||
                   PeerLimitPerTorrent.HasValue ||
                   PexEnabled.HasValue ||
                   PeerPort.HasValue ||
                   PeerPortRandomOnStart.HasValue ||
                   PortForwardingEnabled.HasValue ||
                   QueueStalledEnabled.HasValue ||
                   QueueStalledMinutes.HasValue ||
                   RenamePartialFiles.HasValue ||
                   !string.IsNullOrWhiteSpace(ScriptTorrentDoneFilename) ||
                   ScriptTorrentDoneEnabled.HasValue ||
                   SeedRatioLimit.HasValue ||
                   SeedRatioLimited.HasValue ||
                   SeedQueueSize.HasValue ||
                   SeedQueueEnabled.HasValue ||
                   SpeedLimitDown.HasValue ||
                   SpeedLimitDownEnabled.HasValue ||
                   SpeedLimitUp.HasValue ||
                   SpeedLimitUpEnabled.HasValue ||
                   StartAddedTorrents.HasValue ||
                   TrashOriginalTorrentFiles.HasValue ||
                   UtpEnabled.HasValue;
        }
    }
}
