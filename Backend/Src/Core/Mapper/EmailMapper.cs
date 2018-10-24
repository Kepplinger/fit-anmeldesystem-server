using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Src.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Core.Mapper
{
    public class EmailMapper
    {
        public static Email MapDtoToEmail(EmailDTO emailTransfer, IUnitOfWork uow) {
            return new Email {
                Id = emailTransfer.Id,
                Timestamp = emailTransfer.Timestamp,
                Identifier = emailTransfer.Identifier,
                Name = emailTransfer.Name,
                Description = emailTransfer.Description,
                Subject = emailTransfer.Subject,
                Template = emailTransfer.Template,
                AvailableVariables = uow.EmailVariableUsageRepository.Get(ev => ev.fk_Email == emailTransfer.Id).ToList()
            };
        }

        public static EmailDTO MapEmailToDto(Email email) {
            return new EmailDTO {
                Id = email.Id,
                Timestamp = email.Timestamp,
                Identifier = email.Identifier,
                Name = email.Name,
                Description = email.Description,
                Template = email.Template,
                Subject = email.Subject,
                AvailableVariables = email.AvailableVariables.Select(v => v.EmailVariable).ToList()
            };
        }
    }
}
