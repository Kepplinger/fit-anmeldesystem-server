using Backend.Core.Contracts;
using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Persistence.Facades
{
    public class DataFileFacade
    {
        private IUnitOfWork _unitOfWork;

        public DataFileFacade(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Inserts if no id and updates if id exists
        /// </summary>
        /// <param name="dataFile"></param>
        /// <returns></returns>
        public DataFile UpdateOrInsert(DataFile dataFile, bool doSave = true) {

            if (dataFile != null) {
                if (dataFile.Id > 0)
                    _unitOfWork.DataFileRepository.Update(dataFile);
                else
                    _unitOfWork.DataFileRepository.Insert(dataFile);

                if (doSave)
                    _unitOfWork.Save();                
            }

            return dataFile;
        }
    }
}
