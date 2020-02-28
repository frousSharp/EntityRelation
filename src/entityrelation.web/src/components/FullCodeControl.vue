<template>
  <div>
    <v-text-field v-model="fullCode" label="Полный код" disabled>
    </v-text-field>
  </div>
</template>

<script>
export default {
  props: ['selectedEntity', 'editableEntityCode'],

  computed: {
    fullCode () {
      var commonParentCode = this.getCommonParentsCode(this.selectedEntity)

      if (commonParentCode.length === 0) {
        return this.editableEntityCode
      }

      if (!this.editableEntityCode || this.editableEntityCode.length === 0) {
        return commonParentCode
      }

      if (
        commonParentCode.length !== 0 &&
        this.editableEntityCode.length !== 0
      ) {
        return commonParentCode + '.' + this.editableEntityCode
      }

      return ''
    }
  },

  methods: {
    getCommonParentsCode (selectedEntity) {
      const codes = []

      if (!this.selectedEntity.parent) {
        return codes
      }

      while (selectedEntity.parent) {
        codes.push(selectedEntity.parent.code)
        selectedEntity = selectedEntity.parent
      }

      const code = codes.reverse().join('.')

      return code
    }
  }
}
</script>
