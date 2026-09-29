import type { NavigationGuard } from 'vue-router'
import type { LoginUser } from '../services/auth'

// Cookie の有無ではなく、API による署名・有効期限の検証結果で遷移を決めます。
export function createAuthGuard(
  loadUser: () => Promise<LoginUser | null>,
  onError: (error: unknown | null) => void,
): NavigationGuard {
  let redirectingAfterError = false
  return async (to) => {
    // 障害でログイン画面へ戻す際は再照会しません。
    // API が成功・失敗を繰り返しても /login と /chat の往復を防ぎます。
    if (redirectingAfterError && to.name === 'login') {
      redirectingAfterError = false
      return true
    }
    redirectingAfterError = false
    if (!['top', 'login', 'chat'].includes(String(to.name))) return true

    try {
      const user = await loadUser()
      onError(null)
      if (to.name === 'top') return { name: user ? 'chat' : 'login', replace: true }
      if (to.name === 'login' && user) return { name: 'chat', replace: true }
      if (to.name === 'chat' && !user) return { name: 'login', replace: true }
      return true
    } catch (error) {
      onError(error)
      redirectingAfterError = to.name !== 'login'
      // API 障害時にもログイン画面は開き、そこでエラーを表示します。
      return to.name === 'login' ? true : { name: 'login', replace: true }
    }
  }
}
