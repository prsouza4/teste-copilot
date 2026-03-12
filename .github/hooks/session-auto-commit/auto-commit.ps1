# Auto-commit on session end (Windows/PowerShell version)

$Branch = git branch --show-current 2>$null
if (-not $Branch) {
    Write-Host "[auto-commit] Not a git repository. Skipping."
    exit 0
}

# Only commit on feature branches
if ($Branch -notmatch '^(feature|fix)/') {
    Write-Host "[auto-commit] Not on feature/fix branch ($Branch). Skipping."
    exit 0
}

# Check for changes
$Status = git status --porcelain
if (-not $Status) {
    Write-Host "[auto-commit] No changes to commit."
    exit 0
}

# Stage all changes
git add -A

# Commit with WIP message
$Timestamp = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
$Message = "wip: auto-save at session end [$Timestamp]`n`nCo-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>"
git commit -m $Message

Write-Host "[auto-commit] ✅ Changes committed to $Branch"
