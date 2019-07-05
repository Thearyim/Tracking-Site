$ScriptPath = split-path $SCRIPT:MyInvocation.MyCommand.Path -parent

function Start-SyntheticTransactionService()
{
    $ErrorActionPreference = 'SilentlyContinue'
	Write-Host "Starting Synthetic Transaction Service..."

	try
	{
		$process = Get-Process -Name "SyntheticTransactionService"
		if ($process -eq $null)
		{
			Start-Process "cmd.exe" -ArgumentList @("/C", (Join-Path $ScriptPath "Eventcore.SyntheticTransactionService\Eventcore.SyntheticTransactionService.exe")) -WindowStyle Normal		
			Write-Host "Synthetic Transaction Service is running..."
		}
		else
		{
			Write-Host "Synthetic Transaction Service is already running..."
		}
	}
	catch
	{
		Write-Host "Synthetic Transaction Service is already running..."
	}
}

function Start-Website()
{
	$ErrorActionPreference = 'SilentlyContinue'
	Write-Host "Starting Web Server..."

	try
	{
	    # Get-Process -Name "Eventcore.Telemetry.Api.exe"
		$websitePath = $(Join-Path $ScriptPath "Eventcore.Monitoring.Website")
	    Start-Process "cmd.exe" -ArgumentList @("/C", "cd $websitePath & npm run start") -WindowStyle Normal			
		Write-Host "Web Server running..."
	}
	catch
	{
		Write-Host "Web Server is already running..."
	}
}

.\Start-Api.ps1
Start-SyntheticTransactionService
Start-Website
