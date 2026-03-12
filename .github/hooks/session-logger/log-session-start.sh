#!/bin/bash
# Log session start to logs/copilot/agent-activity.log

LOG_DIR="logs/copilot"
LOG_FILE="$LOG_DIR/agent-activity.log"

mkdir -p "$LOG_DIR"

BRANCH=$(git branch --show-current 2>/dev/null || echo "no-branch")
TIMESTAMP=$(date -u '+%Y-%m-%dT%H:%M:%SZ')

echo "{\"event\":\"SessionStart\",\"timestamp\":\"$TIMESTAMP\",\"branch\":\"$BRANCH\",\"user\":\"${USER:-unknown}\"}" >> "$LOG_FILE"

echo "[session-logger] 📝 Session started — $TIMESTAMP on $BRANCH"
