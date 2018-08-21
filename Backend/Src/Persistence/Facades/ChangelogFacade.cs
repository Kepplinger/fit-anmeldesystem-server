using Backend.Core.Contracts;
using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Persistence.Facades {

    public class ChangelogFacade {

        private IUnitOfWork _unitOfWork;

        public ChangelogFacade(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public void InsertNewChange(ChangeProtocol change, bool doSave = true) {

            ChangeProtocol existingChange = _unitOfWork.ChangeRepository.Get(
                    filter: c => c.TableName == change.TableName && c.ColumnName == change.ColumnName && c.RowId == change.RowId && c.IsPending == true
                ).FirstOrDefault();

            if (existingChange != null) {
                existingChange.NewValue = change.NewValue;

                if (existingChange.OldValue == existingChange.NewValue) {
                    _unitOfWork.ChangeRepository.Delete(existingChange);
                } else {
                    _unitOfWork.ChangeRepository.Update(existingChange);
                }
            } else {
                _unitOfWork.ChangeRepository.Insert(change);
            }

            if (doSave) {
                _unitOfWork.Save();
            }
        }

    }
}
