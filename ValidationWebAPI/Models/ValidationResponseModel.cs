using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ValidationWebAPI.Models
{
    public class ValidationResponseModel
    {
        public ValidationResponseModel(ValidationResponseStatus status, string reason)
        {
            Status = status;
            Reason = reason;
        }
        [JsonConverter(typeof(StringEnumConverter))]
        public ValidationResponseStatus Status { get; set; }
        public string Reason { get; set; }
    }

    public enum ValidationResponseStatus
    {
        OK,
        Failed
    }
}