<template>
    <v-app>
      <v-card outlined>
        <v-list-item>
          <v-list-item-title class="headline mb-1">Дерево элементов</v-list-item-title>
           <v-btn text icon color="indigo" @click="callAddEntity()">
            <v-icon>mdi-plus</v-icon>
          </v-btn>
        </v-list-item>
      </v-card>
      <v-treeview v-if="isNodesExist"
        :active.sync="active"
        @update:active="update"
        :items.sync="this.treeNodes"
        :load-children="loadChildrenNodes"
        activatable
        return-object
      >
      <template slot="append" slot-scope="{ item, active}" v-if="active">
          <v-btn text icon color="indigo" @click="callAddEntity(item);">
            <v-icon>mdi-plus</v-icon>
          </v-btn>
          <v-btn text icon color="indigo" @click="deleteEntity(item)">
             <v-icon>mdi-delete</v-icon>
          </v-btn>
        </template>
      </v-treeview>
      <div v-else class="title grey--text text--lighten-1 font-weight-light"
                  style="align-self: left; margin: 10px">
          <p>Отсутствуют элементы для отображения.</p>
        </div>
    </v-app>
</template>

<script>

import * as types from '../store/types'
import EntityService from '../service/EntityService'

export default {
  data: () => ({
    active: []
  }),

  computed: {
    selectedEntity:
    {
      get () {
        return this.$store.getters[types.SELECTED_ENTITY]
      },
      set (value) {
        this.$store.commit(types.MUTATE_SET_SELECTED_ENTITY, value)
      }
    },

    editableEntity:
    {
      get () {
        return this.$store.getters[types.EDITABLE_ENTITY]
      },
      set (value) {
        this.$store.commit(types.MUTATE_SET_EDITABLE_ENTITY, value)
      }
    },

    isEdit:
    {
      get () {
        return this.$store.getters[types.ISEDIT]
      },
      set (value) {
        this.$store.commit(types.MUTATE_SET_ISEDIT, value)
      }
    },

    parentForAddEntity:
    {
      get () {
        return this.$store.getters[types.PARENT_FOR_ADD_ENTITY]
      },
      set (value) {
        this.$store.commit(types.MUTATE_SET_PARENT_FOR_ADD_ENTITY, value)
      }
    },

    treeNodes: {
      get () {
        return this.$store.getters[types.TREE_NODES]
      },
      set (value) {
        this.$store.commit(types.MUTATE_SET_TREE_NODES, value)
      }
    },

    isSubscribeOnAddNode:
    {
      get () {
        if (this.$store === undefined) return null

        return this.$store.getters[types.IS_SUBMIT_ADD_ENTITY]
      },
      set (value) {
        this.$store.commit(types.MUTATE_SET_IS_SUBMIT_ADD_ENTITY)
      }
    },

    isNodesExist () {
      return this.treeNodes.length > 0
    }
  },

  methods: {
    loadRootNodes () {
      EntityService.getRoot().then((response) => {
        this.treeNodes = response.data
      }).catch(error => console.warn(error))
    },

    async loadChildrenNodes (item) {
      return EntityService.getChildren(item.id)
        .then(response => {
          response.data.forEach(node => {
            node.parent = item
            item.children.push(node)
          })
        })
        .catch(error => console.warn(error))
    },

    update (data) {
      if (!data || !this.active.length) {
        this.selectedEntity = null
        return
      }

      this.active = data
      this.selectedEntity = this.active[0]
      this.isEdit = true
    },

    async deleteEntity (node) {
      var hasChildrens = node.children.length > 0

      if (hasChildrens === false) {
        hasChildrens = await this.hasChildrens(node)
      }

      if (hasChildrens) {
        if (!confirm('Продолжить удаление элемента "' + node.name + '"? Это приведет к удалению дочерних объектов')) {
          return
        }
      }

      EntityService.delete(node.id)
        .catch(error => console.warn(error))

      this.deleteTreeNode(node)
    },

    callAddEntity (node = null) {
      this.parentForAddEntity = node
      this.isEdit = false
      this.update()
    },

    async addNode (node) {
      const parentNode = this.parentForAddEntity ? this.parentForAddEntity : this.treeNodes
      const parentNodes = this.parentForAddEntity ? this.parentForAddEntity.children : this.treeNodes

      if (parentNodes.length === 0 && parentNode !== this.treeNodes) {
        await this.loadChildrenNodes(parentNode)
      } else {
        parentNodes.push(node)
      }

      this.update()

      this.isEdit = true
      this.isSubscribeOnAddNode = false
    },

    deleteTreeNode (node, tree) {
      var parentNode = node.parent
      var nodes = parentNode ? parentNode.children : this.treeNodes

      for (let i = 0; i < nodes.length; i++) {
        if (nodes[i].id === node.id) {
          nodes.splice(i, 1)
          break
        }
      }
    },

    async hasChildrens (node) {
      var result = await this.loadChildrenNodes(node).then(result => {
        return node.children.length > 0
      })

      return result
    }
  },

  watch: {
    isSubscribeOnAddNode (value) {
      if (value === true) {
        this.addNode(this.editableEntity)
      }
    }
  },

  mounted () {
    this.loadRootNodes()
  }
}
</script>
