using System.Management.Automation;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;
using Transmission.NetCore.Client.Models;

namespace Transmission.Torrents;

/// <summary>
/// <para type="synopsis">
/// Update torrent.
/// </para>
/// <para type="description">
/// Update torrent.
/// </para>
/// <para type="description">
/// Calls the 'torrent-set' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
/// </para>
/// <example>
///     <para>Example 1: Update torrent upload limit to 500 KBps</para>
///     <code>PS C:\> Set-TransmissionTorrents -TorrentIds @(1) -UploadLimit 500</code>
///     <remarks>Updates the torrent's upload limit to 500 KBps.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Update torrent upload limit to 500 KBps from pipeline</para>
///     <code>PS C:\> @(1) | Set-TransmissionTorrents -UploadLimit 500</code>
///     <remarks>Updates the torrent's upload limit to 500 KBps.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
/// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
/// </summary>
[Cmdlet(VerbsCommon.Set, "TransmissionTorrents", HelpUri = "https://github.com/trossr32/ps-transmission")]
public class SetTransmissionTorrentsCmdlet : BaseTransmissionCmdlet
{
    /// <summary>
    /// <para type="description">
    /// Array of torrent ids.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
    public List<int> TorrentIds { get; set; }

    /// <summary>
    /// <para type="description">
    /// Torrent's bandwidth priority.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public int? BandwidthPriority { get; set; }

    /// <summary>
    /// <para type="description">
    /// Maximum download speed (KBps)
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public int? DownloadLimit { get; set; }

    /// <summary>
    /// <para type="description">
    /// Download limit is honoured
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public bool? DownloadLimited { get; set; }

    /// <summary>
    /// <para type="description">
    /// Session upload limits are honoured
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public bool? HonoursSessionLimits { get; set; }

    /// <summary>
    /// <para type="description">
    /// New location of the torrent's content
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public string Location { get; set; }

    /// <summary>
    /// <para type="description">
    /// Maximum number of peers
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public int? PeerLimit { get; set; }

    /// <summary>
    /// <para type="description">
    /// Position of this torrent in its queue [0...n)
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public int? QueuePosition { get; set; }

    /// <summary>
    /// <para type="description">
    /// Torrent-level number of minutes of seeding inactivity
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public int? SeedIdleLimit { get; set; }

    /// <summary>
    /// <para type="description">
    /// Which seeding inactivity to use
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public int? SeedIdleMode { get; set; }

    /// <summary>
    /// <para type="description">
    /// Torrent-level seeding ratio
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public double? SeedRatioLimit { get; set; }

    /// <summary>
    /// <para type="description">
    /// Which ratio to use. 
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public int? SeedRatioMode { get; set; }

    /// <summary>
    /// <para type="description">
    /// Maximum upload speed (KBps)
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public int? UploadLimit { get; set; }

    /// <summary>
    /// <para type="description">
    /// Upload limit is honoured
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public bool? UploadLimited { get; set; }

    /// <summary>
    /// <para type="description">
    /// Strings of announce URLs to add
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public List<string> TrackerAdd { get; set; }

    /// <summary>
    /// <para type="description">
    /// Ids of trackers to remove
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public List<int> TrackerRemove { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied the response will be a boolean, otherwise a success message or a terminating error.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter AsBool { get; set; }

    private List<int> _torrentIds;

    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="SetTransmissionTorrentsCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing()
    {
        base.BeginProcessing();

        _torrentIds = new List<int>();
    }

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="SetTransmissionTorrentsCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
        if (TorrentIds != null)
            _torrentIds.AddRange(TorrentIds);
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="SetTransmissionTorrentsCmdlet"/>.
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
                
            var request = new TorrentSettings
            {
                BandwidthPriority = BandwidthPriority,
                DownloadLimit = DownloadLimit,
                DownloadLimited = DownloadLimited,
                HonorsSessionLimits = HonoursSessionLimits,
                Ids = _torrentIds.ToArray(),
                Location = Location,
                PeerLimit = PeerLimit,
                QueuePosition = QueuePosition,
                SeedIdleLimit = SeedIdleLimit,
                SeedIdleMode = SeedIdleMode,
                SeedRatioLimit = SeedRatioLimit,
                SeedRatioMode = SeedRatioMode,
                UploadLimit = UploadLimit,
                UploadLimited = UploadLimited,
                TrackerAdd = (TrackerAdd ?? new List<string>()).Any()
                    ? TrackerAdd.ToArray()
                    : null,
                TrackerRemove = (TrackerRemove ?? new List<int>()).Any()
                    ? TrackerRemove.ToArray()
                    : null
            };

            bool success = Task.Run(async () => await torrentSvc.SetTorrents(request)).Result;

            if (AsBool.IsPresent)
            {
                WriteObject(success);

                return;
            }

            if (!success)
                ThrowTerminatingError(new ErrorRecord(new Exception("Set torrent(s) failed"), null, ErrorCategory.OperationStopped, null));

            WriteObject("Set torrent(s) successful");
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to set torrent(s) with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }
}