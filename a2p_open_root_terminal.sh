#!/usr/bin/env bash
set -euo pipefail
DIR=$(pwd)
if [ -d "/Applications/iTerm.app" ]; then
  open -a iTerm "$DIR"
else
  open -a Terminal "$DIR"
fi
