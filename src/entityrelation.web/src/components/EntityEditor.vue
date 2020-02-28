<template>
  <v-tabs>
    <v-tab>Атрибуты элемента</v-tab>
    <v-tab-item>
      <div>
        <v-form ref="form" lazy-validation>
          <v-autocomplete
            v-model="parentEntity"
            :items="parentEntities"
            label="Родительский элемент"
            :disabled="true"
          >
          </v-autocomplete>
          <FullCodeControl
            :selectedEntity="this.editableEntity"
            :editableEntityCode="editableEntity.code"
          ></FullCodeControl>

          <v-text-field
            v-model="editableEntity.name"
            label="Название"
            required
            :error-messages="nameErrors"
            @input="$v.editableEntity.name.$touch()"
            @blur="$v.editableEntity.name.$touch()"
          >
          </v-text-field>
          <v-text-field
            v-model.trim="editableEntity.code"
            label="Код"
            :counter="3"
            :error-messages="codeErrors"
            @input="$v.editableEntity.code.$touch()"
            @blur="$v.editableEntity.code.$touch()"
          ></v-text-field>
          <div style="margin: 30px 0px">
            <div v-if="isEdit">
              <v-btn
                class="mr-4"
                @click="updateEntity"
                :disabled="!this.isInputValid"
              >
                Изменить
              </v-btn>
            </div>
            <div v-else>
              <v-btn
                class="mr-4"
                @click="createEntity"
                :disabled="!this.isInputValid"
              >
                Создать
              </v-btn>
            </div>
          </div>
        </v-form>
      </div>
    </v-tab-item>
  </v-tabs>
</template>

<script>
import * as types from '../store/types'
import EntityService from '@/service/EntityService'
import FullCodeControl from './FullCodeControl'
import { required, maxLength } from 'vuelidate/lib/validators'

export default {
  data: () => ({
    data: [],
    parentEntity: '',
    parentEntities: []
  }),

  components: {
    FullCodeControl
  },

  validations: {
    editableEntity: {
      name: { required },
      code: {
        maxLength: maxLength(3),
        isUnique (inputCode) {
          if (inputCode === '') {
            return true
          }

          return this.isEntityCodeUniqueInSameLevel(inputCode)
        }
      }
    }
  },

  computed: {
    selectedEntity: {
      get () {
        return this.$store.getters[types.SELECTED_ENTITY]
      },
      set (value) {
        this.$store.commit(types.MUTATE_SET_SELECTED_ENTITY, value)
      }
    },

    editableEntity: {
      get () {
        return this.$store.getters[types.EDITABLE_ENTITY]
      },
      set (value) {
        this.$store.commit(types.MUTATE_SET_EDITABLE_ENTITY, value)
      }
    },

    isEdit: {
      get () {
        return this.$store.getters[types.ISEDIT]
      }
    },

    parentForAddEntity: {
      get () {
        return this.$store.getters[types.PARENT_FOR_ADD_ENTITY]
      }
    },

    treeNodes: {
      get () {
        return this.$store.getters[types.TREE_NODES]
      }
    },

    isSubscribeOnAddNode: {
      get () {
        return this.$store.getters[types.IS_SUBMIT_ADD_ENTITY]
      },
      set (value) {
        this.$store.commit(types.MUTATE_SET_IS_SUBMIT_ADD_ENTITY, value)
      }
    },

    nameErrors () {
      const errors = []
      if (!this.$v.editableEntity.name.$dirty) return errors
      !this.$v.editableEntity.name.required &&
        errors.push('Название не должно быть пустым')
      return errors
    },

    codeErrors () {
      const errors = []
      if (!this.$v.editableEntity.code.$dirty) return errors
      !this.$v.editableEntity.code.maxLength &&
        errors.push('Длина кода не должна превышать 3 символа')
      !this.$v.editableEntity.code.isUnique &&
        errors.push('Код не является уникальным среди элементов родителя')
      return errors
    },

    isInputValid () {
      return !this.$v.$invalid
    }
  },

  methods: {
    updateEntity () {
      this.validateInputForm()
      if (this.isInputValid === false) {
        return
      }

      this.updateNodeData()
      var updatedEntity = this.getDtoEntity()

      EntityService.update(updatedEntity).catch(error => console.warn(error))
    },

    updateNodeData () {
      this.selectedEntity.name = this.editableEntity.name
      this.selectedEntity.code = this.editableEntity.code
    },

    createEntity () {
      this.validateInputForm()
      if (this.isInputValid === false) {
        return
      }

      var newEntity = this.getDtoEntity()

      EntityService.create(newEntity)
        .then(response => {
          this.editableEntity.id = response.data
          this.isSubscribeOnAddNode = true
        })
        .catch(error => console.warn(error))
    },

    getDtoEntity () {
      var entity = Object.assign({}, this.editableEntity)
      entity.parent = null
      entity.children = []

      return entity
    },

    initializeEditableEntity () {
      this.editableEntity = Object.assign({}, this.selectedEntity)
      this.initializeParentNode()
    },

    initializeNewEditableEntity () {
      const newEntity = {
        name: '',
        code: '',
        children: []
      }

      if (this.parentForAddEntity) {
        newEntity.parentId = this.parentForAddEntity.id
        newEntity.parent = this.parentForAddEntity
      } else {
        newEntity.parentId = null
        newEntity.parent = null
      }

      this.editableEntity = newEntity
      this.initializeParentNode()
    },

    initializeParentNode () {
      this.parentEntities = []
      const missedItem = 'Отсутствует'

      if (!this.editableEntity) {
        return
      }

      if (!this.editableEntity.parentId) {
        this.parentEntities.push(missedItem)
        this.parentEntity = missedItem

        return
      }

      this.parentEntities.push(this.editableEntity.parent.name)
      this.parentEntity = this.editableEntity.parent.name
    },

    isEntityCodeUniqueInSameLevel (code) {
      var currentLevelNodes = this.editableEntity.parent
        ? this.editableEntity.parent.children
        : this.treeNodes

      if (currentLevelNodes.length === 0) {
        return true
      }

      var newCurrentLevelNodes = currentLevelNodes.filter(x => {
        return x.id !== this.editableEntity.id
      })

      var currentLevelCodes = newCurrentLevelNodes.map(x => x.code)
      var result = !currentLevelCodes.includes(code, 0)

      return result
    },

    validateInputForm () {
      this.$v.$touch()
    }
  },

  watch: {
    isEdit (value) {
      if (value === true) {
        return
      }

      this.initializeNewEditableEntity()
    },

    selectedEntity (value) {
      if (!value) {
        return
      }

      this.initializeEditableEntity()
    }
  },

  created () {
    if (this.isEdit === true) {
      this.initializeEditableEntity()
    } else {
      this.initializeNewEditableEntity()
    }
  }
}
</script>
