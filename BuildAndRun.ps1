# Function to display an error message and exit
function Error-Exit {
    param (
        [string]$Message
    )
    Write-Error $Message
    exit 1
}

# Define paths
$aspNetCoreAppPath = "source/api"
$angularAppPath = "source/ui"

# Get the current script directory
$currentDirectory = Get-Location

# Build ASP.NET Core app
Write-Host "Building ASP.NET Core app..."
Set-Location (Join-Path $currentDirectory $aspNetCoreAppPath) -ErrorAction Stop
dotnet build
if ($LASTEXITCODE -ne 0) {
    Error-Exit "Failed to build ASP.NET Core app."
}

# Build Angular app
Write-Host "Building Angular app..."
Set-Location (Join-Path $currentDirectory $angularAppPath) -ErrorAction Stop
npm install
if ($LASTEXITCODE -ne 0) {
    Error-Exit "Failed to install npm packages."
}
ng build
if ($LASTEXITCODE -ne 0) {
    Error-Exit "Failed to build Angular app."
}

# Run ASP.NET Core app
Write-Host "Running ASP.NET Core app..."
Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run" -WorkingDirectory (Join-Path $currentDirectory $aspNetCoreAppPath)

# Run Angular app
Write-Host "Running Angular app..."
Set-Location (Join-Path $currentDirectory $angularAppPath) -ErrorAction Stop
ng serve

Write-Host "Both ASP.NET Core and Angular apps are now running."