using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ValidationWebAPI.Models
{
    public class Validator
    {
        public void HasAmountOnInvoice(DlgInfos dlgInfos)
        {
            var Amount = dlgInfos.Values.First(n => n.FieldName == "AMOUNT");
            var DocType = dlgInfos.Values.First(n => n.FieldName == "DOCUMENT_TYPE");

            if (Amount.Item == null && DocType.Item != null && DocType.Item.ToString().ToLower() == "invoice")
            {
                throw new NoAmountFoundException("You did not povide any amount for this invoice");
            }    
        }

        public void IsPendingDateInFuture(DlgInfos dlgInfos)
        {
            var Date = dlgInfos.Values.First(n => n.FieldName == "PENDING_DATE");
            if (Date.Item == null)
            {
                throw new NoPendingDateException("Pending date has to be filled!");
            }

            if (Date.Item != null)
            {
                DateTime dateTimeNow = DateTime.UtcNow.Date;
                DateTime dateTime = DateTime.Parse(Date.Item.ToString());
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
                int projectnumber = Int32.Parse(project.Item.ToString());
                if (!(projectnumber >= 100))
                {
                    throw new InvalidProjectNumberException("Please provide a valid Project Number >= 100");
                }
            }
        }
    }
}