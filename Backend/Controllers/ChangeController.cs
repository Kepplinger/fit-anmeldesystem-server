using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class ChangeController
    {
        private IUnitOfWork _unitOfWork;

        public ChangeController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ChangeProtocol), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            
            List<ChangeProtocol> changes = _unitOfWork.ChangeRepository.Get().ToList();
            if (changes != null && changes.Count > 0)
            {
                return new OkObjectResult(changes);
            }
            else
            {
                return new NoContentResult();
            }
        }

        [HttpPut("apply")]
        [ProducesResponseType(typeof(ChangeProtocol), StatusCodes.Status200OK)]
        public IActionResult applyChange([FromBody] int id)
        {
            if (id != null)
            {
                ChangeProtocol c = _unitOfWork.ChangeRepository.Get(p => p.Id == id).FirstOrDefault();
                if (c != null)
                {
                    c.IsPending = false;
                    return new OkObjectResult(c);
                }
            }
            return new BadRequestResult();
        }

        [HttpPut("revert")]
        [ProducesResponseType(typeof(ChangeProtocol), StatusCodes.Status200OK)]
        public IActionResult revertChange([FromBody] int id)
        {
            if (id != null)
            {
                ChangeProtocol change = _unitOfWork.ChangeRepository.Get(filter: c => c.Id == id).FirstOrDefault();
                if (change != null)
                {
                    switch (change.TableName)
                    {
                        case "Booking":
                            Booking booking = _unitOfWork.BookingRepository.Get(filter: c => c.Id == id).FirstOrDefault();
                            var bookingInfo = booking.GetType().GetProperty(change.ColumnName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                            bookingInfo.SetValue(booking, change.OldValue, null);
                            break;
                        case "Presentation":
                            break;
                        case "Representative":
                            break;
                        case "FolderInfo":
                            break;
                        case "Contact":
                            break;
                        case "Address":
                            var comp = _unitOfWork.CompanyRepository.Get(p => p.Id == change.CompanyId, includeProperties: "Address").FirstOrDefault();
                            Address address = _unitOfWork.AddressRepository.Get(filter: c => c.Id == comp.Address.Id).FirstOrDefault();

                            var addressInfo = address.GetType().GetProperty(change.ColumnName);
                            addressInfo.SetValue(address, change.OldValue);
                            _unitOfWork.AddressRepository.Update(address);

                            change.IsPending = false;

                            _unitOfWork.ChangeRepository.Update(change);
                            _unitOfWork.Save();

                            return new OkObjectResult(change);
                        case "Company":
                            Company company = _unitOfWork.CompanyRepository.Get(p => p.Id == change.CompanyId, includeProperties: "Address").FirstOrDefault();

                            var companyProperty = company.GetType().GetProperty(change.ColumnName);
                            companyProperty.SetValue(company, change.OldValue);
                            _unitOfWork.CompanyRepository.Update(company);

                            change.IsPending = false;

                            _unitOfWork.ChangeRepository.Update(change);
                            _unitOfWork.Save();

                            return new OkObjectResult(change);
                        default:
                            return new BadRequestResult();
                    }
                }
            }
            return new BadRequestResult();
        }
    }
}
