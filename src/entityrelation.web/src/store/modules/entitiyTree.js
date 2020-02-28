import * as types from '../types'

const state = {
  selectedEntity: null,
  editableEntity: null,
  parentForAddEntity: null,
  isEdit: true,
  isSubscribeOnAddNode: false,
  treeNodes: []
}

const getters = {
  [types.SELECTED_ENTITY]: state => {
    return state.selectedEntity
  },
  [types.EDITABLE_ENTITY]: state => {
    return state.editableEntity
  },
  [types.PARENT_FOR_ADD_ENTITY]: state => {
    return state.parentForAddEntity
  },
  [types.ISEDIT]: state => {
    return state.isEdit
  },
  [types.IS_SUBMIT_ADD_ENTITY]: state => {
    return state.isSubscribeOnAddNode
  },
  [types.IS_NOT_EXIST_EDITABLE_ENTITY]: state => {
    return state.selectedEntity === null && state.isEdit === true
  },
  [types.TREE_NODES]: state => {
    return state.treeNodes
  }
}

const mutations = {
  [types.MUTATE_SET_SELECTED_ENTITY]: (state, value) => {
    state.selectedEntity = value
  },
  [types.MUTATE_SET_EDITABLE_ENTITY]: (state, value) => {
    state.editableEntity = value
  },
  [types.MUTATE_SET_PARENT_FOR_ADD_ENTITY]: (state, value) => {
    state.parentForAddEntity = value
  },
  [types.MUTATE_SET_ISEDIT]: (state, value) => {
    state.isEdit = value
  },
  [types.MUTATE_SET_IS_SUBMIT_ADD_ENTITY]: (state, value) => {
    state.isSubscribeOnAddNode = value
  },
  [types.MUTATE_SET_TREE_NODES]: (state, value) => {
    state.treeNodes = value
  }
}

export default {
  state,
  getters,
  mutations
}
