#!/bin/sh
set -eu

# src/appsettings.Development.json の SupabaseConnection を使用します。
export DOTNET_ENVIRONMENT=Development

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

dotnet ef database update \
    --project "$SCRIPT_DIR/../classlib/classlib.csproj" \
    --startup-project "$SCRIPT_DIR/../src/src.csproj" \
    --context SupabaseDbContext
