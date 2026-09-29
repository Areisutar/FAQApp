import { createRouter, createWebHistory } from 'vue-router'
import FormView from '../views/FormView.vue'
import { authCheckError, authError, currentUser } from '../services/auth'
import { createAuthGuard } from './authGuard'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/Login.vue'),
    },
    {
      path: '/',
      name: 'top',
      component: () => import('../views/Login.vue'),
    },
    {
      path: '/form',
      name: 'form',
      component: FormView,
    },
    {
      path: '/chat',
      name: 'chat',
      component: () => import('../views/Chat.vue'),
    },
  ],
})

router.beforeEach(createAuthGuard(currentUser, (error) => {
  authCheckError.value = error === null ? '' : authError(error)
}))

export default router
