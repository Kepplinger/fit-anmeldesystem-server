using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

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

        [HttpGet]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public IActionResult getImagesToCompany(int companyId)
        {
            string folderPath = "../Media";
            Company c = _unitOfWork.CompanyRepository.GetById(companyId);
            byte[] image = System.Text.Encoding.UTF8.GetBytes(c.FolderInfo.Logo);
            return new OkObjectResult(folderPath);
        }

        public object Base64ToImage(string basestr, string filepath)
        {
            byte[] imageBytes = Convert.FromBase64String(basestr);
            //MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            //ms.Write(imageBytes, 0, imageBytes.Length);
            using (var imageFile = new FileStream(filepath, FileMode.Create))
            {
                imageFile.Write(imageBytes, 0, imageBytes.Length);
                imageFile.Flush();
                return imageFile;
            }
        }
    }
}
