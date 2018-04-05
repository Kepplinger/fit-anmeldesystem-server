using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backend.Utils;

namespace Backend.Controllers
{
    public class EmailController : Controller
    {
        public IActionResult GetAllEmailName()
        {
            return new OkObjectResult(EmailHelper.emailTemplates);
        }

        public void
    }
}
