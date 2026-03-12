# Log session start (Windows/PowerShell)

$LogDir = "logs/copilot"
$LogFile = "$LogDir/agent-activity.log"

New-Item -ItemType Directory -Force -Path $LogDir | Out-Null

$Branch = git branch --show-current 2>$null
if (-not $Branch) { $Branch = "no-branch" }
$Timestamp = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
$User = $env:USERNAME ?? "unknown"

$Entry = @{
    event     = "SessionStart"
    timestamp = $Timestamp
    branch    = $Branch
    user      = $User
} | ConvertTo-Json -Compress

Add-Content -Path $LogFile -Value $Entry

Write-Host "[session-logger] 📝 Session started — $Timestamp on $Branch"
