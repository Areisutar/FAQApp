#!/bin/sh

set -eu

# ローカル開発を既定とし、明示された環境設定は優先する。
export DOTNET_ENVIRONMENT="${DOTNET_ENVIRONMENT:-${ASPNETCORE_ENVIRONMENT:-Development}}"

if [ $# -lt 1 ]; then
  echo "Usage: sh ef.add.sh <MigrationName>"
  exit 1
fi

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

dotnet ef migrations add "$1" \
  --project "$SCRIPT_DIR/../src/src.csproj" \
  --startup-project "$SCRIPT_DIR/../src/src.csproj"
