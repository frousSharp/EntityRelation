<template>
  <div>
    <v-app>
      <v-card>
        <v-card-title class="indigo white--text headline">
          <p style="margin: 10px">DEMO ENTITY RELATION</p>
        </v-card-title>
          <Content></Content>
      </v-card>
    </v-app>
  </div>
</template>

<script>
import Content from './components/Content'

export default {
  name: 'App',

  components: {
    Content
  }
}
</script>

<style scoped>
.footer {
  align-content: center;
}
</style>
