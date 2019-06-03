using System;
using System.Linq;
using ValidationWebAPI.Models.Exceptions;

namespace ValidationWebAPI.Models
{
    public class Validator
    {
        public void HasAmountOnInvoice(DlgInfos dlgInfos)
        {
            var amount = dlgInfos.Values.First(n => n.FieldName == "AMOUNT");
            var docType = dlgInfos.Values.First(n => n.FieldName == "DOCUMENT_TYPE");

            if (amount.Item == null && docType.Item != null && docType.Item.ToString().ToLower() == "invoice")
            {
                throw new NoAmountFoundException("You did not provide any amount for this invoice");
            }    
        }

        public void IsPendingDateInFuture(DlgInfos dlgInfos)
        {
            var date = dlgInfos.Values.First(n => n.FieldName == "PENDING_DATE");
            if (date.Item == null)
            {
                throw new NoPendingDateException("Pending date has to be filled!");
            }

            if (date.Item != null)
            {
                DateTime dateTimeNow = DateTime.UtcNow.Date;
                DateTime dateTime = DateTime.Parse(date.Item.ToString());
                if (dateTimeNow > dateTime)
                {
                    throw new PendingDateIsNotInFutureException("Please provide a pending date value in future!");
                }
            }
        }

        public void ProjectExistsInExternalApp(DlgInfos dlgInfos)
        {
            var project = dlgInfos.Values.First(n => n.FieldName == "PROJECT");
            if (project.Item == null)
            {
                throw new ProjectNumberNotSpecifiedException("Please provide a Project Number");
            }
            else
            {
                int projectNumber = Int32.Parse(project.Item.ToString());
                if (!(projectNumber >= 100))
                {
                    throw new InvalidProjectNumberException("Please provide a valid Project Number >= 100");
                }
            }
        }
    }
}