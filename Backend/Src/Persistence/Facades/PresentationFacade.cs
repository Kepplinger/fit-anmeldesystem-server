using Backend.Core.Contracts;
using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Persistence.Facades {

    public class PresentationFacade {

        private IUnitOfWork _unitOfWork;
        private DataFileFacade _dataFileFacade;

        public PresentationFacade(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
            _dataFileFacade = new DataFileFacade(_unitOfWork);
        }

        /// <summary>
        /// Inserts if no id and updates if id exists
        /// </summary>
        /// <param name="presentation"></param>
        /// <returns></returns>
        public Presentation UpdateOrInsert(Presentation presentation) {
            if (presentation != null) {
                if (presentation.Id > 0)
                    this.Update(presentation);
                else {
                    _unitOfWork.PresentationRepository.Insert(presentation);
                    _unitOfWork.Save();
                }
            }

            return presentation;
        }

        public Presentation Update(Presentation presentation) {

            List<PresentationBranch> presentationBranches = _unitOfWork.PresentationBranchesRepository
                .Get(pb => pb.fk_Presentation == presentation.Id, includeProperties: "Branch")
                .ToList();

            _dataFileFacade.UpdateOrInsert(presentation.File, false);

            foreach (PresentationBranch presentationBranch in presentationBranches) {
                int index = presentation.Branches.FindIndex(pb => pb.fk_Branch == presentationBranch.fk_Branch);

                if (index != -1) {
                    presentation.Branches[index] = presentationBranch;
                } else {
                    _unitOfWork.PresentationBranchesRepository.Delete(presentationBranch);
                }
            }

            foreach (PresentationBranch presentationBranch in presentation.Branches) {
                if (presentationBranch.Id <= 0) {
                    _unitOfWork.PresentationBranchesRepository.Insert(presentationBranch);
                }
            }

            _unitOfWork.PresentationRepository.Update(presentation);
            _unitOfWork.Save();

            presentation = _unitOfWork.PresentationRepository.Get(filter: p => p.Id == presentation.Id).FirstOrDefault();

            return presentation;
        }
    }
}
