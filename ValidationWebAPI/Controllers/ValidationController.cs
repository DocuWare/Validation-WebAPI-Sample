using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using DocuWare.Platform.ServerClient;
using Newtonsoft.Json;
using ValidationWebAPI.Models;

namespace ValidationWebAPI.Controllers
{
	public class ValidationController : ApiController
	{
		[System.Web.Http.HttpPost]
		public IHttpActionResult Post(DlgInfos dlgInfos)
		{
			PlatformClient platformClient = null;
			try
			{
				platformClient = new PlatformClient("http://localhost/", "Peters Engineering", "admin", "admin");

			
				Validator validator = new Validator();
				validator.HasAmountOnInvoice(dlgInfos);
				validator.IsPendingDateInFuture(dlgInfos);
				validator.ProjectExistsInExternalApp(dlgInfos);

				platformClient.IsDuplicate(dlgInfos);

				return Json(new ValidationResponseModel(ValidationResponseStatus.OK, "Everything was fine!"));
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse(ex));
			}
			finally
			{
				platformClient?.Logout();
			}
		}

		private ValidationResponseModel CreateErrorResponse(Exception ex)
		{
			return new ValidationResponseModel(ValidationResponseStatus.Failed, ex.Message);
		}



	}
}