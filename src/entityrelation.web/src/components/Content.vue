<template>
  <v-app>
    <v-row class="pa-4" justify="space-between">
      <v-col cols="4">
        <EntityNavigator></EntityNavigator>
      </v-col>
      <v-divider vertical></v-divider>
      <v-col>
        <div
          v-if="isNotExistEditableEntity"
          class="title grey--text text--lighten-1 font-weight-light"
          style="align-self: center;"
        >
          Не выбран элемент
        </div>
        <div v-else>
          <v-scroll-x-transition mode="out-in">
              <EntityEditor></EntityEditor>
          </v-scroll-x-transition>
        </div>
      </v-col>
    </v-row>
  </v-app>
</template>

<script>
import EntityNavigator from './EntityNavigator'
import EntityEditor from './EntityEditor'
import * as types from '@/store/types'

export default {
  components: {
    EntityNavigator,
    EntityEditor
  },

  computed: {
    isNotExistEditableEntity: {
      get () {
        return this.$store.getters[types.IS_NOT_EXIST_EDITABLE_ENTITY]
      }
    }
  }
}
</script>

<style></style>
