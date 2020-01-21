using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SparePartsRequests.Models
{
    public class Request
    {
        [Key]
        public long RequestId { set; get; }
        public string Title { set; get; }
        public string Desc { set; get; }

        public long RequestTypeId { set; get; }
        public virtual RequestType RequestType { set; get; }

        public bool IsApproved { set; get; }
        public bool IsRejected { set; get; }
        public bool IsCanceled { set; get; }

        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public class RequestType
    {
        [Key]
        public long RequestTypeId { get; set; }
        public string Name { set; get; }
        public virtual ICollection<Request> Requests { get; set; }
    }

    public class Manager
    {
        [Key]
        public long NationalId { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}