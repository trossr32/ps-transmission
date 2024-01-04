using System.Management.Automation;
using PsTransmission.Core.Services.Transmission;
using Transmission.Base;

namespace Transmission.Torrents;

/// <summary>
/// <para type="synopsis">
/// Start torrents now.
/// </para>
/// <para type="description">
/// Start torrents now. 'start now' differs from 'start' because it forces torrents to start even if they're queued.
/// </para>
/// <para type="description">
/// The All, Completed and Incomplete switch parameters will return all torrents, torrents that have finished
/// downloading or are still downloading (based on download percent).
/// </para>
/// <para type="description">
/// Calls the 'torrent-start-now' endpoint: https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt
/// </para>
/// <example>
///     <para>Example 1: Start all torrents now</para>
///     <code>PS C:\> Start-TransmissionTorrentsNow -All</code>
///     <remarks>Starts all torrents now.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Start all completed torrents now</para>
///     <code>PS C:\> Start-TransmissionTorrentsNow -Completed</code>
///     <remarks>Starts all completed torrents now.</remarks>
/// </example>
/// <example>
///     <para>Example 3: Start torrents now by id using the pipeline</para>
///     <code>PS C:\> @(1,2) | Start-TransmissionTorrentsNow</code>
///     <remarks>Starts all torrents with ids 1 and 2 now.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
/// <para type="link" uri="(https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt)">[Transmission RPC API]</para>
/// </summary>
[Cmdlet(VerbsLifecycle.Start, "TransmissionTorrentsNow", HelpUri = "https://github.com/trossr32/ps-transmission")]
public class StartTransmissionTorrentsNowCmdlet : BaseTransmissionCmdlet
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
    /// If supplied all torrents will be started.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter All { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied all completed torrents will be started.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Completed { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied all in progress torrents will be started.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Incomplete { get; set; }

    private List<int> _torrentIds;

    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="StartTransmissionTorrentsNowCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing()
    {
        base.BeginProcessing();

        _torrentIds = [];
    }

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="StartTransmissionTorrentsNowCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
        if (TorrentIds != null)
            _torrentIds.AddRange(TorrentIds);
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="StartTransmissionTorrentsNowCmdlet"/>.
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
                response = Task.Run(() => torrentSvc.StartTorrentsNow()).Result;
            else if (Completed.IsPresent)
            {
                response = Task.Run(torrentSvc.StartCompletedTorrentsNow).Result;

                info = "completed ";
            }
            else if (Incomplete.IsPresent)
            {
                response = Task.Run(torrentSvc.StartIncompleteTorrentsNow).Result;

                info = "incomplete ";
            }
            else
                response = Task.Run(async () => await torrentSvc.StartTorrentsNow(_torrentIds)).Result;

            if (!response.success)
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to start torrents using 'start now'"), null, ErrorCategory.OperationStopped, null));

            if (response.torrentCount == 0)
                WriteWarning("No torrents found.");
            else
                WriteObject($"{response.torrentCount} {info} torrent{(response.torrentCount > 1 ? "s" : "")} started using 'start now'.");
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to start torrents using 'start now' with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }
}