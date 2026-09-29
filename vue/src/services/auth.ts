import axios from 'axios'
import { ref } from 'vue'

export interface LoginUser {
  id: string
  name: string
  email: string | null
  userName: string | null
  roles: string[]
}

export const authCheckError = ref('')

const api = axios.create({ baseURL: '/api/auth', timeout: 10000 })

async function csrfHeaders() {
  const { data } = await api.get<{ token: string }>('/csrf')
  return { 'X-CSRF-TOKEN': data.token }
}

export async function login(email: string, password: string, rememberMe: boolean) {
  const headers = await csrfHeaders()
  const { data } = await api.post<LoginUser>('/login', { email, password, rememberMe }, { headers })
  return data
}

export async function currentUser() {
  try {
    const { data } = await api.get<LoginUser>('/me')
    return data
  } catch (error) {
    if (axios.isAxiosError(error) && error.response?.status === 401) return null
    throw error
  }
}

export async function logout() {
  const headers = await csrfHeaders()
  try {
    await api.post('/logout', {}, { headers })
  } catch (error) {
    // 別タブでのログアウトや期限切れの場合は、既にログアウト済みです。
    if (axios.isAxiosError(error) && error.response?.status === 401) return
    throw error
  }
}

export function authError(error: unknown) {
  if (axios.isAxiosError(error)) {
    if (typeof error.response?.data?.message === 'string') return error.response.data.message
    if (error.response?.status === 400) return '入力内容を確認し、もう一度お試しください。'
  }
  return 'サーバーに接続できませんでした。しばらくしてからもう一度お試しください。'
}
