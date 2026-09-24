#!/bin/sh
set -eu

: "${SUPABASE_PROD_DB_CONNECTION:?Set the production Supabase PostgreSQL connection string.}"
export ConnectionStrings__SupabaseConnection="$SUPABASE_PROD_DB_CONNECTION"
export DOTNET_ENVIRONMENT=Production

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

dotnet ef database update \
    --project "$SCRIPT_DIR/../classlib/classlib.csproj" \
    --startup-project "$SCRIPT_DIR/../src/src.csproj" \
    --context SupabaseDbContext
