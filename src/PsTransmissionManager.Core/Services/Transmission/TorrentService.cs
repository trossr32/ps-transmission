using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Components;
using PsTransmissionManager.Core.Enums;
using Transmission.NetCore.Client;
using Transmission.NetCore.Client.Models;

namespace PsTransmissionManager.Core.Services.Transmission
{
    public class TorrentService
    {
        private readonly TransmissionClient _client;

        public TorrentService()
        {
            _client = new TransmissionClient(TransmissionContext.Credentials?.Host, null, TransmissionContext.Credentials?.User, TransmissionContext.Credentials?.Password);
        }

        /// <summary>
        /// Retrieve all torrents.
        /// </summary>
        /// <returns></returns>
        public async Task<Torrent[]> GetTorrents(List<int> torrentIds = null)
        {
            return torrentIds != null
                ? (await _client.TorrentGetAsync(TorrentFields.AllFields, torrentIds.ToArray()))?.TorrentList
                : (await _client.TorrentGetAsync(TorrentFields.AllFields))?.TorrentList;
        }

        /// <summary>
        /// Get completed torrents.
        /// </summary>
        /// <returns></returns>
        public async Task<Torrent[]> GetCompletedTorrents()
        {
            return (await GetTorrents())
                .Where(t => t.PercentDone == 1.0)
                .ToArray();
        }

        /// <summary>
        /// Get incomplete torrents.
        /// </summary>
        /// <returns></returns>
        public async Task<Torrent[]> GetIncompleteTorrents()
        {
            return (await GetTorrents())
                .Where(t => t.PercentDone < 1.0)
                .ToArray();
        }

        /// <summary>
        /// Stop torrents. If the supplied list of ids is null, all torrents will be stopped.
        /// </summary>
        /// <param name="torrentIds"></param>
        /// <returns></returns>
        public async Task<(bool success, int torrentCount)> StopTorrents(List<int> torrentIds = null)
        {
            torrentIds ??= (await GetTorrents()).Select(t => t.Id).ToList();

            var success = true;

            if (torrentIds.Count > 0)
                success = await _client.TorrentStopAsync(torrentIds.ToArray());

            return (success, torrentCount: torrentIds.Count);
        }

        /// <summary>
        /// Stop all completed torrents.
        /// </summary>
        /// <returns></returns>
        public async Task<(bool success, int torrentCount)> StopCompletedTorrents()
        {
            var torrentIds = (await GetTorrents())
                .Where(t => t.PercentDone == 1.0)
                .Select(t => t.Id)
                .ToList();

            return await StopTorrents(torrentIds);
        }

        /// <summary>
        /// Stop all incomplete torrents.
        /// </summary>
        /// <returns></returns>
        public async Task<(bool success, int torrentCount)> StopIncompleteTorrents()
        {
            var torrentIds = (await GetTorrents())
                .Where(t => t.PercentDone < 1.0)
                .Select(t => t.Id)
                .ToList();

            return await StopTorrents(torrentIds);
        }

        /// <summary>
        /// Start torrents. If the supplied list of ids is null, all torrents will be started.
        /// </summary>
        /// <param name="torrentIds"></param>
        /// <returns></returns>
        public async Task<(bool success, int torrentCount)> StartTorrents(List<int> torrentIds = null)
        {
            torrentIds ??= (await GetTorrents()).Select(t => t.Id).ToList();

            var success = true;

            if (torrentIds.Count > 0)
                success = await _client.TorrentStartAsync(torrentIds.ToArray());

            return (success, torrentCount: torrentIds.Count);
        }

        /// <summary>
        /// Start all completed torrents.
        /// </summary>
        /// <returns></returns>
        public async Task<(bool success, int torrentCount)> StartCompletedTorrents()
        {
            var torrentIds = (await GetTorrents())
                .Where(t => t.PercentDone == 1.0)
                .Select(t => t.Id)
                .ToList();

            return await StartTorrents(torrentIds);
        }

        /// <summary>
        /// Start all incomplete torrents.
        /// </summary>
        /// <returns></returns>
        public async Task<(bool success, int torrentCount)> StartIncompleteTorrents()
        {
            var torrentIds = (await GetTorrents())
                .Where(t => t.PercentDone < 1.0)
                .Select(t => t.Id)
                .ToList();

            return await StartTorrents(torrentIds);
        }

        /// <summary>
        /// Start torrents now. If the supplied list of ids is null, all torrents will be started. <br />
        /// It appears start now differs from the start method by forcing the download to start even if it queued. Currently unconfirmed.
        /// </summary>
        /// <param name="torrentIds"></param>
        /// <returns></returns>
        public async Task<(bool success, int torrentCount)> StartTorrentsNow(List<int> torrentIds = null)
        {
            torrentIds ??= (await GetTorrents()).Select(t => t.Id).ToList();

            var success = true;

            if (torrentIds.Count > 0)
                success = await _client.TorrentStartNowAsync(torrentIds.ToArray());

            return (success, torrentCount: torrentIds.Count);
        }

        /// <summary>
        /// Start all completed torrents now. <br />
        /// It appears start now differs from the start method by forcing the download to start even if it queued. Currently unconfirmed.
        /// </summary>
        /// <returns></returns>
        public async Task<(bool success, int torrentCount)> StartCompletedTorrentsNow()
        {
            var torrentIds = (await GetTorrents())
                .Where(t => t.PercentDone == 1.0)
                .Select(t => t.Id)
                .ToList();

            return await StartTorrentsNow(torrentIds);
        }

        /// <summary>
        /// Start all incomplete torrents now. <br />
        /// It appears start now differs from the start method by forcing the download to start even if it queued. Currently unconfirmed.
        /// </summary>
        /// <returns></returns>
        public async Task<(bool success, int torrentCount)> StartIncompleteTorrentsNow()
        {
            var torrentIds = (await GetTorrents())
                .Where(t => t.PercentDone < 1.0)
                .Select(t => t.Id)
                .ToList();

            return await StartTorrentsNow(torrentIds);
        }

        /// <summary>
        /// Remove torrents, and optionally delete downloaded data. <br />
        /// If the supplied list of ids is null, all torrents will be removed.
        /// </summary>
        /// <param name="torrentIds"></param>
        /// <param name="deleteData"></param>
        /// <returns></returns>
        public async Task<(bool success, int torrentCount)> RemoveTorrents(List<int> torrentIds = null, bool deleteData = false)
        {
            torrentIds ??= (await GetTorrents()).Select(t => t.Id).ToList();

            var success = true;

            if (torrentIds.Count > 0)
                success = await _client.TorrentRemoveAsync(torrentIds.ToArray(), deleteData);

            return (success, torrentCount: torrentIds.Count);
        }

        /// <summary>
        /// Remove all completed torrents, and optionally delete downloaded data.
        /// </summary>
        /// <param name="deleteData"></param>
        /// <returns></returns>
        public async Task<(bool success, int torrentCount)> RemoveCompletedTorrents(bool deleteData = false)
        {
            var torrentIds = (await GetTorrents())
                .Where(t => t.PercentDone == 1.0)
                .Select(t => t.Id)
                .ToList();

            return await RemoveTorrents(torrentIds, deleteData);
        }

        /// <summary>
        /// Remove all completed torrents, and optionally delete downloaded data.
        /// </summary>
        /// <param name="deleteData"></param>
        /// <returns></returns>
        public async Task<(bool success, int torrentCount)> RemoveIncompleteTorrents(bool deleteData = false)
        {
            var torrentIds = (await GetTorrents())
                .Where(t => t.PercentDone < 1.0)
                .Select(t => t.Id)
                .ToList();

            return await RemoveTorrents(torrentIds, deleteData);
        }

        /// <summary>
        /// Move torrents in queue.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> MoveTorrents(MoveInQueue moveInQueue, List<int> torrentIds)
        {
            return moveInQueue switch
            {
                MoveInQueue.Up => await _client.TorrentQueueMoveUpAsync(torrentIds.ToArray()),
                MoveInQueue.Down => await _client.TorrentQueueMoveDownAsync(torrentIds.ToArray()),
                MoveInQueue.Top => await _client.TorrentQueueMoveTopAsync(torrentIds.ToArray()),
                MoveInQueue.Bottom => await _client.TorrentQueueMoveBottomAsync(torrentIds.ToArray()),
                _ => throw new ArgumentOutOfRangeException(nameof(moveInQueue), moveInQueue, null)
            };
        }

        /// <summary>
        /// Verify torrents.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> VerifyTorrents(List<int> torrentIds)
        {
            return await _client.TorrentVerifyAsync(torrentIds.ToArray());
        }

        /// <summary>
        /// Re-announce torrents <br />
        /// ("ask tracker for more peers")
        /// </summary>
        /// <param name="torrentIds"></param>
        /// <returns></returns>
        public async Task<bool> ReannounceTorrents(List<int> torrentIds)
        {
            return await _client.TorrentReannounceAsync(torrentIds.ToArray());
        }

        /// <summary>
        /// Rename a file or directory in a torrent
        /// </summary>
        /// <param name="torrentId">The torrent whose path will be renamed</param>
        /// <param name="path">The path to the file or folder that will be renamed</param>
        /// <param name="name">The file or folder's new name</param>
        public async Task<RenamedTorrent> RenameTorrentPath(int torrentId, string path, string name)
        {
            return await _client.TorrentRenamePathAsync(torrentId, path, name);
        }

        /// <summary>
        /// Set new location for torrents files (API: torrent-set-location)
        /// </summary>
        /// <param name="torrentIds">Torrent ids</param>
        /// <param name="location">The new torrent location</param>
        /// <param name="move">Move from previous location</param>
        public async Task<bool> SetTorrentsLocation(List<int> torrentIds, string location, bool move)
        {
            return await _client.TorrentSetLocationAsync(torrentIds.ToArray(), location, move);
        }

        /// <summary>
        /// Set torrent params
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> SetTorrents(TorrentSettings request)
        {
            return await _client.TorrentSetAsync(request);
        }

        /// <summary>
        /// Add multiple torrents
        /// </summary>
        /// <param name="requests"></param>
        /// <returns>A list of successful and failed results</returns>
        public async Task<AddTorrentsResponse> AddTorrents(List<NewTorrent> requests)
        {
            return await _client.TorrentsAddAsync(requests);
        }
    }
}
