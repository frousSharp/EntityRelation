using System;
using System.Collections.Generic;
using System.Linq;
using EntityRelationAPI.Core.Model;
using EntityRelationAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EntityRelationAPI.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntityController : ControllerBase
    {
        private readonly IEntityService _service;

        public EntityController(IEntityService service)
        {
            _service = service;
        }

        [HttpGet]
        public Entity Get(long id)
        {
            var entity = _service.GetById(id);

            return entity;
        }

        [HttpGet]
        [Route("GetAll")]
        public List<Entity> GetAll()
        {
            var entities = _service.Select().ToList();

            return entities;
        }

        [HttpGet]
        [Route("GetRoot")]
        public List<Entity> GetRootEntities()
        {
            var rootEntities = _service.GetRootEntities();

            return rootEntities;
        }

        [HttpGet]
        [Route("GetChildren/{parentId}")]
        public List<Entity> GetChildrenEntities(long parentId)
        {
            var childrenEntities = _service.GetChildrenEntities(parentId);

            return childrenEntities;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Entity model)
        {
            long entityId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                entityId = _service.Create(model);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

            return Ok(entityId);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Entity model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _service.Update(model);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                _service.Delete(id);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

            return Ok();
        }
    }
}
