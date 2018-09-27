using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class ChangeController {
        private IUnitOfWork _unitOfWork;

        public ChangeController(IUnitOfWork unitOfWork) {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Policy = "WritableAdmin")]
        [ProducesResponseType(typeof(ChangeProtocol), StatusCodes.Status200OK)]
        public IActionResult GetAll() {

            List<ChangeProtocol> changes = _unitOfWork.ChangeRepository.Get().ToList();
            if (changes != null && changes.Count > 0) {
                return new OkObjectResult(changes);
            } else {
                return new NoContentResult();
            }
        }

        [HttpPut("apply")]
        [Authorize(Policy = "WritableAdmin")]
        [ProducesResponseType(typeof(ChangeProtocol), StatusCodes.Status200OK)]
        public IActionResult ApplyChange([FromBody] int id) {
            if (id != null) {
                ChangeProtocol change = _unitOfWork.ChangeRepository.Get(p => p.Id == id).FirstOrDefault();
                if (change != null) {
                    change.IsPending = false;
                    _unitOfWork.ChangeRepository.Update(change);
                    _unitOfWork.Save();
                    return new OkObjectResult(change);
                }
            }
            return new BadRequestResult();
        }

        [HttpPut("revert")]
        [Authorize(Policy = "WritableAdmin")]
        [ProducesResponseType(typeof(ChangeProtocol), StatusCodes.Status200OK)]
        public IActionResult RevertChange([FromBody] int id) {

            if (id != null) {
                ChangeProtocol change = _unitOfWork.ChangeRepository.Get(filter: c => c.Id == id).FirstOrDefault();

                if (change != null) {
                    switch (change.TableName) {
                        case "Booking":
                            RevertChangeOnEntity<Booking>(_unitOfWork.BookingRepository, change);
                            break;
                        case "Presentation":
                            RevertChangeOnEntity<Presentation>(_unitOfWork.PresentationRepository, change);
                            break;
                        case "Representative":
                            RevertChangeOnEntity<Representative>(_unitOfWork.RepresentativeRepository, change);
                            break;
                        case "Contact":
                            RevertChangeOnEntity<Contact>(_unitOfWork.ContactRepository, change);
                            break;
                        case "Address":
                            RevertChangeOnEntity<Address>(_unitOfWork.AddressRepository, change);
                            break;
                        case "Company":
                            RevertChangeOnEntity<Company>(_unitOfWork.CompanyRepository, change);
                            break;
                        default:
                            return new BadRequestResult();
                    }

                    change.IsPending = false;
                    change.isReverted = true;

                    _unitOfWork.ChangeRepository.Update(change);
                    _unitOfWork.Save();

                    return new OkObjectResult(change);
                }
            }
            return new BadRequestResult();
        }

        private void RevertChangeOnEntity<T> (IGenericRepository<T> repository, ChangeProtocol change) where T : class, IEntityObject, new() {
            T entity = repository.Get(filter: c => c.Id == change.RowId).FirstOrDefault();
            if (entity != null) {
                PropertyInfo propertyInfo = entity.GetType().GetProperty(change.ColumnName);
                propertyInfo.SetValue(entity, Convert.ChangeType(change.OldValue, propertyInfo.PropertyType));
                repository.Update(entity);
                _unitOfWork.Save();
            }
        }
    }
}
