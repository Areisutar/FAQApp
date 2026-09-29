import assert from 'node:assert/strict'
import { readFile } from 'node:fs/promises'
import test from 'node:test'
import ts from 'typescript'
import { createMemoryHistory, createRouter } from 'vue-router'

const source = await readFile(new URL('../src/router/authGuard.ts', import.meta.url), 'utf8')
const { outputText } = ts.transpileModule(source, {
  compilerOptions: { module: ts.ModuleKind.ESNext, target: ts.ScriptTarget.ES2022 },
})
const { createAuthGuard } = await import(`data:text/javascript;base64,${Buffer.from(outputText).toString('base64')}`)

const user = { id: 'test-user', name: '全体管理者', email: 'admin@example.com', userName: 'admin@example.com', roles: ['Admin'] }

function setup(loadUser) {
  const errors = []
  const router = createRouter({
    history: createMemoryHistory(),
    routes: [
      { path: '/', name: 'top', component: {} },
      { path: '/login', name: 'login', component: {} },
      { path: '/chat', name: 'chat', component: {} },
      { path: '/form', name: 'form', component: {} },
    ],
  })
  router.beforeEach(createAuthGuard(loadUser, (error) => errors.push(error)))
  return { router, errors }
}

for (const path of ['/', '/login', '/chat']) {
  test(`未ログイン: ${path} → /login`, async () => {
    const { router } = setup(async () => null)
    await router.push(path)
    assert.equal(router.currentRoute.value.path, '/login')
  })

  test(`ログイン済み: ${path} → /chat`, async () => {
    const { router } = setup(async () => user)
    await router.push(path)
    assert.equal(router.currentRoute.value.path, '/chat')
  })

  test(`API障害: ${path} → エラー付き /login（ループなし）`, async () => {
    const failure = new Error('API unavailable')
    let calls = 0
    const { router, errors } = setup(async () => {
      assert.ok(++calls <= 2, '認証確認が繰り返されている')
      throw failure
    })
    await router.push(path)
    assert.equal(router.currentRoute.value.path, '/login')
    assert.equal(errors.at(-1), failure)
  })
}

test('ログイン後の遷移と、ログアウト後に戻る操作でも認証を再確認する', async () => {
  let session = null
  const { router } = setup(async () => session)
  await router.push('/')
  session = user
  await router.replace('/chat')
  assert.equal(router.currentRoute.value.path, '/chat')
  session = null
  await router.replace('/login')
  await router.push('/chat')
  assert.equal(router.currentRoute.value.path, '/login')
})

test('セッション期限切れ後にルートへ戻るとログイン画面を開く', async () => {
  let session = user
  const { router } = setup(async () => session)
  await router.push('/chat')
  session = null
  await router.push('/')
  assert.equal(router.currentRoute.value.path, '/login')
})

test('対象外のページでは認証APIを呼ばない', async () => {
  const { router } = setup(async () => assert.fail('認証APIが呼ばれた'))
  await router.push('/form')
  assert.equal(router.currentRoute.value.path, '/form')
})

test('認証APIが断続的に失敗してもログインとチャットを往復しない', async () => {
  let calls = 0
  const failure = new Error('Temporary failure')
  const { router, errors } = setup(async () => {
    if (++calls % 2 === 1) throw failure
    return user
  })
  await router.push('/chat')
  assert.equal(router.currentRoute.value.path, '/login')
  assert.equal(calls, 1)
  assert.equal(errors.at(-1), failure)
  // 後続の操作では再度認証状態を確認できること。
  await router.push('/chat')
  assert.equal(router.currentRoute.value.path, '/chat')
  assert.equal(errors.at(-1), null)
})
