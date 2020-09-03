using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using Transmission.NetCore.Client.Models;

namespace Transmission.NetCore.Client
{
    public class TransmissionClient
    {
        private string Host { get; set; }
        private string SessionId { get; set; }
        private string Login { get; set; }
        private string Password { get; set; }
        private int CurrentTag { get; set; }

        private readonly bool _needAuthorization;

        /// <summary>
		/// Initialize client
		/// </summary>
		/// <param name="host">Host address</param>
		/// <param name="sessionId">Session id</param>
		/// <param name="login">Login</param>
		/// <param name="password">Password</param>
		public TransmissionClient(string host, string sessionId = null, string login = null, string password = null)
        {
            Host = host;
            SessionId = sessionId;
            Login = login;
            Password = password;

            _needAuthorization = !string.IsNullOrWhiteSpace(login);
        }

        #region Session

        /// <summary>
        /// Close current session (API: session-close) <br />
        /// Careful with this one; it essentially shuts transmission down and will need to be restarted to bring it back.
        /// </summary>
        public async Task<bool> SessionCloseAsync()
        {
            var request = new TransmissionRequest("session-close", null);

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Set information to current session (API: session-set)
        /// </summary>
        /// <param name="settings">New session settings</param>
        public async Task<bool> SessionSetAsync(SessionSettings settings)
        {
            var request = new TransmissionRequest("session-set", settings.ToDictionary());

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Get session statistic
        /// </summary>
        /// <returns>Session stat</returns>
        public async Task<Statistic> SessionGetStatisticAsync()
        {
            var request = new TransmissionRequest("session-stats", null);

            TransmissionResponse response = await SendRequestAsync(request);
            
            return response.Deserialize<Statistic>();
        }

        /// <summary>
        /// Get information of current session (API: session-get)
        /// </summary>
        /// <returns>Session information</returns>
        public async Task<SessionInformation> SessionGetAsync()
        {
            var request = new TransmissionRequest("session-get", null);

            TransmissionResponse response = await SendRequestAsync(request);
            
            return response.Deserialize<SessionInformation>();
        }

        #endregion

        #region Torrents

        /// <summary>
        /// Add torrent (API: torrent-add)
        /// </summary>
        /// <param name="torrent"></param>
        /// <returns>Torrent info (ID, Name and HashString)</returns>
        public async Task<CreatedTorrent> TorrentAddAsync(NewTorrent torrent)
        {
            if (string.IsNullOrWhiteSpace(torrent.MetaInfo) && string.IsNullOrWhiteSpace(torrent.Filename))
                throw new Exception("Either \"filename\" or \"metainfo\" must be included.");

            var request = new TransmissionRequest("torrent-add", torrent.ToDictionary());

            TransmissionResponse response = await SendRequestAsync(request);

            JObject jObject = response.Deserialize<JObject>();

            if (jObject?.First == null)
                return null;

            if (jObject.TryGetValue("torrent-duplicate", out JToken value))
                return JsonConvert.DeserializeObject<CreatedTorrent>(value.ToString());

            if (jObject.TryGetValue("torrent-added", out value))
                return JsonConvert.DeserializeObject<CreatedTorrent>(value.ToString());

            return null;
        }

        /// <summary>
        /// Add torrents (API: torrent-add)
        /// </summary>
        /// <param name="torrents"></param>
        /// <returns>Torrent info (ID, Name and HashString)</returns>
        public async Task<AddTorrentsResponse> TorrentsAddAsync(List<NewTorrent> torrents)
        {
            var successes = new List<AddTorrentSuccess>();
            var failures = new List<AddTorrentFailure>();

            foreach (var torrent in torrents)
            {
                try
                {
                    var request = new TransmissionRequest("torrent-add", torrent.ToDictionary());

                    TransmissionResponse response = await SendRequestAsync(request);

                    if (!response.Result.Equals("success", StringComparison.OrdinalIgnoreCase))
                    {
                        failures.Add(new AddTorrentFailure
                        {
                            SubmittedData = torrent.Filename ?? torrent.MetaInfo,
                            SubmittedDataType = !string.IsNullOrEmpty(torrent.Filename) ? "file or url" : "meta info",
                            Error = response.Result
                        });

                        continue;
                    }

                    JObject jObject = response.Deserialize<JObject>();

                    if (jObject?.First == null)
                    {
                        failures.Add(new AddTorrentFailure
                        {
                            SubmittedData = torrent.Filename ?? torrent.MetaInfo,
                            SubmittedDataType = !string.IsNullOrEmpty(torrent.Filename) ? "file or url" : "meta info",
                            Error = "Deserialization error"
                        });

                        continue;
                    }

                    if (jObject.TryGetValue("torrent-duplicate", out JToken value))
                    {
                        successes.Add(new AddTorrentSuccess
                        {
                            SubmittedData = torrent.Filename ?? torrent.MetaInfo,
                            SubmittedDataType = !string.IsNullOrEmpty(torrent.Filename) ? "file or url" : "meta info",
                            TransmissionResponse = JsonConvert.DeserializeObject<CreatedTorrent>(value.ToString())
                        });

                        continue;
                    }

                    if (jObject.TryGetValue("torrent-added", out value))
                        successes.Add(new AddTorrentSuccess
                        {
                            SubmittedData = torrent.Filename ?? torrent.MetaInfo,
                            SubmittedDataType = !string.IsNullOrEmpty(torrent.Filename) ? "file or url" : "meta info",
                            TransmissionResponse = JsonConvert.DeserializeObject<CreatedTorrent>(value.ToString())
                        });
                }
                catch (Exception e)
                {
                    failures.Add(new AddTorrentFailure
                    {
                        SubmittedData = torrent.Filename ?? torrent.MetaInfo,
                        SubmittedDataType = !string.IsNullOrEmpty(torrent.Filename) ? "file or url" : "meta info",
                        Error = $"Unknown error occurred: {e.Message}"
                    });
                }
            }

            return new AddTorrentsResponse
            {
                Successes = successes.Any() ? successes : null,
                Failures = failures.Any() ? failures : null
            };
        }

        /// <summary>
        /// Set torrent params (API: torrent-set)
        /// </summary>
        /// <param name="settings">New torrent params</param>
        public async Task<bool> TorrentSetAsync(TorrentSettings settings)
        {
            var request = new TransmissionRequest("torrent-set", settings.ToDictionary());

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Get fields of torrents from ids (API: torrent-get)
        /// </summary>
        /// <param name="fields">Fields of torrents</param>
        /// <param name="ids">IDs of torrents (null or empty for get all torrents)</param>
        /// <returns>Torrents info</returns>
        public async Task<Torrents> TorrentGetAsync(string[] fields, params int[] ids)
        {
            var arguments = new Dictionary<string, object>
            {
                { "fields", fields }
            };

            if (ids != null && ids.Length > 0)
                arguments.Add("ids", ids);

            var request = new TransmissionRequest("torrent-get", arguments);

            var response = await SendRequestAsync(request);
            
            return response.Deserialize<Torrents>();
        }

        /// <summary>
        /// Remove torrents
        /// </summary>
        /// <param name="ids">Torrents id</param>
        /// <param name="deleteData">Remove local data</param>
        public async Task<bool> TorrentRemoveAsync(int[] ids, bool deleteData = false)
        {
            var arguments = new Dictionary<string, object>
            {
                { "ids", ids },
                { "delete-local-data", deleteData }
            };

            var request = new TransmissionRequest("torrent-remove", arguments);

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Start torrents (API: torrent-start)
        /// </summary>
        /// <param name="ids">Torrents id</param>
        public async Task<bool> TorrentStartAsync(int[] ids)
        {
            var arguments = new Dictionary<string, object>
            {
                { "ids", ids }
            };

            var request = new TransmissionRequest("torrent-start", arguments);

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Start now torrents (API: torrent-start-now)
        /// </summary>
        /// <param name="ids">Torrents id</param>
        public async Task<bool> TorrentStartNowAsync(int[] ids)
        {
            var arguments = new Dictionary<string, object>
            {
                { "ids", ids }
            };

            var request = new TransmissionRequest("torrent-start-now", arguments);

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Stop torrents (API: torrent-stop)
        /// </summary>
        /// <param name="ids">Torrents id</param>
        public async Task<bool> TorrentStopAsync(int[] ids)
        {
            var arguments = new Dictionary<string, object>
            {
                { "ids", ids }
            };

            var request = new TransmissionRequest("torrent-stop", arguments);

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Verify torrents (API: torrent-verify)
        /// </summary>
        /// <param name="ids">Torrent ids</param>
        public async Task<bool> TorrentVerifyAsync(int[] ids)
        {
            var arguments = new Dictionary<string, object>
            {
                { "ids", ids }
            };

            var request = new TransmissionRequest("torrent-verify", arguments);

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Re-announce torrents (API: torrent-reannounce) <br />
        /// ("ask tracker for more peers")
        /// </summary>
        /// <param name="ids">Torrent ids</param>
        public async Task<bool> TorrentReannounceAsync(int[] ids)
        {
            var arguments = new Dictionary<string, object>
            {
                { "ids", ids }
            };

            var request = new TransmissionRequest("torrent-reannounce", arguments);

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Move torrents in queue on top (API: queue-move-top)
        /// </summary>
        /// <param name="ids">Torrents id</param>
        public async Task<bool> TorrentQueueMoveTopAsync(int[] ids)
        {
            var arguments = new Dictionary<string, object>
            {
                { "ids", ids }
            };

            var request = new TransmissionRequest("queue-move-top", arguments);

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Move up torrents in queue (API: queue-move-up)
        /// </summary>
        /// <param name="ids"></param>
        public async Task<bool> TorrentQueueMoveUpAsync(int[] ids)
        {
            var arguments = new Dictionary<string, object>
            {
                { "ids", ids }
            };

            var request = new TransmissionRequest("queue-move-up", arguments);

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Move down torrents in queue (API: queue-move-down)
        /// </summary>
        /// <param name="ids"></param>
        public async Task<bool> TorrentQueueMoveDownAsync(int[] ids)
        {
            var arguments = new Dictionary<string, object>
            {
                { "ids", ids }
            };

            var request = new TransmissionRequest("queue-move-down", arguments);

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Move torrents to bottom in queue  (API: queue-move-bottom)
        /// </summary>
        /// <param name="ids"></param>
        public async Task<bool> TorrentQueueMoveBottomAsync(int[] ids)
        {
            var arguments = new Dictionary<string, object>
            {
                { "ids", ids }
            };

            var request = new TransmissionRequest("queue-move-bottom", arguments);

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Set new location for torrents files (API: torrent-set-location)
        /// </summary>
        /// <param name="ids">Torrent ids</param>
        /// <param name="location">The new torrent location</param>
        /// <param name="move">Move from previous location</param>
        public async Task<bool> TorrentSetLocationAsync(int[] ids, string location, bool move)
        {
            var arguments = new Dictionary<string, object>
            {
                { "ids", ids },
                { "location", location },
                { "move", move }
            };

            var request = new TransmissionRequest("torrent-set-location", arguments);

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Rename a file or directory in a torrent (API: torrent-rename-path)
        /// </summary>
        /// <param name="id">The torrent whose path will be renamed</param>
        /// <param name="path">The path to the file or folder that will be renamed</param>
        /// <param name="name">The file or folder's new name</param>
        public async Task<RenamedTorrent> TorrentRenamePathAsync(int id, string path, string name)
        {
            var arguments = new Dictionary<string, object>
            {
                { "ids", new[] { id } },
                { "path", path },
                { "name", name }
            };

            var request = new TransmissionRequest("torrent-rename-path", arguments);
            
            TransmissionResponse response = await SendRequestAsync(request);

            return response.Deserialize<RenamedTorrent>();
        }

        #endregion

        #region System

        /// <summary>
        /// See if your incoming peer port is accessible from the outside world (API: port-test)
        /// </summary>
        /// <returns>Accessible state</returns>
        public async Task<bool> PortTestAsync()
        {
            var request = new TransmissionRequest("port-test", null);

            TransmissionResponse response = await SendRequestAsync(request);

            JObject data = response.Deserialize<JObject>();
            
            return (bool)data.GetValue("port-is-open");
        }

        /// <summary>
        /// Update blocklists (API: blocklist-update)
        /// </summary>
        /// <returns>success flag and error message, if applicable</returns>
        public async Task<(bool success, string error)> UpdateBlockListAsync()
        {
            var request = new TransmissionRequest("blocklist-update", null);

            TransmissionResponse response = await SendRequestAsync(request);

            return response.Result.Equals("success", StringComparison.OrdinalIgnoreCase) 
                ? (success: true, null) 
                : (success: false, response.Result);
        }

        #endregion

        /// <summary>
        /// Perform the Transmission API request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<TransmissionResponse> SendRequestAsync(TransmissionRequest request)
        {
            TransmissionResponse result;

            request.Tag = ++CurrentTag;

            try
            {
                if (_needAuthorization)
                {
                    result = await Host
                        .WithBasicAuth(Login, Password)
                        .WithHeader("Accept", "application/json-rpc")
                        .WithHeader("X-Transmission-Session-Id", SessionId)
                        .PostJsonAsync(request)
                        .ReceiveJson<TransmissionResponse>();
                }
                else
                {
                    result = await Host
                        .WithHeader("Accept", "application/json-rpc")
                        .WithHeader("X-Transmission-Session-Id", SessionId)
                        .PostJsonAsync(request)
                        .ReceiveJson<TransmissionResponse>();
                }

                if (result.Result != "success")
                    throw new Exception(result.Result);
            }
            catch (FlurlHttpException e)
            {
                if (e.Call.Response.StatusCode != HttpStatusCode.Conflict)
                    throw;

                SessionId = e.Call.Response.GetHeaderValue("X-Transmission-Session-Id");

                if (SessionId == null)
                    throw new Exception("Session id error");

                // repeat request with retrieved session id
                result = await SendRequestAsync(request);
            }

            return result;
        }
    }
}
