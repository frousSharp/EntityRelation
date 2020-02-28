import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store/index'
import vuetify from './plugins/vuetify'
import axius from 'axios'
import VueAxius from 'vue-axios'
import Vuelidate from 'vuelidate'

Vue.use(VueAxius, axius)
Vue.use(Vuelidate)

Vue.axios.defaults.baseURL = 'https://localhost:44343/'
Vue.config.productionTip = false

new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app')
