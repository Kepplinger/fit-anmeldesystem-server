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
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Get()
        {
            List<Tag> tags = new List<Tag>();

            using (IUnitOfWork uow = new UnitOfWork())
            {
                tags = uow.TagRepository.Get().ToList();
            }
            if (tags.Count > 0)
            {
                return new OkObjectResult(tags);
            }
            else
            {
                return new BadRequestResult();
            }
        }


        [HttpPut]
        [ProducesResponseType(typeof(Tag), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Put(Tag tagToUpdate)
        {
            if (tagToUpdate != null)
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    uow.TagRepository.Update(tagToUpdate);
                    return new OkObjectResult(tagToUpdate);
                }
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Tag), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Post(Tag tagToUpdate)
        {
            if (tagToUpdate != null)
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    uow.TagRepository.Insert(tagToUpdate);
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
