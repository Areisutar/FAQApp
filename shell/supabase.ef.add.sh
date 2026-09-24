#!/bin/sh
set -eu

if [ "$#" -ne 1 ] || [ -z "$1" ]; then
    echo "Usage: sh supabase.ef.add.sh <MigrationName>" >&2
    exit 1
fi

# src/appsettings.Development.json の SupabaseConnection を使用します。
export DOTNET_ENVIRONMENT=Development

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

dotnet ef migrations add "$1" \
    --project "$SCRIPT_DIR/../classlib/classlib.csproj" \
    --startup-project "$SCRIPT_DIR/../src/src.csproj" \
    --context SupabaseDbContext \
    --output-dir Migrations
