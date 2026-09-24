-- 開発用の公開チャット: 未ログインの利用者が全メッセージを閲覧・投稿できます。
-- 既存の public."TestModel" ("Id", "Text") に対して実行してください。
-- テーブルや既存データの削除・作り直しは行いません。
begin;

grant usage on schema public to anon;
grant select, insert on table public."TestModel" to anon;
alter table public."TestModel" enable row level security;

do $$
declare
  id_sequence text;
begin
  id_sequence := pg_get_serial_sequence('public."TestModel"', 'Id');
  if id_sequence is not null then
    execute format('grant usage on sequence %s to anon', id_sequence);
  end if;

  if not exists (
    select 1 from pg_policies
    where schemaname = 'public' and tablename = 'TestModel'
      and policyname = 'chat_anon_select'
  ) then
    create policy chat_anon_select on public."TestModel"
      for select to anon using (true);
  end if;

  if not exists (
    select 1 from pg_policies
    where schemaname = 'public' and tablename = 'TestModel'
      and policyname = 'chat_anon_insert'
  ) then
    create policy chat_anon_insert on public."TestModel"
      for insert to anon with check (length(btrim("Text")) > 0);
  end if;

  if not exists (
    select 1 from pg_publication_tables
    where pubname = 'supabase_realtime'
      and schemaname = 'public' and tablename = 'TestModel'
  ) then
    alter publication supabase_realtime add table public."TestModel";
  end if;
end $$;

commit;
