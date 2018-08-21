﻿using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Persistence.Facades {

    public class BookingFacade {

        private IUnitOfWork _unitOfWork;
        private PresentationFacade _presentationFacade;
        private DataFileFacade _dataFileFacade;
        private RepresentativeFacade _representativeFacade;

        public BookingFacade(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
            _presentationFacade = new PresentationFacade(_unitOfWork);
            _dataFileFacade = new DataFileFacade(_unitOfWork);
            _representativeFacade = new RepresentativeFacade(_unitOfWork);
        }

        public Booking Update(Booking booking, bool protocolChanges = false, bool isAdminChange = false) {
            using (IDbContextTransaction transaction = this._unitOfWork.BeginTransaction()) {
                try {
                    Booking bookingToUpdate = _unitOfWork.BookingRepository.Get(filter: p => p.Id.Equals(booking.Id)).FirstOrDefault();
                    // booking.CreationDate = bookingToUpdate.CreationDate;

                    _presentationFacade.UpdateOrInsert(booking.Presentation);

                    ImageHelper.ManageBookingImages(booking);
                    _dataFileFacade.UpdateOrInsert(booking.Logo, false);

                    foreach (Representative representative in booking.Representatives) {
                        _representativeFacade.UpdateOrInsert(representative, false);
                    }
                    ClearDeletedRepresentatives(booking, false);

                    UpdateBookingBranches(booking);
                    UpdateResourceBookings(booking);

                    if (protocolChanges) { 
                        ChangeProtocolHelper.GenerateChangeProtocolForType(_unitOfWork, typeof(Booking), booking, bookingToUpdate, nameof(Booking), bookingToUpdate.Company.Id, isAdminChange);
                        ChangeProtocolHelper.GenerateChangeProtocolForType(_unitOfWork, typeof(Contact), booking.Contact, bookingToUpdate.Contact, nameof(Contact), bookingToUpdate.Company.Id, isAdminChange);
                    }

                    _unitOfWork.ContactRepository.Update(booking.Contact);
                    _unitOfWork.BookingRepository.Update(booking);
                    _unitOfWork.Save();
                    transaction.Commit();

                    return booking;
                } catch (DbUpdateException ex) {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        private void ClearDeletedRepresentatives(Booking booking, bool doSave = true) {

            Representative[] databaseRepresentatives = _unitOfWork.BookingRepository.GetRepresentativesOfBooking(booking);

            foreach (Representative representative in databaseRepresentatives) {
                if (!booking.Representatives.Any(r => r.Id == representative.Id)) {
                    _unitOfWork.DataFileRepository.Delete(representative.Image);
                    _unitOfWork.RepresentativeRepository.Delete(representative);
                }
            }

            if (doSave) {
                _unitOfWork.Save();
            }
        }

        private void UpdateBookingBranches(Booking booking) {
            List<BookingBranch> bookingBranches = _unitOfWork.BookingBranchesRepository
                .Get(bb => bb.fk_Booking == booking.Id, includeProperties: "Branch")
                .ToList();

            foreach (BookingBranch bookingBranch in bookingBranches) {
                int index = booking.Branches.FindIndex(bb => bb.fk_Branch == bookingBranch.fk_Branch);

                if (index != -1) {
                    booking.Branches[index] = bookingBranch;
                } else {
                    _unitOfWork.BookingBranchesRepository.Delete(bookingBranch);
                }
            }

            foreach (BookingBranch bookingBranch in booking.Branches) {
                if (bookingBranch.Id <= 0) {
                    _unitOfWork.BookingBranchesRepository.Insert(bookingBranch);
                }
            }

            _unitOfWork.Save();
        }

        private void UpdateResourceBookings(Booking booking) {
            List<ResourceBooking> resourceBookings = _unitOfWork.ResourceBookingRepository
                .Get(bb => bb.fk_Booking == booking.Id, includeProperties: "Resource")
                .ToList();

            foreach (ResourceBooking resourceBooking in resourceBookings) {
                int index = booking.Resources.FindIndex(rb => rb.fk_Resource == resourceBooking.fk_Resource);

                if (index != -1) {
                    booking.Resources[index] = resourceBooking;
                } else {
                    _unitOfWork.ResourceBookingRepository.Delete(resourceBooking);
                }
            }

            foreach (ResourceBooking resourceBooking in booking.Resources) {
                if (resourceBooking.Id <= 0) {
                    _unitOfWork.ResourceBookingRepository.Insert(resourceBooking);
                }
            }

            _unitOfWork.Save();
        }
    }
}
