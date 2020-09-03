# TransmissionManager
A Powershell module that integrates with the Transmission RPC API.

Available in the [Powershell Gallery](https://www.powershellgallery.com/packages/Transmission)

## Description
A collection of Powershell cmdlets that allow integration with a Transmission instance via the RPC API.

All endpoints in the RPC specification are exposed: [Transmission RPC spec](https://github.com/transmission/transmission/blob/master/extras/rpc-spec.txt).

## Installation (from the Powershell Gallery)

```powershell
Install-Module Transmission
Import-Module Transmission
```

## Supplying API credentials

Before any of the Transmission cmdlets can be run, both your host and credentials need to be registered to the current session using the Set-TransmissionCredentials cmdlet, for example:

```powershell
Set-TransmissionCredentials -Host "http://192.168.0.1:9091/transmission/rpc" -User "user" -Password "password"
Import-Module Transmission
```

This registers your credentials for the duration of the session. Adding a -StorePermanent switch to the Set-TransmissionCredentials command will create an encrypted file saved on your machine that will obviate the need to set credentials with each new session.

You can remove credentials at any time by using the Remove-TransmissionCredentials cmdlet. To remove a file created using the -StorePermanent switch run the Remove-TransmissionCredentials with a -DeletePermanent switch.

## Included cmdlets

```powershell
Get-TransmissionConfig 
Remove-TransmissionConfig
Set-TransmissionConfig
Close-TransmissionSession
Get-TransmissionSession
Get-TransmissionSessionStatistics
Test-TransmissionPort
Update-TransmissionBlockLists
Add-TransmissionTorrents
Assert-TransmissionTorrentsVerified
Get-TransmissionTorrents
Invoke-TransmissionTorrentsReannounce
Move-TransmissionTorrentsQueue
Remove-TransmissionTorrents
Rename-TransmissionTorrentPath
Set-TransmissionTorrents
Set-TransmissionTorrentsLocation
Start-TransmissionTorrents
Start-TransmissionTorrentsNow
Stop-TransmissionTorrents
```

## Building the module and importing locally

### Build the .NET core solution

```powershell
dotnet build [Github clone/download directory]\ps-transmission\src\PsTransmission.sln
```

### Copy the built files to your Powershell modules directory

Remove any existing installation in this directory, create a new module directory and copy all the built files.

```powershell
Remove-Item "C:\Users\[User]\Documents\PowerShell\Modules\Transmission" -Recurse -Force -ErrorAction SilentlyContinue
New-Item -Path 'C:\Users\[User]\Documents\PowerShell\Modules\Transmission' -ItemType Directory
Get-ChildItem -Path "[Github clone/download directory]\ps-transmission\src\PsTransmissionCmdlet\bin\Debug\netcoreapp3.1\" | Copy-Item -Destination "C:\Users\[User]\Documents\PowerShell\Modules\Transmission" -Recurse
```

## Contribute

Please raise an issue if you find a bug, or create a pull request to contribute.