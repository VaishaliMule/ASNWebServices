using BussinessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TechnosoftModel;
using static ASNTechnosoft.Areas.ASNAdmin.Models.Enum;

namespace ASNTechnosoft.Areas.ASNAdmin.Controllers
{
    public class ServicesController : BaseController
    {
        public ActionResult Index()
        {
            List<SubscriptionService> lstItems = SubscriptionServicesBL.GetAll();
            return View(lstItems);
        }


        [HttpGet]
        public ActionResult Create()
        {
            SubscriptionService dataModel = new SubscriptionService();
            return View("_CreateService", dataModel);
        }

        [HttpPost]
        public ActionResult Create(SubscriptionService data, HttpPostedFileBase PhotoImageFile)
        {
            if (ModelState.IsValid)
            {
                SubscriptionService obj = new SubscriptionService();
                obj.Description = data.Description;
                obj.IsDeleted = false;
                obj.Quantity = data.Quantity;
                obj.ServiceAmount = data.ServiceAmount;
                obj.ServiceName = data.ServiceName;

                if (PhotoImageFile != null && PhotoImageFile.ContentLength > 0)
                {
                    string FileName = Path.GetFileNameWithoutExtension(PhotoImageFile.FileName);
                    string FileExtension = Path.GetExtension(PhotoImageFile.FileName);
                    FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                    string UploadPath = Path.Combine(Server.MapPath("~/ServiceLogo"), FileName);
                    data.Logo = UploadPath.ToString();
                    PhotoImageFile.SaveAs(data.Logo);
                    obj.Logo = data.Logo.ToString();
                    //  data.LogoFileName = FileName;
                    obj.LogoFileName = FileName;
                }

                if (SubscriptionServicesBL.Add(obj) == true)
                {
                    SweetAlert("Done", "Service added Successfully", NotificationType.success);
                    return RedirectToAction("AllServices", "Services");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SubscriptionService obj = SubscriptionServicesBL.GetById(Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                SubscriptionService data = new SubscriptionService();
                data.ServiceName = obj.ServiceName;
                data.ServiceAmount = obj.ServiceAmount;
                data.Quantity = obj.Quantity;
                data.Logo = obj.Logo;
                data.LogoFileName = obj.LogoFileName;
                data.IsDeleted = false;
                data.Description = obj.Description;
                return View("_EditService", data);
            }

        }

        [HttpPost]
        public ActionResult Edit(SubscriptionService data, HttpPostedFileBase PhotoImageFile)
        {
            if (data.Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SubscriptionService obj = SubscriptionServicesBL.GetById(Convert.ToInt32(data.Id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    obj.Description = data.Description;
                    obj.IsDeleted = false;
                    obj.Quantity = data.Quantity;
                    obj.ServiceAmount = data.ServiceAmount;
                    obj.ServiceName = data.ServiceName;
                    if (PhotoImageFile != null && PhotoImageFile.ContentLength > 0)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(PhotoImageFile.FileName);
                        string FileExtension = Path.GetExtension(PhotoImageFile.FileName);
                        FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                        string UploadPath = Path.Combine(Server.MapPath("~/ServiceLogo"), FileName);
                        data.Logo = UploadPath.ToString();
                        PhotoImageFile.SaveAs(data.Logo);
                        obj.Logo = data.Logo.ToString();
                        obj.LogoFileName = FileName;
                    }
                    if (SubscriptionServicesBL.Edit(obj) == true)
                    {
                        SweetAlert("Done", "Service updated Successfully", NotificationType.success);
                        return RedirectToAction("AllServices", "Services");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "description of model error found");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SubscriptionService obj = SubscriptionServicesBL.GetById(Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                SubscriptionService data = new SubscriptionService();
                data.ServiceName = obj.ServiceName;
                data.ServiceAmount = obj.ServiceAmount;
                data.Quantity = obj.Quantity;
                data.Logo = obj.Logo;
                data.LogoFileName = obj.LogoFileName;
                data.IsDeleted = false;
                data.Description = obj.Description;
                return View("_ViewService", data);
            }
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SubscriptionService obj = SubscriptionServicesBL.GetById(Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                SubscriptionService data = new SubscriptionService();
                data.ServiceName = obj.ServiceName;
                data.ServiceAmount = obj.ServiceAmount;
                data.Quantity = obj.Quantity;
                data.Logo = obj.Logo;
                data.IsDeleted = false;
                data.Description = obj.Description;
                return View("_DeleteService", data);
            }
        }
        [HttpPost]
        public ActionResult Delete(SubscriptionService data)
        {
            if (data.Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SubscriptionService obj = SubscriptionServicesBL.GetById(Convert.ToInt32(data.Id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                SubscriptionServicesBL.Delete(obj);
                SweetAlert("Delete", "Service deleted Successfully", NotificationType.success);
                return RedirectToAction("AllServices", "Services");
            }
        }

        [HttpPost]
        public ActionResult ExistOrNot(string servicename)
        {
            SubscriptionService service = SubscriptionServicesBL.GetByServiceName(servicename);
            return Json(service.Id, JsonRequestBehavior.AllowGet);
        }
    }
}