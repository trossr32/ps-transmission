using System.Management.Automation;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;

namespace Transmission.Torrents;

/// <summary>
/// <para type="synopsis">
/// Stop torrents.
/// </para>
/// <para type="description">
/// Stop torrents. The All, Completed and Incomplete switch parameters will return all torrents, torrents that have finished downloading or are still downloading (based on download percent).
/// </para>
/// <para type="description">
/// Calls the 'torrent-stop' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
/// </para>
/// <example>
///     <para>Example 1: Stop all torrents</para>
///     <code>PS C:\> Stop-TransmissionTorrents -All</code>
///     <remarks>Stops all torrents.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Stop all completed torrents</para>
///     <code>PS C:\> Stop-TransmissionTorrents -Completed</code>
///     <remarks>Stops all completed torrents.</remarks>
/// </example>
/// <example>
///     <para>Example 3: Stop torrents by id using the pipeline</para>
///     <code>PS C:\> @(1,2) | Stop-TransmissionTorrents</code>
///     <remarks>Stops all torrents with ids 1 and 2.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
/// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
/// </summary>
[Cmdlet(VerbsLifecycle.Stop, "TransmissionTorrents", HelpUri = "https://github.com/trossr32/ps-transmission")]
public class StopTransmissionTorrentsCmdlet : BaseTransmissionCmdlet
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
    /// If supplied all torrents will be stopped.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter All { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied all completed torrents will be stopped.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Completed { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied all in progress torrents will be stopped.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Incomplete { get; set; }

    private List<int> _torrentIds;

    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="StopTransmissionTorrentsCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing()
    {
        base.BeginProcessing();

        _torrentIds = [];
    }

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="StopTransmissionTorrentsCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
        if (TorrentIds != null)
            _torrentIds.AddRange(TorrentIds);
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="StopTransmissionTorrentsCmdlet"/>.
    /// Retrieve all torrents
    /// </summary>
    protected override void EndProcessing()
    {
        try
        {
            if (!All.IsPresent && !Completed.IsPresent && !Incomplete.IsPresent && (TorrentIds ?? []).Count == 0)
                ThrowTerminatingError(new ErrorRecord(new Exception("One of the following parameters must be supplied: All, Completed, Incomplete, TorrentIds"), null, ErrorCategory.InvalidArgument, null));

            var torrentSvc = new TorrentService();

            (bool success, int torrentCount) response;
            var info = "";

            if (All.IsPresent)
                response = Task.Run(async () => await torrentSvc.StopTorrents()).Result;
            else if (Completed.IsPresent)
            {
                response = Task.Run(torrentSvc.StopCompletedTorrents).Result;

                info = "completed ";
            }
            else if (Incomplete.IsPresent)
            {
                response = Task.Run(torrentSvc.StopIncompleteTorrents).Result;

                info = "incomplete ";
            }
            else
                response = Task.Run(async () => await torrentSvc.StopTorrents(_torrentIds)).Result;

            if (!response.success)
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to stop torrents"), null, ErrorCategory.OperationStopped, null));

            if (response.torrentCount == 0)
                WriteWarning("No torrents found.");
            else
                WriteObject($"{response.torrentCount} {info} torrent{(response.torrentCount > 1 ? "s" : "")} stopped.");
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to stop torrents with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }
}