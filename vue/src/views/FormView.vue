<template>
  <div class="form-container">
    <h1>アンケートフォーム</h1>
    <form @submit.prevent="submitForm">
      <div class="form-group">
        <label>性別</label>
        <div class="checkbox-group">
          <label>
            <input type="checkbox" value="男" v-model="gender" />
            男
          </label>
          <label>
            <input type="checkbox" value="女" v-model="gender" />
            女
          </label>
        </div>
      </div>

      <div class="form-group">
        <label>好きなSNS</label>
        <div class="radio-group">
          <label v-for="sns in snsOptions" :key="sns">
            <input type="radio" :value="sns" v-model="favoriteSns" />
            {{ sns }}
          </label>
        </div>
      </div>

      <div class="form-group">
        <label for="description">自由記述</label>
        <textarea id="description" v-model="description" rows="5" placeholder="自由に入力してください"></textarea>
      </div>

      <button type="submit">送信</button>
    </form>
  </div>
</template>

<style scoped>
.form-container {
  max-width: 600px;
  margin: 2rem auto;
  padding: 2rem;
}
.form-group {
  margin-bottom: 1.5rem;
}
.checkbox-group,
.radio-group {
  display: flex;
  gap: 1.5rem;
  margin-top: 0.5rem;
}
textarea {
  width: 100%;
  padding: 0.5rem;
}
button {
  padding: 0.5rem 2rem;
}
</style>

<script setup lang="ts">
import { ref } from 'vue'
import { HttpClient } from '../services/api'
import type { formModel } from '../interfaces/IHttpClient'

const gender = ref<string[]>([])
const favoriteSns = ref('')
const description = ref('')

const snsOptions = ['Twitter/X', 'Instagram', 'YouTube', 'TikTok', 'その他']

const submitForm = async () => {
  const client = new HttpClient()
  const data: formModel = {
    gender: gender.value.join(','),
    favoriteSns: favoriteSns.value,
    description: description.value
  }

  try {
    await client.formApi(data)
    alert('送信成功！')
  } catch (error) {
    console.error('送信失敗:', error)
    alert('送信に失敗しました')
  }
}
</script>
