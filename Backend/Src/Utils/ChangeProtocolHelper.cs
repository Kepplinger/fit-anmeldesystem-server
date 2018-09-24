using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Src.Persistence.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Backend.Utils {

    public class ChangeProtocolHelper {

        public static void GenerateChangeProtocolForType(
            IUnitOfWork unitOfWork,
            Type type,
            IEntityObject newObject,
            IEntityObject oldObject,
            string tableName,
            int companyId,
            bool isAdminChange,
            bool doSave = false) {
            foreach (System.Reflection.PropertyInfo p in type.GetProperties()) {
                ChangeProtocolHelper.GenerateChangeProtocol(unitOfWork, p, newObject, oldObject, tableName, companyId, isAdminChange, doSave);
            }
        }

        public static void GenerateChangeProtocol(
            IUnitOfWork unitOfWork,
            PropertyInfo p,
            IEntityObject newObject,
            IEntityObject oldObject,
            string tableName,
            int companyId,
            bool isAdminChange,
            bool doSave) {

            ChangeProtocol change = new ChangeProtocol();

            if (!p.Name.ToLower().Contains("timestamp")
                && !p.Name.ToLower().Contains("id")
                && (!p.Name.ToLower().Contains("fk") || p.Name.ToLower().Contains("fk_fitpackage"))
                && !p.Name.ToLower().Contains("tags")
                && !p.Name.ToLower().Contains("branches")
                && !p.Name.ToLower().Contains("representatives")
                && !p.Name.ToLower().Contains("resources")
                && !p.Name.ToLower().Contains("contact")
                && !p.Name.ToLower().Contains("presentation")
                && !p.Name.ToLower().Contains("creationdate")
                && !p.Name.ToLower().Contains("logo")
                && !p.Name.ToLower().Contains("file")
                && p.GetValue(newObject) != null 
                && !p.GetValue(newObject).Equals(p.GetValue(oldObject))) {

                if (p != null && newObject != null && oldObject != null) {
                    change.ChangeDate = DateTime.Now;
                    change.ColumnName = p.Name;
                    change.NewValue = p.GetValue(newObject).ToString();
                    change.OldValue = p.GetValue(oldObject).ToString();
                    change.TableName = tableName;
                    change.RowId = oldObject.Id;
                    change.IsPending = true;
                    change.CompanyId = companyId;
                    change.isAdminChange = isAdminChange;
                    change.isReverted = false;
                }

                ChangelogFacade changelogFacade = new ChangelogFacade(unitOfWork);
                changelogFacade.InsertNewChange(change, doSave);
            }
        }
    }
}
