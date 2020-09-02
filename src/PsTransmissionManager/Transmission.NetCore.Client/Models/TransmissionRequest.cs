using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transmission.NetCore.Client.Models
{
    /// <summary>
	/// Transmission request 
	/// </summary>
	internal class TransmissionRequest : CommunicateBase
    {
        /// <summary>
        /// Name of the method to invoke
        /// </summary>
        [JsonProperty("method")]
        public string Method;

        public TransmissionRequest(string method, Dictionary<string, object> arguments)
        {
            Method = method;
            Arguments = arguments;
        }
    }
}
