using System;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmission.Core.Components;
using PsTransmission.Core.Models;
using PsTransmission.Core.Services;

namespace Transmission.Credentials
{
    /// <summary>
    /// <para type="synopsis">
    /// Register Transmission credentials for the session. Required before running any Transmission cmdlets.
    /// </para>
    /// <para type="description">
    /// Register Transmission credentials for the session. Required before running any Transmission cmdlets.
    /// </para>
    /// <para type="description">
    /// Optionally store credentials permanently in an encrypted locally stored file to remove the need for credentials
    /// to be set in each session by supplying the StorePermanent switch parameter.
    /// </para>
    /// <example>
    ///     <para>Example 1: Register credentials in session</para>
    ///     <code>PS C:\> Set-TransmissionCredentials -Host "http://192.168.0.1:9091/transmission/rpc" -User "user" -Password "12345"</code>
    ///     <remarks>Credentials will be registered for the lifetime of the session.</remarks>
    /// </example>
    /// <example>
    ///     <para>Example 2: Register credentials in session and store permanently</para>
    ///     <code>PS C:\> Set-TransmissionCredentials -Host "http://192.168.0.1:9091/transmission/rpc" -User "user" -Password "12345" -StorePermanent</code>
    ///     <remarks>Credentials will be registered for the lifetime of the session and stored locally so that this command will not need to be run again.</remarks>
    /// </example>
    /// <para type="link" uri="(https://github.com/trossr32/ps-transmission)">[Github]</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "TransmissionCredentials", HelpUri = "https://github.com/trossr32/ps-transmission")]
    public class SetTransmissionCredentialsCmdlet : Cmdlet
    {
        /// <summary>
        /// <para type="description">
        /// The URL of your Transmission API instance, which will look something like: http://192.168.0.1:9091/transmission/rpc
        /// </para>
        /// <para type="description">
        /// The Host is only validated to be a non-empty string, so please ensure you enter the URL correctly.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        [Alias("H")]
        public string Host { get; set; }

        /// <summary>
        /// <para type="description">
        /// The user name used to login to your Transmission instance, if applicable.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, Position = 1)]
        [Alias("U")]
        public string User { get; set; }

        /// <summary>
        /// <para type="description">
        /// The password used to login to your Transmission instance, if applicable.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false, Position = 2)]
        [Alias("P")]
        public string Password { get; set; }

        /// <summary>
        /// <para type="description">
        /// If supplied credentials will be stored permanently in an encrypted local file that removes the need for credentials to be set in each session.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [Alias("S")]
        public SwitchParameter StorePermanent { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="SetTransmissionCredentialsCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="SetTransmissionCredentialsCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                var config = new TransmissionCredentials
                {
                    Host = Host,
                    User = User,
                    Password = Password
                };

                TransmissionContext.Credentials = config;

                if (StorePermanent.IsPresent)
                    Task.Run(async () => await AuthService.SetConfig(config));
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Failed to set credentials, see inner exception for details", e), null, ErrorCategory.OperationStopped, null));
            }
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="SetTransmissionCredentialsCmdlet"/>.
        /// </summary>
        protected override void EndProcessing()
        {
            
        }
    }
}
