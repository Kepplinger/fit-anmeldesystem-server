using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models.Validation
{
    public class HomepageValidation
    {
        public static ValidationResult LinkRules(string link)
        {
            if (Uri.IsWellFormedUriString(link, UriKind.RelativeOrAbsolute) == true)
                return ValidationResult.Success;
            else
                return new ValidationResult("Non Valid Link");
        }
    }
}