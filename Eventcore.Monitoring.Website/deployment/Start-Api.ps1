$ScriptPath = split-path $SCRIPT:MyInvocation.MyCommand.Path -parent

function Start-SqlServer()
{
	$ErrorActionPreference = 'SilentlyContinue'
	Write-Host "Starting MySql Database Server..."

	try
	{
		$process = Get-Process -Name "mysqld"
		if ($process -eq $null)
		{
			Start-Process "cmd.exe" -ArgumentList @("/C", "mysqld.exe --user=root --console") -WindowStyle Normal		
			Start-Sleep 5
			Write-Host "MySql Database Server is running..."
		}
		else
		{
			Write-Host "MySql Database Server is already running..."
		}
	}
	catch
	{
		Write-Host "MySql Database Server is already running..."
	}
}

function Start-TelemetryApiService()
{
	$ErrorActionPreference = 'SilentlyContinue'
	Write-Host "Starting Telemetry API Service..."

	try
	{
		$process = Get-Process -Name "Eventcore.Telemetry.Api"
		if ($process -eq $null)
		{
			Start-Process "cmd.exe" -ArgumentList @("/C", (Join-Path $ScriptPath "Eventcore.Telemetry.Api\Eventcore.Telemetry.Api.exe")) -WindowStyle Normal		
			Start-Sleep 5	
			Write-Host "Telemetry API Service is running..."
		}
		else
		{
			Write-Host "Telemetry API Service is already running..."
		}
	}
	catch
	{
		Write-Host "Telemetry API Service is already running..."
	}
}

Start-SqlServer
Start-TelemetryApiService
