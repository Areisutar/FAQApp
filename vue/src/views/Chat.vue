<script setup lang="ts">
import { nextTick, onMounted, onUnmounted, ref } from 'vue'
import type { RealtimeChannel } from '@supabase/supabase-js'
import { createChatClient, type ChatMessage } from '../services/supabase'

const messages = ref<ChatMessage[]>([])
const draft = ref('')
const loading = ref(false)
const sending = ref(false)
const configured = ref(false)
const connected = ref(false)
const connectionStatus = ref('接続中…')
const configurationError = ref('')
const realtimeError = ref('')
const loadError = ref('')
const sendError = ref('')
const messageList = ref<HTMLElement | null>(null)

let client: ReturnType<typeof createChatClient> | undefined
let channel: RealtimeChannel | undefined
let disposed = false
let refreshPending = false
const abortController = new AbortController()

function errorMessage(error: unknown) {
  return error instanceof Error ? error.message : '通信に失敗しました。接続を確認してください。'
}

// 通知が読み込み中に届いた場合は再取得し、古いレスポンスによる上書きを防ぎます。
async function refreshMessages() {
  if (!client || disposed) return
  refreshPending = true
  if (loading.value) return

  loading.value = true
  try {
    do {
      refreshPending = false
      const { data, error } = await client
        .from('TestModel')
        .select('Id, Text')
        .order('Id', { ascending: false })
        .limit(100)
        .abortSignal(abortController.signal)

      if (disposed) return
      if (error) {
        loadError.value = `メッセージを取得できませんでした: ${error.message}`
      } else {
        messages.value = (data ?? []).reverse()
        loadError.value = ''
        await nextTick()
        messageList.value?.scrollTo({ top: messageList.value.scrollHeight })
      }
    } while (refreshPending && !disposed)
  } catch (error) {
    if (!disposed) loadError.value = errorMessage(error)
  } finally {
    loading.value = false
  }
}

async function sendMessage() {
  const text = draft.value.trim()
  if (!client || !text || sending.value || disposed) return

  sending.value = true
  sendError.value = ''
  try {
    const { error } = await client.from('TestModel').insert({ Text: text })
    if (disposed) return
    if (error) {
      sendError.value = `送信できませんでした: ${error.message}`
      return
    }
    draft.value = ''
    // 自分の送信は Realtime の通知が遅れても反映します。
    await refreshMessages()
  } catch (error) {
    if (!disposed) sendError.value = errorMessage(error)
  } finally {
    sending.value = false
  }
}

onMounted(() => {
  try {
    client = createChatClient()
    configured.value = true
    channel = client
      .channel('test-model-chat', {
        config: { postgres_changes_options: { wait: true } },
      })
      .on('system', {}, (payload) => {
        if (disposed || payload.extension !== 'postgres_changes') return
        if (payload.status === 'error') {
          connected.value = false
          connectionStatus.value = 'リアルタイム購読に失敗しました'
          realtimeError.value = String(payload.message)
        }
      })
      .on('postgres_changes', { event: '*', schema: 'public', table: 'TestModel' }, () => {
        void refreshMessages()
      })
      .subscribe((status, error) => {
        if (disposed) return
        connected.value = status === 'SUBSCRIBED'
        if (status === 'SUBSCRIBED') {
          connectionStatus.value = 'リアルタイム接続中'
          realtimeError.value = ''
          // 再接続時にも取得し、切断中の変更を反映します。
          void refreshMessages()
        } else {
          connectionStatus.value = 'リアルタイム接続が切れています。再接続を待っています。'
          if (error) realtimeError.value = error.message
          else if (status === 'TIMED_OUT') realtimeError.value = '接続がタイムアウトしました。'
        }
      })
    void refreshMessages()
  } catch (error) {
    configured.value = false
    connectionStatus.value = '接続できません'
    configurationError.value = errorMessage(error)
  }
})

onUnmounted(() => {
  disposed = true
  abortController.abort()
  if (client && channel) void client.removeChannel(channel)
})
</script>

<template>
  <main class="chat">
    <header>
      <RouterLink to="/">トップへ</RouterLink>
      <h1>チャット</h1>
      <p class="status" :class="{ connected }" role="status">{{ connectionStatus }}</p>
      <p class="description">最新100件のメッセージを表示します。</p>
    </header>

    <p v-if="configurationError" class="error" role="alert">{{ configurationError }}</p>
    <p v-if="realtimeError" class="error" role="alert">
      変更通知を受信できません: {{ realtimeError }}
    </p>
    <div v-if="loadError" class="error" role="alert">
      {{ loadError }}
      <button type="button" :disabled="loading" @click="refreshMessages">再読み込み</button>
    </div>

    <div ref="messageList" class="messages" role="log" aria-label="メッセージ一覧" aria-live="polite">
      <p v-if="loading && !messages.length" class="empty">読み込み中…</p>
      <p v-else-if="configured && !loadError && !messages.length" class="empty">
        まだメッセージはありません。最初のメッセージを送ってみましょう。
      </p>
      <article v-for="message in messages" :key="message.Id" class="message">
        <span class="message-id">#{{ message.Id }}</span>
        <p>{{ message.Text }}</p>
      </article>
    </div>

    <form @submit.prevent="sendMessage">
      <label for="chat-message">メッセージ</label>
      <textarea
        id="chat-message"
        v-model="draft"
        rows="3"
        placeholder="メッセージを入力してください"
        :disabled="!configured || sending"
      />
      <p v-if="sendError" class="error" role="alert">{{ sendError }}</p>
      <button type="submit" :disabled="!configured || sending || !draft.trim()">
        {{ sending ? '送信中…' : '送信' }}
      </button>
    </form>
  </main>
</template>

<style scoped>
.chat {
  max-width: 720px;
  margin: 2rem auto;
  padding: 0 1rem;
  color: #1f2937;
  font-family: sans-serif;
}
h1 { margin-bottom: 0.5rem; }
.status, .description, .empty, .message-id { color: #64748b; }
.status.connected { color: #15803d; }
.description, .message-id { font-size: 0.85rem; }
.messages {
  height: min(50vh, 480px);
  overflow-y: auto;
  padding: 1rem;
  border: 1px solid #cbd5e1;
  border-radius: 12px;
  background: #f8fafc;
}
.message {
  margin-bottom: 0.75rem;
  padding: 0.75rem 1rem;
  border-radius: 8px;
  background: white;
  border: 1px solid #e2e8f0;
}
.message p { margin: 0.35rem 0 0; white-space: pre-wrap; overflow-wrap: anywhere; }
form { display: grid; gap: 0.75rem; margin-top: 1rem; }
textarea {
  box-sizing: border-box;
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #94a3b8;
  border-radius: 8px;
  resize: vertical;
  font: inherit;
}
button {
  justify-self: end;
  padding: 0.65rem 1.5rem;
  border: 0;
  border-radius: 8px;
  background: #1d4ed8;
  color: white;
  cursor: pointer;
}
button:disabled { opacity: 0.5; cursor: not-allowed; }
.error { padding: 0.75rem; background: #fef2f2; color: #b91c1c; overflow-wrap: anywhere; }
</style>
