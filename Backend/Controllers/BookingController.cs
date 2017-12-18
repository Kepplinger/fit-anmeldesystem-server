using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class BookingController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Creates a Booking Object
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] Booking temp)
        {
            using (IDbContextTransaction transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (temp != null && temp.Company.Id != 0)
                    {
                        // Update already persistent Entities ------------------
                        Company toUpdate = _unitOfWork.CompanyRepository.Get(filter: p => p.Id == temp.Company.Id).FirstOrDefault();

                        if (toUpdate.Address != null && toUpdate.FK_Address != 0)
                        {
                            _unitOfWork.AddressRepository.Update(toUpdate.Address);
                            _unitOfWork.Save();
                        }
                        else if (toUpdate.FK_Address == 0)
                        {
                            _unitOfWork.AddressRepository.Insert(toUpdate.Address);
                            _unitOfWork.Save();
                            toUpdate.FK_Address = toUpdate.Address.Id;
                        }

                        if (toUpdate.FK_Contact != 0 && toUpdate.Contact != null)
                        {
                            _unitOfWork.ContactRepository.Update(toUpdate.Contact);
                            _unitOfWork.Save();
                        }
                        else if (toUpdate.FK_Contact == 0)
                        {
                            _unitOfWork.ContactRepository.Insert(toUpdate.Contact);
                            _unitOfWork.Save();
                            toUpdate.FK_Contact = toUpdate.Contact.Id;
                        }

                        _unitOfWork.CompanyRepository.Update(temp.Company);
                        _unitOfWork.Save();

                        transaction.Commit();

                        return new OkObjectResult(temp);
                    }
                    else if ((temp.Location.Area != null) && (temp != null && temp.Company.Id == 0))
                    {
                        // Insert new Company and Booking ----------------------
                        _unitOfWork.AddressRepository.Insert(temp.Company.Address);
                        _unitOfWork.Save();
                        _unitOfWork.ContactRepository.Insert(temp.Company.Contact);
                        _unitOfWork.Save();
                        _unitOfWork.CompanyRepository.Insert(temp.Company);
                        _unitOfWork.Save();
                        _unitOfWork.RepresentativeRepository.InsertMany(temp.Representatives);
                        _unitOfWork.Save();
                        temp.Location.Area = _unitOfWork.AreaRepository.Get(filter: p => p.Id == temp.Location.Area.Id).FirstOrDefault();
                        _unitOfWork.LocationRepository.Insert(temp.Location);
                        _unitOfWork.Save();

                        transaction.Commit();
                        return new OkObjectResult(temp);
                    }

                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();

                    String error = "*********************\n\nDbUpdateException Message: " + ex.Message + "\n\n*********************\n\nInnerExceptionMessage: " + ex.InnerException.Message;
                    System.Console.WriteLine(error);

                    return new BadRequestObjectResult(error);
                }
            }

            Console.WriteLine("Bad Request 400: Possible Problem Json Serialization: " + temp.ToString());
            return new BadRequestObjectResult(temp);
        }



        /// <summary>
        /// Returns all saved Bookings
        /// </summary>
        /// <response code="200">Returns all available Bookings</response>
        [HttpGet]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var bookings = _unitOfWork.BookingRepository.Get(includeProperties: "Event,Branches,Company,Package,Location,Presentation");
            return new ObjectResult(bookings);
        }

        /// <response code="200">Returns the available bookings with the </response>
        /// <summary>
        /// Getting all bookings from Database
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
        public IActionResult GetById(int id)
        {
            var bookings = _unitOfWork.BookingRepository.GetById(id);
            return new ObjectResult(bookings);
        }
    }
}
