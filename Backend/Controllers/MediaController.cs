using Backend.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class MediaController
    {

        private IUnitOfWork _unitOfWork;

        public MediaController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }
    }
}
