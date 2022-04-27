using System.Management.Automation;
using Newtonsoft.Json;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;
using Transmission.NetCore.Client.Models;

namespace Transmission.Torrents;

/// <summary>
/// <para type="synopsis">
/// Get torrents.
/// </para>
/// <para type="description">
/// Get torrents. If no torrent ids are supplied then all torrents will be returned. Alternatively the Completed and
/// Incomplete switch parameters will return torrents that have finished downloading or are still downloading (based on download percent).
/// </para>
/// <para type="description">
/// Calls the 'torrent-get' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
/// </para>
/// <example>
///     <para>Example 1: Get all torrents</para>
///     <code>PS C:\> Get-TransmissionTorrents</code>
///     <remarks>Retrieves all torrents.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Get all completed torrents and return as JSON</para>
///     <code>PS C:\> Get-TransmissionTorrents -Completed -Json</code>
///     <remarks>Retrieves all completed torrents and returns as JSON.</remarks>
/// </example>
/// <example>
///     <para>Example 3: Get torrents by id using the pipeline</para>
///     <code>PS C:\> @(1,2) | Get-TransmissionTorrents</code>
///     <remarks>Retrieves all torrents with ids 1 and 2.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
/// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
/// </summary>
[Cmdlet(VerbsCommon.Get, "TransmissionTorrents", HelpUri = "https://github.com/trossr32/ps-transmission")]
[OutputType(typeof(Torrent[]))]
public class GetTransmissionTorrentsCmdlet : BaseTransmissionCmdlet
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
    /// If supplied all completed torrents will be retrieved.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Completed { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied all in progress torrents will be retrieved.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Incomplete { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied the data will be output as a JSON string.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Json { get; set; }

    private List<int> _torrentIds;
        
    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="GetTransmissionTorrentsCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing()
    {
        base.BeginProcessing();

        _torrentIds = new List<int>();
    }

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="GetTransmissionTorrentsCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
        if (TorrentIds != null)
            _torrentIds.AddRange(TorrentIds);
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="GetTransmissionTorrentsCmdlet"/>.
    /// Retrieve all torrents
    /// </summary>
    protected override void EndProcessing()
    {
        try
        {
            var torrentSvc = new TorrentService();
            
            Torrent[] torrents;

            if (Completed.IsPresent)
                torrents = Task.Run(async () => await torrentSvc.GetCompletedTorrents()).Result;
            else if (Incomplete.IsPresent)
                torrents = Task.Run(async () => await torrentSvc.GetIncompleteTorrents()).Result;
            else if (_torrentIds.Any())
                torrents = Task.Run(async () => await torrentSvc.GetTorrents(_torrentIds)).Result;
            else
                torrents = Task.Run(async () => await torrentSvc.GetTorrents()).Result;

            if (Json)
                WriteObject(JsonConvert.SerializeObject(torrents));
            else
                WriteObject(torrents);
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to retrieve torrents with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }
}