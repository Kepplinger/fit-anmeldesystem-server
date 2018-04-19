using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backend.Core.Contracts;
using StoreService.Persistence;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class TagController : Controller
    {

        [HttpGet]
        [ProducesResponseType(typeof(Tag), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public IActionResult Get()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                List<Tag> tags = uow.TagRepository.Get().ToList();
                if (tags != null)
                {
                    return new OkObjectResult(tags);
                }
                else
                {
                    return new NoContentResult();
                }
            }
        }


        [HttpPut]
        [ProducesResponseType(typeof(Tag), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Put(List<Tag> tagToUpdate)
        {
            if (tagToUpdate != null)
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    for (int i = 0; i < tagToUpdate.Count; i++)
                    {
                        if (tagToUpdate.ElementAt(i).Id > 0)
                        {
                            uow.TagRepository.Update(tagToUpdate.ElementAt(i));
                        }
                        else
                        {
                            uow.TagRepository.Insert(tagToUpdate.ElementAt(i));
                        }
                    }
                    uow.Save();
                    return new OkObjectResult(tagToUpdate);
                }
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}
