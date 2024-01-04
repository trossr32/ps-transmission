using System.Management.Automation;
using Newtonsoft.Json;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;
using Transmission.NetCore.Client.Models;

namespace Transmission.Torrents;

/// <summary>
/// <para type="synopsis">
/// Adds torrents to Transmission.
/// </para>
/// <para type="description">
/// Adds torrents to Transmission. Add via URL (i.e. magnet link), .torrent file or base64 encoded .torrent content.
/// </para>
/// <para type="description">
/// One of the following parameters must be supplied: Urls, Files or MetaInfos.
/// </para>
/// <para type="description">
/// If multiple torrents are supplied, then NONE of the following parameters should be supplied: FilesWanted, FilesUnwanted, PriorityHigh, PriorityLow, PriorityNormal.
/// </para>
/// <para type="description">
/// Calls the 'torrent-add' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
/// </para>
/// <example>
///     <para>Example 1: Add torrent from magnet link</para>
///     <code>PS C:\> Add-TransmissionTorrents -Urls @("magnet:?xt=urn:btih:D540FC48EB12F2833163EED6421D449DD8F1CE1F&amp;dn=Ubuntu+desktop+19.04+%2864bit%29")</code>
///     <remarks>Adds the torrent to Transmission.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Add torrent from pipeline, pause download and get response as JSON</para>
///     <code>PS C:\> @("magnet:?xt=urn:btih:D540FC48EB12F2833163EED6421D449DD8F1CE1F&amp;dn=Ubuntu+desktop+19.04+%2864bit%29") | Add-TransmissionTorrents -Json -Paused $true</code>
///     <remarks>Adds the torrent to Transmission, pauses download and returns response as JSON.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
/// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
/// </summary>
[Cmdlet(VerbsCommon.Add, "TransmissionTorrents", HelpUri = "https://github.com/trossr32/ps-transmission")]
[OutputType(typeof(AddTorrentsResponse))]
public class AddTransmissionTorrentsCmdlet : BaseTransmissionCmdlet
{
    /// <summary>
    /// <para type="description">
    /// Array of URLs.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
    public List<string> Urls { get; set; }

    /// <summary>
    /// <para type="description">
    /// Array of .torrent files.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, Position = 1)]
    public List<string> Files { get; set; }

    /// <summary>
    /// <para type="description">
    /// Array of base64 encoded .torrent content.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, Position = 2)]
    public List<string> MetaInfos { get; set; }

    /// <summary>
    /// <para type="description">
    /// Pointer to a string of one or more cookies. The format should be NAME=CONTENTS, where NAME is the cookie name
    /// and CONTENTS is what the cookie should contain. Set multiple cookies like this: "name1=content1; name2=content2;" etc.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public string Cookies { get; set; }

    /// <summary>
    /// <para type="description">
    /// Path to download the torrent to.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public string DownloadDirectory { get; set; }

    /// <summary>
    /// <para type="description">
    /// If true, don't start the torrent.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public bool Paused { get; set; }

    /// <summary>
    /// <para type="description">
    /// Maximum number of peers.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public int? PeerLimit { get; set; }

    /// <summary>
    /// <para type="description">
    /// Torrent's bandwidth priority.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public int? BandwidthPriority { get; set; }

    /// <summary>
    /// <para type="description">
    /// Indices of file(s) to download.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public List<int> FilesWanted { get; set; }

    /// <summary>
    /// <para type="description">
    /// Indices of file(s) to not download.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public List<int> FilesUnwanted { get; set; }

    /// <summary>
    /// <para type="description">
    /// Indices of high priority file(s).
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public List<int> PriorityHigh { get; set; }

    /// <summary>
    /// <para type="description">
    /// Indices of low priority file(s).
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public List<int> PriorityLow { get; set; }

    /// <summary>
    /// <para type="description">
    /// Indices of normal priority file(s).
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public List<int> PriorityNormal { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied the data will be output as a JSON string.
    /// </para>
    /// </summary>
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

        _urls = [];
        _files = [];
        _metas = [];
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

            // validate files, urls or meta infos have been supplied
            if (allTorrents.Count == 0)
                ThrowTerminatingError(new ErrorRecord(new Exception("At least one of the following parameters must be supplied: Urls, Files, MetaInfos"), null, ErrorCategory.InvalidArgument, null));

            // validate FilesWanted, FilesUnwanted, PriorityHigh, PriorityLow & PriorityNormal haven't been supplied if multiple torrents are to be added
            if (allTorrents.Count > 1 &&
                (FilesWanted ?? []).Count != 0 ||
                (FilesUnwanted ?? []).Count != 0 ||
                (PriorityHigh ?? []).Count != 0 ||
                (PriorityLow ?? []).Count != 0 ||
                (PriorityNormal ?? []).Count != 0)
                ThrowTerminatingError(new ErrorRecord(new Exception("If multiple torrents are supplied then none of the following parameters should be supplied: FilesWanted, FilesUnwanted, PriorityHigh, PriorityLow, PriorityNormal"), null, ErrorCategory.InvalidArgument, null));

            var torrentSvc = new TorrentService();

            var newTorrents = _urls
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
                    FilesWanted = (FilesWanted ?? []).Count != 0 ? [.. FilesWanted] : null,
                    FilesUnwanted = (FilesUnwanted ?? []).Count != 0 ? [.. FilesUnwanted] : null,
                    PriorityHigh = (PriorityHigh ?? []).Count != 0 ? [.. PriorityHigh] : null,
                    PriorityLow = (PriorityLow ?? []).Count != 0 ? [.. PriorityLow] : null,
                    PriorityNormal = (PriorityNormal ?? []).Count != 0 ? [.. PriorityNormal] : null
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
                    FilesWanted = (FilesWanted ?? []).Count != 0 ? [.. FilesWanted] : null,
                    FilesUnwanted = (FilesUnwanted ?? []).Count != 0 ? [.. FilesUnwanted] : null,
                    PriorityHigh = (PriorityHigh ?? []).Count != 0 ? [.. PriorityHigh] : null,
                    PriorityLow = (PriorityLow ?? []).Count != 0 ? [.. PriorityLow] : null,
                    PriorityNormal = (PriorityNormal ?? []).Count != 0 ? [.. PriorityNormal] : null
                })
                .ToList());

            var response = Task.Run(async () => await torrentSvc.AddTorrents(newTorrents)).Result;

            if (Json)
                WriteObject(JsonConvert.SerializeObject(response));
            else
                WriteObject(response);
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to add torrent(s) with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }
}