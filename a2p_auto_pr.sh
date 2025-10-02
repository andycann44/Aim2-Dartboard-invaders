#!/usr/bin/env bash
set -euo pipefail

MSG=${1:-"update"}
[ -f ./a2p_snap.sh ] && ./a2p_snap.sh || true
git add -A
git commit -m "$MSG" || true

# Detect base branch (origin/HEAD if set, else main)
BASE=$(git symbolic-ref refs/remotes/origin/HEAD 2>/dev/null | sed 's@^refs/remotes/origin/@@' || echo main)

# Current branch
BR=$(git symbolic-ref --short HEAD 2>/dev/null || echo "")
# If on base branch or detached, move to a feature branch
if [ -z "${BR}" ] || [ "$BR" = "HEAD" ] || [ "$BR" = "$BASE" ]; then
  BR="a2p/$(date +%Y-%m-%d)-auto"
  git checkout -b "$BR" || git checkout "$BR"
else
  # stay on current feature branch
  :
fi

git push -u origin "$BR"

if command -v gh >/dev/null 2>&1; then
  # Create PR if none exists; else open it
  if ! gh pr view >/dev/null 2>&1; then
    TITLE=$(git log -1 --pretty=%s)
    gh pr create -t "$TITLE" -b "Auto PR from $BR" -B "$BASE" || true
  else
    gh pr view --web || true
  fi
else
  echo "GitHub CLI not found. Install: brew install gh && gh auth login"
fi

echo "Done. Branch: $BR"
