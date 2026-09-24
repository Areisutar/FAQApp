# Supabase用のEF Coreクラスライブラリ

.NET 9 / EF Core 9 / Npgsqlで、Supabase PostgreSQLのアプリ用テーブルを管理します。
TiDB用の `src/Data/ApplicationDbContext.cs` とは独立したContextとマイグレーションです。

## 構成

- `Models/`: C#モデルクラスの配置先。作成したモデルを `SupabaseDbContext` の `DbSet<T>` として登録します。
- `SupabaseDbContext.cs`: Supabase用Context。既定のスキーマは `public` です。
- `../src/Program.cs`: `AddDbContext<SupabaseDbContext>` と `UseNpgsql()` でContextをDI登録します。`dotnet ef` もこの登録を利用します。
- `Migrations/`: EFで生成したマイグレーションの配置先。ローカル・本番で同じファイルを使います。

現在は構成のひな形です。モデル・初期マイグレーション・RLS・Realtimeの設定はまだありません。
既存のWebプロジェクトがこのライブラリを参照し、ContextをDI登録しています。
ASP.NETのサービスからもコンストラクター注入で利用できます。

## 開発時の操作

開発用の接続先は `src/appsettings.Development.json` の
`ConnectionStrings.SupabaseConnection` に設定します。
マイグレーション作成用・ローカル更新用シェルは、この設定を利用します。
`SUPABASE_LOCAL_DB_CONNECTION` の設定は不要です。
Supabase CLIはEFのマイグレーションには不要です。

```sh
# Models/ とContextを変更した後、リポジトリのルートで実行
sh shell/supabase.ef.add.sh InitialCreate
sh shell/supabase.ef.update.local.sh
```

開発コンテナ内では `localhost` はそのコンテナ自身を指します。
Mac側で公開したSupabaseへDocker Desktopから接続する場合は、例えば
`Host=host.docker.internal;Port=54322` を使います。別コンテナへ直接接続する構成では、
共有ネットワーク上のDBホスト名と内部ポートを指定してください。

接続文字列は `Host=...;Port=...;Database=...;Username=...;Password=...;` 形式です。
SupabaseのAPI URL・APIキー・`postgresql://...` のURIではありません。
認証情報はコミットせず、環境変数に設定してください。

## 本番への適用

本番用シェルは `SUPABASE_PROD_DB_CONNECTION` を必須とします。
本番用シェルはこの値を `ConnectionStrings__SupabaseConnection` へ渡し、
`src/Program.cs` が `GetConnectionString("SupabaseConnection")` で読みます。
ローカル用とマイグレーション作成用シェルは `Development`、本番用は `Production` を指定します。
環境変数は `appsettings.json` / `appsettings.{環境名}.json` の
`ConnectionStrings.SupabaseConnection` より優先されます。
TiDB用の `DefaultConnection` とSupabase用の `SupabaseConnection` は別の設定です。
Supabase用の設定は、そのContextを生成するときに検証します。

GitHub Actionsに追加するステップの例です。Secretの登録と初期マイグレーション作成後、
Cloud Runへのデプロイ前に配置します。このステップ自体はまだ `deploy.yml` に追加していません。

```yaml
- name: Run Supabase EF Core migrations
  env:
    SUPABASE_PROD_DB_CONNECTION: ${{ secrets.SUPABASE_PROD_DB_CONNECTION }}
  run: sh shell/supabase.ef.update.prod.sh
```

既存ワークフローの.NET SDKと `dotnet-ef` を使います。CIから到達可能なSupabaseの
PostgreSQL接続先を指定し、TiDB用Secretとは分けてください。

テーブル変更のマイグレーションは開発時に生成・レビュー・コミットし、本番では適用だけを行います。
RLS、ポリシー、Realtime用publicationやトリガーは、必要に応じてEFマイグレーションの
`migrationBuilder.Sql(...)` にSQLを記述します。Supabase管理下の `auth` / `storage` スキーマは
モデル化しません。同じテーブルの構造変更をEFとSupabase CLIの両方で管理しないでください。
