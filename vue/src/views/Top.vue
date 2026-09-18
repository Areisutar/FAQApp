<template>
  <h1>You did it!</h1>
  <p>
    Visit <a href="https://vuejs.org/" target="_blank" rel="noopener">vuejs.org</a> to read the
    documentation
  </p>
  <input v-model="form.text" type="text" placeholder="テキストを入力" />
  <button @click="sendData">送信</button>
</template>

<style scoped>
input {
  display: block;
  margin: 1rem 0;
  padding: 0.5rem;
  width: 300px;
}
</style>

<script setup lang="ts">
import { ref } from 'vue'
import { HttpClient } from '../services/api'
import { type testModel } from '../interfaces/IHttpClient'

const client = new HttpClient()

const form = ref<testModel>({ text: '' })

const sendData = async () => {
  try {
    const response = await client.testApi(form.value)
    console.log(response)
    alert('送信成功！C#のコンソールを見てね')
  } catch (error) {
    console.dir('失敗:', error)
  }
}
</script>