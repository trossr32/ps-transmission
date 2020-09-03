﻿using System;
using System.Management.Automation;
using System.Threading.Tasks;
using PsTransmissionManager.Core.Components;
using PsTransmissionManager.Core.Services;

namespace TransmissionManager.Base
{
    public abstract partial class BaseTransmissionCmdlet : Cmdlet
    {
        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="BaseTransmissionCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            if (TransmissionContext.HasCredentials)
                return;

            TransmissionContext.Credentials = Task.Run(async () => await AuthService.GetConfig()).Result;

            if (TransmissionContext.HasCredentials)
                return;

            ThrowTerminatingError(new ErrorRecord(new Exception("No credentials set, Set-TransmissionCredentials must be run to save credentials before running any cmdlets."), null,
                ErrorCategory.AuthenticationError, null)
            {
                CategoryInfo =
                {
                    Reason = "No credentials set, Set-TransmissionCredentials must be run to save credentials before running any cmdlets."
                },
                ErrorDetails = new ErrorDetails("No credentials set, Set-TransmissionCredentials must be run to save credentials before running any cmdlets.")
            });
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="BaseTransmissionCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="BaseTransmissionCmdlet"/>.
        /// </summary>
        protected override void EndProcessing()
        {
            
        }
    }
}
