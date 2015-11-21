namespace Ignite.Api
{
    using Entities;
    using Services.Infrastructure;
    using Microsoft.AspNet.JsonPatch;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class SimpleController<T> : BaseController
        where T : BaseEntity
    {
        private readonly ILogger localLogger;
        private readonly IBaseService<T> localService;

        public SimpleController(IBaseService<T> baseService, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            localService = baseService;
            localLogger = loggerFactory.CreateLogger<IBaseService<T>>();
        }

        [HttpGet]        
        public virtual IActionResult Get()
        {
            try
            {
                localLogger.LogVerbose("Find all entities");
                var allEntities = localService.FindAll();
                return Json(new { data = allEntities.ToList() });
            }
            catch (Exception ex)
            {
                return GetErrorJson(ex, "Error finding all entities");
            }
        }

        [HttpGet("{id:int}")]
        public virtual IActionResult Get(int id)
        {
            try
            {
                var entity = localService.FindById(id);
                if (entity == null)
                {
                    localLogger.LogVerbose($"Find by id failed: {id}");
                    return HttpNotFound();
                }
                localLogger.LogVerbose($"Found entity: {id}");
                return Json(new { data = new[] { entity } });
            }
            catch (Exception ex)
            {
                return GetErrorJson(ex, "Error getting entity by id.");
            }
        }

        [HttpPost]
        public virtual IActionResult Post([FromBody, Required]T value)
        {
            try
            {
                if (value == null)
                {
                    localLogger.LogError("Entity value not passed in, not creating");
                    return new BadRequestResult();
                }
                var entity = localService.Create(value);
                if (entity == null)
                {
                    return new BadRequestResult();
                }
                return Json(new { data = new[] { entity } });
            }
            catch (Exception ex)
            {
                return GetErrorJson(ex, "Error creating entity");
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public virtual IActionResult Put(int id, [FromBody, Required]T value)
        {
            try
            {
                if (value == null)
                {
                    localLogger.LogError($"Entity value not passed in, not updating: {id}");
                    return HttpBadRequest();
                }
                if (!localService.Exists(id))
                {
                    localLogger.LogVerbose($"Entity not found, not updating: {id}");
                    return HttpNotFound();
                }
                localLogger.LogInformation($"Before updating entity {id}");
                if (localService.Update(value))
                {
                    return new HttpStatusCodeResult(204);
                }
                else
                {
                    localLogger.LogWarning($"Entity failed to update: {id}");
                    return HttpBadRequest();
                }
            }
            catch (Exception ex)
            {
                return GetErrorJson(ex, "Error updating entity.");
            }
        }

        [HttpPatch("{id}")]
        public virtual IActionResult Patch(int id, [FromBody]JsonPatchDocument<T> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    localLogger.LogError($"Entity value not passed in, not patching: {id}");
                    return HttpBadRequest();
                }
                var baseEntity = localService.FindById(id);
                if (baseEntity == null)
                {
                    localLogger.LogVerbose($"Entity not found, not patching: {id}");
                    return HttpNotFound();
                }
                localLogger.LogInformation($"Before patching entity {id}");
                patchDoc.ApplyTo(baseEntity);
                if (localService.Update(baseEntity))
                {
                    return new HttpStatusCodeResult(204);
                }
                else
                {
                    localLogger.LogWarning($"Entity failed to patch: {id}");
                    return HttpBadRequest();
                }
            }
            catch (Exception ex)
            {
                return GetErrorJson(ex, "Error patching entity.");
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id:int}")]
        public virtual IActionResult Delete(int id)
        {
            try
            {
                if (!localService.Exists(id))
                {
                    localLogger.LogVerbose($"Entity not found, not deleting: {id}");
                    return HttpNotFound();
                }
                localLogger.LogInformation($"Before deleting entity {id}");
                if (localService.Delete(id))
                {
                    localLogger.LogInformation($"Entity deleted: {id}");
                    return new HttpStatusCodeResult(204);
                }
                else
                {
                    localLogger.LogWarning($"Entity failed to delete: {id}");
                    return HttpBadRequest();
                }
            }
            catch (Exception ex)
            {
                return GetErrorJson(ex, "Error deleting entity.");
            }
        }
    }
}
