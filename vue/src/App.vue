<template>
  <h1>You did it!</h1>
  <p>
    Visit <a href="https://vuejs.org/" target="_blank" rel="noopener">vuejs.org</a> to read the
    documentation
  </p>
  <button @click="sendData">ここをクリック！！！</button>
</template>

<style scoped></style>

<script setup lang="ts">
import { HttpClient } from './services/api'
import { type testModel } from './interfaces/IHttpClient'

const sendData = async () => {
  const client = new HttpClient()
  const data: testModel = { text: "Vueから送信テスト！" }
  
  try {
    await client.testApi(data)
    alert("送信成功！C#のコンソールを見てね")
  } catch (error) {
    console.error("失敗:", error)
  }
}
</script>
