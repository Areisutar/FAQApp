import assert from 'node:assert/strict'
import { readFile } from 'node:fs/promises'
import test from 'node:test'
import axios from 'axios'
import ts from 'typescript'

// 通信境界だけを差し替え、実際の auth.ts のエラー処理を検証します。
let status = 204
const requests = []
axios.defaults.adapter = async (config) => {
  requests.push(config)
  if (config.url === '/csrf') {
    return { status: 200, data: { token: 'test-csrf' }, config, headers: {}, statusText: 'OK' }
  }
  const response = { status, data: {}, config, headers: {}, statusText: String(status) }
  if (status >= 400) throw new axios.AxiosError('Request failed', 'ERR_BAD_RESPONSE', config, null, response)
  return response
}

const source = await readFile(new URL('../src/services/auth.ts', import.meta.url), 'utf8')
const { outputText } = ts.transpileModule(source, {
  compilerOptions: { module: ts.ModuleKind.ESNext, target: ts.ScriptTarget.ES2022 },
})
const executable = outputText.replace(/from (['"])(axios|vue)\1/g,
  (_, quote, specifier) => `from ${quote}${import.meta.resolve(specifier)}${quote}`)
const { logout, currentUser } = await import(`data:text/javascript;base64,${Buffer.from(executable).toString('base64')}`)

test('ログアウトはCSRFトークンを付与して送信する', async () => {
  status = 204
  requests.length = 0
  await logout()
  assert.deepEqual(requests.map((request) => request.url), ['/csrf', '/logout'])
  assert.equal(requests[1].headers.get('X-CSRF-TOKEN'), 'test-csrf')
})

test('期限切れ・別タブのログアウトによる401はログアウト済みとして扱う', async () => {
  status = 401
  await assert.doesNotReject(logout())
})

test('ログアウト時のサーバー障害やCSRFエラーは成功扱いしない', async () => {
  for (status of [400, 500]) await assert.rejects(logout())
})

test('認証確認は401だけを未ログインとして扱う', async () => {
  status = 401
  assert.equal(await currentUser(), null)
  status = 500
  await assert.rejects(currentUser())
})
