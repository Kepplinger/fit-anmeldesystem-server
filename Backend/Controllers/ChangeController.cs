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


        [HttpPut("revert")]
        [ProducesResponseType(typeof(ChangeProtocol), StatusCodes.Status200OK)]
        public IActionResult revertChange(int id)
        {
            ChangeProtocol change = _unitOfWork.ChangeRepository.Get(filter: c => c.Id == id).First();
            
            switch (change.TableName)
            {
                case "Booking":
                    Booking booking = _unitOfWork.BookingRepository.Get(filter: c => c.Id == id).First();
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
                case "Addresse":
                    Address address = _unitOfWork.AddressRepository.Get(filter: c => c.Id == id).First();
                    var addressInfo = address.GetType().GetProperty(change.ColumnName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    addressInfo.SetValue(address, change.OldValue, null);
                    _unitOfWork.AddressRepository.Update(address);
                    change.IsPending = false;
                    _unitOfWork.ChangeRepository.Update(change);
                    _unitOfWork.Save();

                    return new NoContentResult();
                    break;
                case "Company":
                    Company company = _unitOfWork.CompanyRepository.Get(filter: c => c.Id == id).First();
                    var companyInfo = company.GetType().GetProperty(change.ColumnName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    companyInfo.SetValue(company, change.OldValue, null);
                    _unitOfWork.CompanyRepository.Update(company);
                    change.IsPending = false;
                    _unitOfWork.ChangeRepository.Update(change);
                    _unitOfWork.Save();

                    return new NoContentResult();
                    break;
                default:
                    return new BadRequestResult();
              
            }
            return new BadRequestResult();
        }

    }
}
