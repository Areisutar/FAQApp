#!/usr/bin/env bash

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

dotnet ef migrations remove \
  --project "$SCRIPT_DIR/../src/src.csproj" \
  --startup-project "$SCRIPT_DIR/../src/src.csproj"
