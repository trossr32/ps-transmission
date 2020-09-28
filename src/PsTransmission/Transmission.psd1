#
# Module manifest for module 'Transmission'
#
# Generated by: Rob Green
#
# Generated on: 30/08/2020
#

@{

    # Script module or binary module file associated with this manifest.
    RootModule           = 'Transmission.dll'

    # Version number of this module.
    ModuleVersion        = '1.0.8'

    # Supported PSEditions
    CompatiblePSEditions = 'Core'

    # ID used to uniquely identify this module
    GUID                 = '4890638d-8ea8-477c-ba30-b87536ede962'

    # Author of this module
    Author               = 'Rob Green'

    # Company or vendor of this module
    CompanyName          = 'Unknown'

    # Copyright statement for this module
    Copyright            = '(c) 2020 Rob Green. All rights reserved.'

    # Description of the functionality provided by this module
    Description          = 'A Powershell module that integrates with the Transmission RPC API.'

    # Minimum version of the Windows PowerShell engine required by this module
    PowerShellVersion    = '7.0'

    # Name of the Windows PowerShell host required by this module
    # PowerShellHostName = ''

    # Minimum version of the Windows PowerShell host required by this module
    # PowerShellHostVersion = ''

    # Minimum version of Microsoft .NET Framework required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
    # DotNetFrameworkVersion = ''

    # Minimum version of the common language runtime (CLR) required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
    # CLRVersion = ''

    # Processor architecture (None, X86, Amd64) required by this module
    # ProcessorArchitecture = ''

    # Modules that must be imported into the global environment prior to importing this module
    # RequiredModules = @()

    # Assemblies that must be loaded prior to importing this module
    # RequiredAssemblies = @()

    # Script files (.ps1) that are run in the caller's environment prior to importing this module.
    # ScriptsToProcess = @()

    # Type files (.ps1xml) to be loaded when importing this module
    # TypesToProcess = @()

    # Format files (.ps1xml) to be loaded when importing this module
    # FormatsToProcess = @()

    # Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
    # NestedModules = @()

    # Functions to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no functions to export.
    # FunctionsToExport    = @()

    # Cmdlets to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no cmdlets to export.
    CmdletsToExport      = @(
                            'Set-TransmissionCredentials', 
                            'Remove-TransmissionCredentials',  
                            'Close-TransmissionSession', 
                            'Get-TransmissionSession', 
                            'Get-TransmissionSessionStatistics',
                            'Set-TransmissionAltSpeedLimits',
                            'Set-TransmissionSession', 
                            'Test-TransmissionPort', 
                            'Update-TransmissionBlockLists', 
                            'Invoke-TransmissionWeb', 
                            'Add-TransmissionTorrents', 
                            'Assert-TransmissionTorrentsVerified', 
                            'Get-TransmissionTorrents', 
                            'Invoke-TransmissionTorrentsReannounce', 
                            'Move-TransmissionTorrentsQueue', 
                            'Remove-TransmissionTorrents', 
                            'Rename-TransmissionTorrentPath', 
                            'Set-TransmissionTorrents', 
                            'Set-TransmissionTorrentsLocation', 
                            'Start-TransmissionTorrents', 
                            'Start-TransmissionTorrentsNow', 
                            'Stop-TransmissionTorrents'
                            )

    # Variables to export from this module
    # VariablesToExport = @()

    # Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
    AliasesToExport      = @()

    # DSC resources to export from this module
    # DscResourcesToExport = @()

    # List of all modules packaged with this module
    # ModuleList = @()

    # List of all files packaged with this module
    # FileList = @()

    # Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
    PrivateData          = @{

        PSData = @{

            # Tags applied to this module. These help with module discovery in online galleries.
            Tags       = 'Transmission', 'RPC', 'API', 'Manager', 'Torrent', 'Magnet'

            # A URL to the license for this module.
            LicenseUri = 'https://github.com/trossr32/ps-transmission/blob/master/LICENSE'

            # A URL to the main website for this project.
            ProjectUri = 'https://github.com/trossr32/ps-transmission'

            # A URL to an icon representing this module.
            IconUri    = 'https://github.com/trossr32/ps-transmission/blob/master/assets/images/ps7_icon_88.png'

            # ReleaseNotes of this module
            # ReleaseNotes = 'Latest version is a migration from a Powershell script to a .NET core Cmdlet.'

            # Prerelease string of this module
            # Prerelease = ''

            # Flag to indicate whether the module requires explicit user acceptance for install/update/save
            # RequireLicenseAcceptance = $false

            # External dependent modules of this module
            # ExternalModuleDependencies = @()

        } # End of PSData hashtable

    } # End of PrivateData hashtable

    # HelpInfo URI of this module
    HelpInfoURI          = 'https://github.com/trossr32/ps-transmission'

    # Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
    # DefaultCommandPrefix = ''

}