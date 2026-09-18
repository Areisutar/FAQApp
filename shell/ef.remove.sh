#!/usr/bin/env bash

set -euo pipefail

# ローカル開発を既定とし、明示された環境設定は優先する。
export DOTNET_ENVIRONMENT="${DOTNET_ENVIRONMENT:-${ASPNETCORE_ENVIRONMENT:-Development}}"

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

dotnet ef migrations remove \
  --project "$SCRIPT_DIR/../src/src.csproj" \
  --startup-project "$SCRIPT_DIR/../src/src.csproj"
