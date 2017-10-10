using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;

namespace Backend.Models.Validation
{
    public static class EmailValidation
    {
         public static ValidationResult EmailRules(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return ValidationResult.Success;
            }
            catch (FormatException)
            {
                return new ValidationResult("Non Valid Email");
            }

        }
    }
}