using Backend.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Backend.Core.Entities;
using System.Collections.Generic;
using StoreService.Persistence;
using Backend.Utils;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers {

    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class EventController : Controller {

        private IUnitOfWork _unitOfWork;

        public EventController(IUnitOfWork uow) {
            this._unitOfWork = uow;
        }

        [HttpGet]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllEvents() {
            List<Event> events = _unitOfWork.EventRepository.Get().ToList();

            if (events.Count > 0) {
                return new OkObjectResult(events);
            } else {
                return new NoContentResult();
            }
        }

        /// <summary>
        /// Creates a Event Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Authorize(Policy = "WritableFitAdmin")]
        public IActionResult CreateEventWithAreasAndLocations([FromBody] Event jsonEvent) {
            try {
                if (jsonEvent.Id > 0) {
                    return UpdateEvent(jsonEvent);
                } else {
                    return InsertEvent(jsonEvent);
                }
            } catch (DbUpdateException ex) {
                return DbErrorHelper.CatchDbError(ex);
            }
        }

        [HttpPost("presentationLock/{id}")]
        [Authorize(Policy = "WritableFitAdmin")]
        [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
        public IActionResult ChangePresentationLock(int id, [FromBody] bool presentaitonLock) {
            Event fitEvent = _unitOfWork.EventRepository.Get(e => e.Id == id).FirstOrDefault();
            fitEvent.PresentationsLocked = presentaitonLock;
            _unitOfWork.EventRepository.Update(fitEvent);
            _unitOfWork.Save();
            return new OkObjectResult(fitEvent);
        }

        [HttpGet("current")]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status200OK)]
        public IActionResult GetLatestEvent() {
            Event e;
            if ((e = this.DetermineCurrentEvent()) != null) {
                return new OkObjectResult(e);
            } else {
                return new NoContentResult();
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status200OK)]
        public IActionResult GetEvent(int id) {
            Event fitEvent = _unitOfWork.EventRepository.Get(e => e.Id == id).FirstOrDefault();
            return new OkObjectResult(fitEvent);
        }

        private IActionResult UpdateEvent(Event fitEvent) {

            Event eventToUpdate = _unitOfWork.EventRepository.Get(p => p.Id == fitEvent.Id).FirstOrDefault();

            using (IDbContextTransaction transaction = this._unitOfWork.BeginTransaction()) {
                if (eventToUpdate != null) {

                    DeleteUntrackedChildren(eventToUpdate, fitEvent);
                    ImageHelper.ManageEventGraphic(fitEvent);

                    foreach (Area area in fitEvent.Areas) {

                        area.fk_Event = fitEvent.Id;

                        if (area.Graphic != null && area.Graphic.DataUrl != null) {
                            if (area.Graphic.Id > 0) {
                                _unitOfWork.DataFileRepository.Update(area.Graphic);
                            } else {
                                _unitOfWork.DataFileRepository.Insert(area.Graphic);
                            }
                            _unitOfWork.Save();
                        }

                        List<Location> locations = _unitOfWork.LocationRepository.Get().ToList();

                        foreach (Location location in area.Locations) {
                            if (location.Id > 0) {
                                _unitOfWork.LocationRepository.Update(location);
                            } else {
                                _unitOfWork.LocationRepository.Insert(location);
                            }
                        }

                        if (area.Id > 0) {
                            _unitOfWork.AreaRepository.Update(area);
                        } else {
                            _unitOfWork.AreaRepository.Insert(area);
                        }
                        _unitOfWork.Save();
                    }

                    _unitOfWork.RegistrationStateRepository.Update(fitEvent.RegistrationState);
                    _unitOfWork.EventRepository.Update(fitEvent);
                    _unitOfWork.Save();
                    transaction.Commit();

                    this.DetermineCurrentEvent();

                    return new OkObjectResult(new {
                        changedEvent = fitEvent,
                        events = _unitOfWork.EventRepository.Get()
                    });

                } else {
                    transaction.Rollback();
                    return new BadRequestObjectResult(new {
                        errorMessage = "Der zu bearbeitende FIT konnte nicht in der Datenbank gefunden werden!"
                    });
                }
            }
        }

        private IActionResult InsertEvent(Event fitEvent) {
            fitEvent.RegistrationState.IsCurrent = true;

            if (fitEvent != null) {
                _unitOfWork.EventRepository.Insert(fitEvent);
                _unitOfWork.Save();

                ImageHelper.ManageEventGraphic(fitEvent);

                foreach (Area area in fitEvent.Areas) {
                    if (area.Graphic != null && area.Graphic.DataUrl != null) {
                        _unitOfWork.DataFileRepository.Update(area.Graphic);
                        _unitOfWork.Save();
                    }
                }

                this.DetermineCurrentEvent();

                return new OkObjectResult(new {
                    changedEvent = fitEvent,
                    events = _unitOfWork.EventRepository.Get()
                });

            } else {
                return new BadRequestObjectResult(new {
                    errorMessage = "Es sind keine Areas und Locations vorhanden!"
                });
            }
        }

        private Event DetermineCurrentEvent() {
            if (_unitOfWork.EventRepository.Count() > 0) {

                Event currentEvent = _unitOfWork.EventRepository.GetCurrentEvent();

                // if curr event is available (and not already current) set it to isCurrent true and set al other to false
                if (currentEvent != null) {
                    List<Event> events = _unitOfWork.EventRepository.Get(p => p.RegistrationState.IsCurrent == true).ToList();

                    if (events != null && events.Count > 0) {
                        foreach (Event fitEvent in events) {
                            fitEvent.RegistrationState.IsCurrent = false;
                            _unitOfWork.RegistrationStateRepository.Update(fitEvent.RegistrationState);
                        }
                        _unitOfWork.Save();
                    }

                    // check if registration is open
                    bool isLocked = DateTime.Compare(currentEvent.RegistrationStart, DateTime.Now) > 0
                        || DateTime.Compare(currentEvent.RegistrationEnd, DateTime.Now) < 0;

                    if (!currentEvent.RegistrationState.IsCurrent || currentEvent.RegistrationState.IsLocked != isLocked) {
                        currentEvent.RegistrationState.IsCurrent = true;
                        currentEvent.RegistrationState.IsLocked = isLocked;
                        _unitOfWork.RegistrationStateRepository.Update(currentEvent.RegistrationState);
                        _unitOfWork.Save();
                    }

                    return currentEvent;
                }
            }
            return null;
        }

        /// <summary>
        /// Deletes Areas and Locations which aren't in the event anymore. (They were deleted by the admin)
        /// </summary>
        /// <param name="eventToUpdate"></param>
        /// <param name="newEvent"></param>
        private void DeleteUntrackedChildren(Event eventToUpdate, Event newEvent) {

            foreach (Area area in eventToUpdate.Areas) {
                if (!newEvent.Areas.Any(a => a.Id == area.Id)) {
                    area.Locations.ForEach(l => _unitOfWork.LocationRepository.Delete(l));
                    _unitOfWork.AreaRepository.Delete(area);
                } else {
                    Area newArea = newEvent.Areas.Find(a => a.Id == area.Id);

                    foreach (Location location in area.Locations) {
                        if (!newArea.Locations.Any(l => l.Id == location.Id)) {
                            _unitOfWork.LocationRepository.Delete(location);
                        }
                    }
                }
            }

        }
    }
}