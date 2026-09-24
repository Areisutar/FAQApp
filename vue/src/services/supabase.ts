import { createClient } from '@supabase/supabase-js'

export type ChatMessage = {
  Id: number
  Text: string
}

type Database = {
  public: {
    Tables: {
      TestModel: {
        Row: ChatMessage
        Insert: { Text: string }
        Update: { Text?: string }
        Relationships: []
      }
    }
    Views: { [_ in never]: never }
    Functions: { [_ in never]: never }
  }
}

export function createChatClient() {
  const url = import.meta.env.VITE_SUPABASE_URL?.trim()
  const key = import.meta.env.VITE_SUPABASE_PUBLISHABLE_KEY?.trim()

  if (!url || !key || key === 'YOUR_PUBLISHABLE_KEY') {
    throw new Error('Supabase の接続設定がありません。vue/README.md の設定手順を確認してください。')
  }

  return createClient<Database>(url, key, {
    auth: { persistSession: false, autoRefreshToken: false, detectSessionInUrl: false },
  })
}
