using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SparePartsRequests.ViewModels
{
    public class RequestApprovalViewModel
    {
        public long RequestId { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage ="الحد الأقصى للحروف هو 100 حرف")]
        public string Desc { set; get; }
       

    }
}