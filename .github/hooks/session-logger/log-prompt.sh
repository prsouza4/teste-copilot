#!/bin/bash
# Log user prompt submit to logs/copilot/agent-activity.log
# Note: $COPILOT_PROMPT is passed by the Copilot CLI runtime

LOG_DIR="logs/copilot"
LOG_FILE="$LOG_DIR/agent-activity.log"

mkdir -p "$LOG_DIR"

BRANCH=$(git branch --show-current 2>/dev/null || echo "no-branch")
TIMESTAMP=$(date -u '+%Y-%m-%dT%H:%M:%SZ')
# Truncate prompt to first 120 chars for log readability
PROMPT_EXCERPT=$(echo "${COPILOT_PROMPT:-}" | head -c 120)

echo "{\"event\":\"UserPrompt\",\"timestamp\":\"$TIMESTAMP\",\"branch\":\"$BRANCH\",\"promptExcerpt\":\"$PROMPT_EXCERPT\"}" >> "$LOG_FILE"
