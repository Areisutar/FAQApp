# vue

## Supabase チャット (`/chat`)

`src/views/Chat.vue` から `public."TestModel"` の `"Id"` / `"Text"` を利用します。
最新100件を Id 順に表示し、投稿を INSERT します。INSERT / UPDATE / DELETE を
Supabase Realtime で購読して一覧を再取得し、再接続時も最新状態に同期します。
ログイン機能・投稿者名・投稿日時はありません。

### 接続設定

1. この `vue` ディレクトリで `.env.example` を `.env.local` にコピーします。
2. Supabase Dashboard のプロジェクト URL と API Keys にある **Publishable key**
   （従来の **anon key** でも可）を設定します。

   ```dotenv
   VITE_SUPABASE_URL=https://aortfuqxhogkbxrhsxoc.supabase.co
   VITE_SUPABASE_PUBLISHABLE_KEY=ここに公開APIキーを入力
   ```

   ブラウザ用の設定なので、DB パスワード・secret key・service_role key は入れません。
   `src/appsettings.Development.json` の DB 接続文字列とは別の設定です。
   `.env.local` は Git の管理対象外です。

3. Supabase の SQL Editor で [`supabase/chat-setup.sql`](supabase/chat-setup.sql) を実行します。
   既存の `public."TestModel"` に SELECT / INSERT 権限と RLS ポリシーを設定し、
   `supabase_realtime` の配信対象に追加します。テーブルの作り直しは不要です。
   **この SQL は開発用の公開チャット向けです。未ログインの誰でも既存データを含む
   全メッセージを閲覧・投稿できるようになります。** 認証が必要な用途では、
   利用者に応じた RLS ポリシーとログイン処理を用意してください。
   Supabase の Data API が有効で、`public` が公開スキーマに含まれている必要があります。
4. `npm install` と `npm run dev` を実行し、`http://localhost:8888/chat` を開きます。
   環境変数を変更した場合は開発サーバーを再起動してください。
   本番環境でも、Vite の **ビルド時** に同じ環境変数を設定してください。

### 動作確認

- `/chat` を2つのタブで開き、片方で送信した内容が両方に表示されることを確認します。
- Supabase の Table Editor で `Text` を変更、または行を削除し、画面に反映されることを確認します。
- 表示や送信が失敗する場合は、URL・公開キー・列名の大文字小文字・RLS・テーブル権限を確認します。
- 他のタブへの反映がない場合は、`TestModel` が `supabase_realtime` に追加されているか確認します。
  EF Core でテーブルを作成しただけでは、Realtime の配信対象には追加されません。
  `RealtimeDisabledForConfiguration` と表示された場合は、手順3の SQL を実行してください。
  画面の接続表示は、DB の変更通知の購読が成立したことを確認してから切り替えます。

参考: [Supabase Postgres Changes](https://supabase.com/docs/guides/realtime/postgres-changes)、
[API keys](https://supabase.com/docs/guides/api/api-keys)。

This template should help get you started developing with Vue 3 in Vite.

## Recommended IDE Setup

[VS Code](https://code.visualstudio.com/) + [Vue (Official)](https://marketplace.visualstudio.com/items?itemName=Vue.volar) (and disable Vetur).

## Recommended Browser Setup

- Chromium-based browsers (Chrome, Edge, Brave, etc.):
  - [Vue.js devtools](https://chromewebstore.google.com/detail/vuejs-devtools/nhdogjmejiglipccpnnnanhbledajbpd)
  - [Turn on Custom Object Formatter in Chrome DevTools](http://bit.ly/object-formatters)
- Firefox:
  - [Vue.js devtools](https://addons.mozilla.org/en-US/firefox/addon/vue-js-devtools/)
  - [Turn on Custom Object Formatter in Firefox DevTools](https://fxdx.dev/firefox-devtools-custom-object-formatters/)

## Type Support for `.vue` Imports in TS

TypeScript cannot handle type information for `.vue` imports by default, so we replace the `tsc` CLI with `vue-tsc` for type checking. In editors, we need [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) to make the TypeScript language service aware of `.vue` types.

## Customize configuration

See [Vite Configuration Reference](https://vite.dev/config/).

## Project Setup

```sh
npm install
```

### Compile and Hot-Reload for Development

```sh
npm run dev
```

### Type-Check, Compile and Minify for Production

```sh
npm run build
```
