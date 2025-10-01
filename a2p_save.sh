#!/usr/bin/env bash
set -euo pipefail
MSG=${1:-"update"}
[ -f ./a2p_snap.sh ] && ./a2p_snap.sh || true
git add -A
git commit -m "$MSG" || echo "Nothing to commit."
git push
