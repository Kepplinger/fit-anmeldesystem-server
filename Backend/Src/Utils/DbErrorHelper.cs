using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Utils
{
    public static class DbErrorHelper
    {
        public static IActionResult CatchDbError(DbUpdateException ex)
        {
            if (ex.InnerException != null)
            {
                String error = "*********************\n\nDbUpdateException Message: " + ex.Message + "\n\n*********************\n\nInnerExceptionMessage: " + ex.InnerException.Message;
                System.Console.WriteLine(error);
                return new BadRequestObjectResult(new {
                    errorMessage = "Der Vorgang konnte nicht durchgeführt werden.",
                    exception = error
                });
            }
            else
            {
                String error = "*********************\n\nDbUpdateException Message: " + ex.Message;
                System.Console.WriteLine(error);
                return new BadRequestObjectResult(new {
                    errorMessage = "Der Vorgang konnte nicht durchgeführt werden.",
                    exception = error
                });
            }
        }
    }
}
