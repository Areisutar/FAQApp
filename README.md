# FAQApp
FAQApp

## ログイン・確認用シード

`/login` でメールアドレスとパスワードを入力してログインできます。
`http://localhost:5134/` にアクセスすると、未ログインなら `/login`、
ログイン済みなら `/chat` へ移動します。ログイン成功後も `/chat` へ移動し、
チャット画面からログアウトできます。
HttpOnly Cookie の `FAQApp.Auth` は `/api/auth/me` でサーバーが検証します。
`/chat` に直接アクセスした場合も認証を確認し、未ログインなら `/login` へ戻します。
認証APIに接続できない場合はログイン画面にエラーを表示します。

認証のユーザー検索・パスワード検証・ロール取得は `src/Services/IAuthService.cs`、
リクエスト・レスポンスなどのモデルは `src/Models` に配置しています。

シードデータは `src/Models/SeedDatas` に定義しています。

- `ApplicationRoleSeedData.cs`: Admin・Public のロール
- `ApplicationUserSeedData.cs`: 各ロールの確認用ユーザー
- `ApplicationUserRoleSeedData.cs`: ユーザーとロールの関連

`ApplicationDbContext.OnModelCreating` の `HasData` から読み込みます。
シードと表示名のマイグレーションは `src/Migrations` に含まれています。
開発DBに反映する際は、接続先を確認して `sh shell/ef.update.sh` で適用してください。

| ロール | メールアドレス（ユーザー名） | 開発用パスワード |
| --- | --- | --- |
| Admin | admin@example.com | `Admin123!` |
| Public | public@example.com | `Public123!` |

`AspNetRoles` の GUID は Admin が `79a68294-7d70-4ed6-b153-66b7e6dcad50`、
Public が `a866201e-95db-46c5-9c48-f61b65ebd43b` です。
`AspNetUsers` に各1名、`AspNetUserRoles` に対応する関連を作成します。
パスワードは Identity V3 形式の生成済みハッシュで定義しています。
ID・ハッシュ・スタンプを固定し、モデル構築のたびに差分が出ないようにしています。
確認用の既知パスワードなので、このシードを含むマイグレーションは開発DB向けです。

ASP.NET をホストの `http://localhost:5134` で起動する場合、Vue は次のように起動します。

```sh
cd vue
VITE_API_PROXY_TARGET=http://localhost:5134 npm run dev
```

`http://localhost:5134/` を開き、上記の2ユーザーでログイン・チャットへの遷移・
再読み込み・ログアウトを確認してください。チャットデータの Supabase 認証・RLS とは連動しません。
Vue 側の認証遷移とエラー処理のテストは `cd vue` の後に `npm test` で実行できます。

参考: [ASP.NET Core Identity](https://learn.microsoft.com/aspnet/core/security/authentication/identity?view=aspnetcore-9.0)、
[CSRF 対策](https://learn.microsoft.com/aspnet/core/security/anti-request-forgery?view=aspnetcore-9.0)。
