#!/bin/sh
set -eu

# ローカル開発を既定とし、明示された環境設定は優先する。
export DOTNET_ENVIRONMENT="${DOTNET_ENVIRONMENT:-${ASPNETCORE_ENVIRONMENT:-Development}}"

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_PATH="$SCRIPT_DIR/../src/src.csproj"

# 引数が渡された場合と、そうでない場合でコマンドを分ける
if [ -z "${1:-}" ]; then
  # 引数なし（最新まで反映）
  dotnet ef database update \
    --context ApplicationDbContext \
    --project "$PROJECT_PATH" \
    --startup-project "$PROJECT_PATH"
else
  # 引数あり（特定の移行まで反映）
  dotnet ef database update "$1" \
    --context ApplicationDbContext \
    --project "$PROJECT_PATH" \
    --startup-project "$PROJECT_PATH"
fi
