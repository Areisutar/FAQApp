#!/bin/sh

set -eu

if [ $# -lt 1 ]; then
  echo "Usage: sh ef.add.sh <MigrationName>"
  exit 1
fi

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

dotnet ef migrations add "$1" \
  --project "$SCRIPT_DIR/../src/src.csproj" \
  --startup-project "$SCRIPT_DIR/../src/src.csproj"