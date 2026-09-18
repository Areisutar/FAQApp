import { createRouter, createWebHistory } from 'vue-router'
import FormView from '../views/FormView.vue'
import Top from '../views/Top.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'top',
      component: Top,
    },
    {
      path: '/form',
      name: 'form',
      component: FormView,
    },
  ],
})

export default router
