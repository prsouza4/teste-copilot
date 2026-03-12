# Log user prompt submit (Windows/PowerShell)
# Note: $env:COPILOT_PROMPT is passed by the Copilot CLI runtime

$LogDir = "logs/copilot"
$LogFile = "$LogDir/agent-activity.log"

New-Item -ItemType Directory -Force -Path $LogDir | Out-Null

$Branch = git branch --show-current 2>$null
if (-not $Branch) { $Branch = "no-branch" }
$Timestamp = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")

# Truncate prompt to first 120 chars
$RawPrompt = $env:COPILOT_PROMPT ?? ""
$PromptExcerpt = if ($RawPrompt.Length -gt 120) { $RawPrompt.Substring(0, 120) } else { $RawPrompt }
$PromptExcerpt = $PromptExcerpt -replace '"', "'"

$Entry = @{
    event         = "UserPrompt"
    timestamp     = $Timestamp
    branch        = $Branch
    promptExcerpt = $PromptExcerpt
} | ConvertTo-Json -Compress

Add-Content -Path $LogFile -Value $Entry
