<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { authCheckError, authError, login } from '../services/auth'

const router = useRouter()

const email = ref('')
const password = ref('')
const rememberMe = ref(false)
const showPassword = ref(false)
const busy = ref(false)
const errorMessage = ref('')

async function submit() {
  if (busy.value) return
  busy.value = true
  errorMessage.value = ''
  authCheckError.value = ''
  try {
    await login(email.value.trim(), password.value, rememberMe.value)
    password.value = ''
    showPassword.value = false
    await router.replace({ name: 'chat' })
  } catch (error) {
    errorMessage.value = authError(error)
  } finally {
    busy.value = false
  }
}

</script>

<template>
  <main class="login-page">
    <section class="login-card" aria-labelledby="login-title" :aria-busy="busy">
      <RouterLink to="/" class="brand">FAQ<span>App</span></RouterLink>
      <p class="eyebrow">WELCOME BACK</p>
      <h1 id="login-title">ログイン</h1>
      <p class="description">アカウント情報を入力して、はじめましょう。</p>

      <p v-if="errorMessage || authCheckError" role="alert" class="error">{{ errorMessage || authCheckError }}</p>

      <form @submit.prevent="submit">
        <fieldset :disabled="busy">
          <label for="email">メールアドレス</label>
          <input id="email" v-model="email" type="email" autocomplete="username" placeholder="you@example.com" required />

          <label for="password">パスワード</label>
          <div class="password-field">
            <input id="password" v-model="password" :type="showPassword ? 'text' : 'password'" autocomplete="current-password" placeholder="パスワードを入力" required />
            <button type="button" class="toggle" :aria-pressed="showPassword" aria-controls="password" @click="showPassword = !showPassword">{{ showPassword ? '隠す' : '表示' }}</button>
          </div>

          <label class="remember"><input v-model="rememberMe" type="checkbox" />ログイン状態を保持する</label>
          <button type="submit" class="primary">{{ busy ? 'ログイン中…' : 'ログイン' }}</button>
        </fieldset>
      </form>
      <p class="footer">FAQApp · みんなの疑問を、ひとつずつ。</p>
    </section>
  </main>
</template>

<style scoped>
.login-page { min-height: 100svh; box-sizing: border-box; display: grid; place-items: center; padding: 40px 20px; background: radial-gradient(ellipse at top left, #e2eee8, transparent 60%), #f5f7f5; color: #243c32; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif; }
.login-card { box-sizing: border-box; width: 100%; max-width: 460px; padding: 40px; border: 1px solid #dfe7e1; border-radius: 20px; background: #fff; box-shadow: 0 16px 48px #233e3010; }
.brand { display: inline-block; margin-bottom: 36px; font-size: 25px; font-weight: 800; color: #244b3a; text-decoration: none; }
.brand span { font-weight: 400; }
.eyebrow { color: #527663; font-size: 11px; font-weight: 700; letter-spacing: 0.15em; }
h1 { margin: 10px 0; font-size: 28px; }
.description { margin: 0 0 28px; font-size: 13px; line-height: 1.8; color: #637369; }
fieldset { padding: 0; margin: 0; border: 0; min-width: 0; }
label { display: block; margin: 22px 0 8px; font-size: 13px; font-weight: 600; }
input:not([type='checkbox']) { box-sizing: border-box; width: 100%; padding: 13px 14px; border: 1px solid #cbd7ce; border-radius: 8px; background: #fcfdfc; color: #243c32; font: inherit; font-size: 16px; }
input:focus-visible, button:focus-visible, a:focus-visible { outline: 3px solid #8bc4a5; outline-offset: 3px; }
.password-field { position: relative; }
.password-field input { padding-right: 66px; }
button { font: inherit; cursor: pointer; }
.toggle { position: absolute; top: 0; right: 4px; height: 100%; padding: 0 12px; border: 0; background: transparent; color: #39614c; font-size: 12px; }
.remember { display: flex; align-items: center; gap: 8px; margin: 20px 0 26px; color: #53665b; font-weight: 400; }
.remember input { width: 16px; height: 16px; accent-color: #285b40; }
.primary { box-sizing: border-box; display: block; width: 100%; padding: 14px; border: 0; border-radius: 8px; background: #285b40; color: white; font-size: 14px; font-weight: 600; text-align: center; }
.primary:hover { background: #1b442e; }
button:disabled, fieldset:disabled button { cursor: wait; opacity: 0.65; }
.home-link { text-decoration: none; margin-top: 28px; }
.secondary { width: 100%; margin-top: 12px; padding: 12px; border: 1px solid #cbd7ce; border-radius: 8px; background: white; color: #285b40; }
.error, .success { padding: 12px 14px; border-radius: 8px; font-size: 13px; line-height: 1.7; }
.error { background: #fff0ed; color: #9c3222; }
.success { background: #edf6ef; color: #285b40; }
dt { font-size: 12px; color: #637369; margin-top: 18px; }
dd { margin: 6px 0 0; overflow-wrap: anywhere; }
.role { display: inline-block; margin-right: 6px; padding: 4px 10px; border-radius: 5px; background: #edf3ee; color: #285b40; font-size: 13px; }
.footer { margin: 32px 0 0; padding-top: 24px; border-top: 1px solid #edf0ed; color: #718177; text-align: center; font-size: 11px; }
@media (max-width: 480px) { .login-page { padding: 20px 12px; } .login-card { padding: 28px 24px; } }
</style>
