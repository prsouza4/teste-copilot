#!/bin/bash
# Auto-commit on session end — commits staged work-in-progress changes

BRANCH=$(git branch --show-current 2>/dev/null)
if [ -z "$BRANCH" ]; then
  echo "[auto-commit] Not a git repository. Skipping."
  exit 0
fi

# Only commit on feature branches
if [[ "$BRANCH" != feature/* ]] && [[ "$BRANCH" != fix/* ]]; then
  echo "[auto-commit] Not on feature/fix branch ($BRANCH). Skipping."
  exit 0
fi

# Check for changes
if git diff --quiet && git diff --staged --quiet; then
  echo "[auto-commit] No changes to commit."
  exit 0
fi

# Stage all changes
git add -A

# Commit with WIP message
TIMESTAMP=$(date '+%Y-%m-%d %H:%M:%S')
git commit -m "wip: auto-save at session end [$TIMESTAMP]

Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>"

echo "[auto-commit] ✅ Changes committed to $BRANCH"
