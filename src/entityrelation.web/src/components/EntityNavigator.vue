<template>
  <div>
    <EntityTree ref="entityTree"></EntityTree>
  </div>
</template>

<script>
import EntityTree from './EntityTreeView'

export default {
  data: function () {
    return {
      search: null
    }
  },

  components: {
    EntityTree
  }
}
</script>
