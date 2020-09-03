using PsTransmission.Core.Models;

namespace PsTransmission.Core.Components
{
    /// <summary>
    /// The static credentials used by the PsTransmissionManager module, set with the Set-TransmissionCredentials cmdlet.
    /// </summary>
    public static partial class TransmissionContext
    {
        public static TransmissionCredentials Credentials { get; set; }

        /// <summary>
        /// Check if the credentials are valid (i.e. that there is a Host property)
        /// </summary>
        public static bool HasCredentials => !string.IsNullOrWhiteSpace(Credentials?.Host);

        /// <summary>
        /// Set credentials to null
        /// </summary>
        public static void Dispose()
        {
            Credentials = null;
        }
    }
}
