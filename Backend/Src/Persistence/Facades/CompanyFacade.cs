using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Persistence.Facades {

    public class CompanyFacade {

        private IUnitOfWork _unitOfWork;

        public CompanyFacade(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public Company Update(Company company, bool protocolChanges = true, bool isAdminChange = false) {
            using (IDbContextTransaction transaction = this._unitOfWork.BeginTransaction()) {
                try {
                    Company companyToUpdate = _unitOfWork.CompanyRepository.Get(filter: p => p.Id.Equals(company.Id), includeProperties: "Address,Contact,Tags,Branches").FirstOrDefault();

                    UpdateCompanyBranches(company);
                    UpdateCompanyTags(company);
                    company.fk_MemberStatus = company.MemberStatus.Id;

                    if (company.Address.Id > 0) {
                        if (protocolChanges)
                            ChangeProtocolHelper.GenerateChangeProtocolForType(_unitOfWork, typeof(Address), company.Address, companyToUpdate.Address, nameof(Address), companyToUpdate.Id, isAdminChange);
                        _unitOfWork.AddressRepository.Update(company.Address);
                        _unitOfWork.Save();
                    }

                    if (company.Contact.Id > 0) {
                        if (protocolChanges)
                            ChangeProtocolHelper.GenerateChangeProtocolForType(_unitOfWork, typeof(Contact), company.Contact, companyToUpdate.Contact, nameof(Contact), companyToUpdate.Id, isAdminChange);
                        _unitOfWork.ContactRepository.Update(company.Contact);
                        _unitOfWork.Save();
                    }

                    if (company.Id > 0) {
                        company.RegistrationToken = companyToUpdate.RegistrationToken;
                        if (protocolChanges)
                            ChangeProtocolHelper.GenerateChangeProtocolForType(_unitOfWork, typeof(Company), company, companyToUpdate, nameof(Company), companyToUpdate.Id, isAdminChange);
                        _unitOfWork.CompanyRepository.Update(company);
                        _unitOfWork.Save();
                    }

                    transaction.Commit();
                    return company;

                } catch (DbUpdateException ex) {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        private void UpdateCompanyBranches(Company company) {
            List<CompanyBranch> companyBranches = _unitOfWork.CompanyBranchRepository
                .Get(bb => bb.fk_Company == company.Id, includeProperties: "Branch")
                .ToList();

            foreach (CompanyBranch companyBranch in companyBranches) {
                int index = company.Branches.FindIndex(bb => bb.fk_Branch == companyBranch.fk_Branch);

                if (index != -1) {
                    company.Branches[index] = companyBranch;
                } else {
                    _unitOfWork.CompanyBranchRepository.Delete(companyBranch);
                }
            }

            foreach (CompanyBranch companyBranch in company.Branches) {
                if (companyBranch.Id <= 0) {
                    _unitOfWork.CompanyBranchRepository.Insert(companyBranch);
                    companyBranch.Branch = _unitOfWork.BranchRepository.GetById(companyBranch.fk_Branch);
                }
            }

            _unitOfWork.Save();
        }

        private void UpdateCompanyTags(Company company) {
            List<CompanyTag> companyTags = _unitOfWork.CompanyTagRepository
                .Get(bb => bb.fk_Company == company.Id, includeProperties: "Tag")
                .ToList();

            foreach (CompanyTag companyTag in companyTags) {
                int index = company.Tags.FindIndex(bb => bb.fk_Tag == companyTag.fk_Tag);

                if (index != -1) {
                    company.Tags[index] = companyTag;
                } else {
                    _unitOfWork.CompanyTagRepository.Delete(companyTag);
                }
            }

            foreach (CompanyTag companyTag in company.Tags) {
                if (companyTag.Id <= 0) {
                    companyTag.Tag = null;
                    _unitOfWork.CompanyTagRepository.Insert(companyTag);
                    _unitOfWork.Save();
                    companyTag.Tag = _unitOfWork.TagRepository.GetById(companyTag.fk_Tag);
                }
            }

            _unitOfWork.Save();
        }
    }
}
