# Log session end (Windows/PowerShell)

$LogDir = "logs/copilot"
$LogFile = "$LogDir/agent-activity.log"

New-Item -ItemType Directory -Force -Path $LogDir | Out-Null

$Branch = git branch --show-current 2>$null
if (-not $Branch) { $Branch = "no-branch" }
$Timestamp = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
$LastCommit = (git log --oneline -1 2>$null) -replace '"', "'"
if (-not $LastCommit) { $LastCommit = "no-commits" }

$Entry = @{
    event      = "SessionEnd"
    timestamp  = $Timestamp
    branch     = $Branch
    lastCommit = $LastCommit
} | ConvertTo-Json -Compress

Add-Content -Path $LogFile -Value $Entry

Write-Host "[session-logger] 📝 Session ended — $Timestamp"
