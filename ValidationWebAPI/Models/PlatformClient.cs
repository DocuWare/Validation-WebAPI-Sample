using DocuWare.Platform.ServerClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ValidationWebAPI.Models
{
    class PlatformClient
    {
        ServiceConnection connector;
        Organization org;

        const string urlFormatString = @"{0}/docuware/platform";


        /// <summary> Constructor creating connection using DocuWare user's credential. </summary>
        /// <param name="serverUrl"> The URL of the server Platform is running on. </param>
        /// <param name="organizationName"> Name of the organization you want the client to be connected to.</param>
        /// <param name="userName"> User to use when connecting to the organization specified.</param>
        /// <param name="userPassword"> Password of the user specified. </param>
        /// <remarks>
        /// When this constructor is running a connection to DocuWare Platform will be created; this action potentially consumes a license.
        /// "Potentially" means that not every run of this constructor will consume a license, in many cases the license will be re-used.
        /// We are not going into the details here because the underlying implementation regarding license consumption can be changed in the future.
        /// </remarks>
        public PlatformClient(string serverUrl, string organizationName, string userName, string userPassword)
        {

            this.connector = ServiceConnection.Create(new System.Uri(String.Format(urlFormatString, serverUrl)),
                                                      userName: userName,
                                                      password: userPassword,
                                                      organization: organizationName);

            this.org = this.connector.Organizations[0];
        }


        /// <summary>
        /// Gives you access to all file cabinets the user that has been used when creating this instance of PlatformClient has access to.
        /// </summary>
        public IEnumerable<FileCabinet> GetAllFileCabinetsUserHasAccessTo()
        {
            return (from fileCabinet in this.org.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                    where fileCabinet.IsBasket == false
                    select fileCabinet);
        }


        /// <summary>
        /// Gives you access to document trays (baskets) the user that has been used when creating this instance of PlatformClient has access to.
        /// </summary>
        public IEnumerable<FileCabinet> GetAllDocumentTraysUserHasAccessTo()
        {
            return (from fileCabinet in this.org.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                    where fileCabinet.IsBasket == true
                    select fileCabinet);
        }


        /// <summary> Use it to access to a particular file cabinet. </summary>
        /// <param name="fileCabinetName"> Name of the file cabinet (case insensitive). </param>
        /// <returns> FileCabinet or null if no one found. </returns>
        public FileCabinet GetFileCabinet(string fileCabinetName)
        {
            return (from fileCabinet in GetAllFileCabinetsUserHasAccessTo()
                    where String.Compare(fileCabinet.Id, fileCabinetName, ignoreCase: true) == 0
                    select fileCabinet).SingleOrDefault();
        }


        /// <summary> Use it to access to a particular document tray (basket). </summary>
        /// <param name="documentTrayName"> Name of the file cabinet (case insensitive). </param>
        /// <returns> Document tray (basket) or null if no one found. </returns>
        public FileCabinet GetDocumentTray(string documentTrayName)
        {
            return (from documentTray in GetAllDocumentTraysUserHasAccessTo()
                    where String.Compare(documentTray.Name, documentTrayName, ignoreCase: true) == 0
                    select documentTray).SingleOrDefault();
        }


        /// <summary> Use it if you are searching for documents. </summary>
        /// <param name="targetName"> The name if file cabinet or document tray (basket). </param>
        /// <param name="isDocumentTray"> Specifies if the target is a document tray (basket). </param>
        /// <param name="query"> Searching criteria. </param>
        /// <returns> List containing documents found. </returns>
        public List<Document> GetDocumentsByQuery(string targetName, bool isDocumentTray, DialogExpression query)
        {
            var target = isDocumentTray ? GetDocumentTray(targetName) : GetFileCabinet(targetName);
            var searchDialog = getDefaultSearchDialog(target);

            return runQueryForDocuments(searchDialog, query).Items;
        }

        #region Private methods

        private Dialog getDefaultSearchDialog(FileCabinet fileCabinet)
        {
            return fileCabinet.GetDialogInfosFromSearchesRelation().Dialog.Where(dlg => dlg.IsDefault == !fileCabinet.IsBasket).FirstOrDefault().GetDialogFromSelfRelation();
        }

        private DocumentsQueryResult runQueryForDocuments(Dialog dialog, DialogExpression query)
        {
            return dialog.Query.PostToDialogExpressionRelationForDocumentsQueryResult(query);
        }

        #endregion

        public void IsDuplicate(DlgInfos dlgInfos)
        {

            var query = new DialogExpression()
            {
                Operation = DialogExpressionOperation.And,
                Condition = GetDialogExpressionConditions(dlgInfos)
            };
            int count = GetDocumentsByQuery(dlgInfos.FileCabinetGuid, isDocumentTray: false, query: query).Count;
            if (count > 0)
            {
                throw new DuplicateDocumentException("Document with same index data already exists!");
            }

        }

        public static List<DialogExpressionCondition> GetDialogExpressionConditions(DlgInfos dlgInfos)
        {
            List<DialogExpressionCondition> dialogExpressionConditions = new List<DialogExpressionCondition>();
            foreach (var item in dlgInfos.Values)
            {
                if (item.Item != null && item.ItemElementName.ToLower() == "string")
                {
                    dialogExpressionConditions.Add(DialogExpressionCondition.Create(fieldName: item.FieldName, value: item.Item.ToString()));
                }
                if (item.Item != null && item.ItemElementName.ToLower() == "decimal")
                {
                    dialogExpressionConditions.Add(DialogExpressionCondition.Create(fieldName: item.FieldName, value: item.Item.ToString()));
                }
                if (item.Item != null && item.ItemElementName.ToLower() == "int")
                {
                    dialogExpressionConditions.Add(DialogExpressionCondition.Create(fieldName: item.FieldName, value: item.Item.ToString()));
                }
                if (item.Item != null && item.ItemElementName.ToLower() == "memo")
                {
                    dialogExpressionConditions.Add(DialogExpressionCondition.Create(fieldName: item.FieldName, value: item.Item.ToString()));
                }
                if (item.Item != null && item.ItemElementName.ToLower() == "date")
                {
                    DateTime dateTime = DateTime.Parse(item.Item.ToString());
                    dialogExpressionConditions.Add(DialogExpressionCondition.Create(fieldName: item.FieldName, value: dateTime.Date.ToString("s",CultureInfo.CreateSpecificCulture("en-US"))));
                }
                if (item.Item != null && item.ItemElementName.ToLower() == "datetime")
                {
                    DateTime dateTime = DateTime.Parse(item.Item.ToString());
                    dialogExpressionConditions.Add(DialogExpressionCondition.Create(fieldName: item.FieldName, value: dateTime.ToString("s", CultureInfo.CreateSpecificCulture("en-US"))));
                }
                if (item.Item != null && item.ItemElementName.ToLower() == "keywords")
                {
                    var key = JsonConvert.DeserializeObject<DocumentIndexFieldKeywords>(item.Item.ToString());

                    foreach (var keyword in key.Keyword)
                    {
                        dialogExpressionConditions.Add(DialogExpressionCondition.Create(item.FieldName, keyword));
                    }
                }
            }
            return dialogExpressionConditions;
        }


    }
    public class DlgField
    {
        public string FieldName { get; set; }
        public string ItemElementName { get; set; }
        public object Item { get; set; }
    }

    public class DlgInfos
    {
        public DateTime TimeStamp { get; set; }
        public string UserName { get; set; }
        public string OrganizationName { get; set; }
        public string FileCabinetGuid { get; set; }
        public string DialogGuid { get; set; }
        public string DialogType { get; set; }
        public List<DlgField> Values { get; set; }
    }

}
