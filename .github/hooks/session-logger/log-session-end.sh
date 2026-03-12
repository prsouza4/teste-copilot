#!/bin/bash
# Log session end to logs/copilot/agent-activity.log

LOG_DIR="logs/copilot"
LOG_FILE="$LOG_DIR/agent-activity.log"

mkdir -p "$LOG_DIR"

BRANCH=$(git branch --show-current 2>/dev/null || echo "no-branch")
TIMESTAMP=$(date -u '+%Y-%m-%dT%H:%M:%SZ')
COMMIT=$(git log --oneline -1 2>/dev/null | head -c 80 || echo "no-commits")

echo "{\"event\":\"SessionEnd\",\"timestamp\":\"$TIMESTAMP\",\"branch\":\"$BRANCH\",\"lastCommit\":\"$COMMIT\"}" >> "$LOG_FILE"

echo "[session-logger] 📝 Session ended — $TIMESTAMP"
