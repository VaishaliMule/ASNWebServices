using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static IMS.Models.Enum;

namespace IMS.Controllers
{
    public class BaseController : Controller
    {
        public void SweetAlert(string title, string message, NotificationType notificationType)
        {
            var msg = "Swal.fire('" + title + "', '" + message + "','" + notificationType + "')" + "";
            TempData["Message"] = msg;
        }
        public void SweetAlertForReceipt(string title, string message, NotificationType notificationType)
        {
            var msg = "Swal.fire('" + title + "', '" + message + "','" + notificationType + "')" + "";
            TempData["ReceiptMessage"] = msg;
        }
    }
}