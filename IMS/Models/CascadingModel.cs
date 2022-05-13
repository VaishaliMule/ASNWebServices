using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.Models
{
    public class CascadingModel
    {
        public IList<SelectListItem> StateNames { get; set; }
        public IList<SelectListItem> DistrictNames { get; set; }

        //[Display(Name ="State")]
        //public int StateId { get; set; }
        //[Display(Name ="District")]
        //public int DistrictId { get; set; }

    }
}