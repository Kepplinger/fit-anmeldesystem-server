using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

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



        //[HttpGet]
        //[ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        //public IActionResult getImagesToCompany(int companyId)
        //{
        //    string folderPath = "../Media";
        //    Company c = _unitOfWork.CompanyRepository.GetById(companyId);
        //    byte[] image = System.Text.Encoding.UTF8.GetBytes(c.Logo);
        //    return new OkObjectResult(folderPath);
        //}

    }
}
