using Backend.Core.Contracts;
using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Persistence.Facades
{
    public class RepresentativeFacade
    {
        private IUnitOfWork _unitOfWork;
        private DataFileFacade _dataFileFacade;

        public RepresentativeFacade(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
            _dataFileFacade = new DataFileFacade(_unitOfWork);
        }

        public Representative Update(Representative representative, bool doSave = true) {

            if (representative != null) {
                _dataFileFacade.UpdateOrInsert(representative.Image, doSave);

                if (representative.Id > 0)
                    _unitOfWork.RepresentativeRepository.Update(representative);
                //else
                //    _unitOfWork.RepresentativeRepository.Insert(representative);

                if (doSave)
                    _unitOfWork.Save();
            }

            return representative;
        }
    }
}
