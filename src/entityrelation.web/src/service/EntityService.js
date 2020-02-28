import axius from 'axios'

class EntityService {
  getAll () {
    return axius.get('api/entity/GetAll')
  }

  getRoot () {
    return axius.get('api/entity/GetRoot')
  }

  getChildren (parentId) {
    return axius.get('api/entity/GetChildren/' + parentId)
  }

  create (entity) {
    return axius.post('api/entity/', entity)
  }

  update (entity) {
    return axius.put('api/entity/', entity)
  }

  delete (id) {
    return axius.delete('api/entity/' + id)
  }
}

export default new EntityService()
