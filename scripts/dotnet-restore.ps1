$rootLocation = "$PSScriptRoot/.."
$source = "-source https://api.nuget.org/v3/index.json --source https://www.myget.org/F/netcoretour/api/v3/index.json --no-cache"
$projects = @("Depot.Api", "Depot.Messages", "Depot.Services.Entries")

$projects | ForEach-Object {
    Write-Host "========================================================"
    Write-Host "Restoring packages for test project: $project"
    Write-Host "========================================================"
    Set-Location "$($rootLocation)/src/$($_)"
    Invoke-Command -ScriptBlock { dotnet restore }
}

$testProjects = @("Depot.Tests", "Depot.Tests.EndToEnd")
$testProjects | ForEach-Object {
    Write-Host "========================================================"
    Write-Host "Restoring packages for test project: $project"
    Write-Host "========================================================"
    Set-Location "$($rootLocation)/tests/$($_)"
    Invoke-Command -ScriptBlock { dotnet restore }
}

Set-Location "$rootLocation/scripts"