using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BussinessLayer;
using TechnosoftModel;
using System.Web.Mvc;
using static ASNTechnosoft.Areas.Admin.Models.Enum;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using ASNTechnosoft.Areas.Admin.Models;

namespace ASNTechnosoft.Areas.Admin.Controllers
{
    [CheckSessionOut]
    public class ServicesController : BaseController
    {
        // GET: Admin/Services
        public ActionResult Index()
        {
            List<SubscriptionService> lstItems = SubscriptionServicesBL.GetAll();
            ViewBag.Item = lstItems;
            return View(lstItems);
        }

        //public ActionResult AllServices()
        //{
        //    List<SubscriptionService> lstItems = SubscriptionServicesBL.GetAll();
        //    return View(lstItems);
        //}

        //[HttpGet]
        //public ActionResult Create()
        //{
        //    SubscriptionService dataModel = new SubscriptionService();
        //    return View("_CreateService", dataModel);
        //}

        //[HttpPost]
        //public ActionResult Create(SubscriptionService data, HttpPostedFileBase PhotoImageFile)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        SubscriptionService obj = new SubscriptionService();
        //        obj.Description = data.Description;
        //        obj.IsDeleted = false;
        //        obj.Quantity = data.Quantity;
        //        obj.ServiceAmount = data.ServiceAmount;
        //        obj.ServiceName = data.ServiceName;

        //        if (PhotoImageFile != null && PhotoImageFile.ContentLength > 0)
        //        {
        //            string FileName = Path.GetFileNameWithoutExtension(PhotoImageFile.FileName);
        //            string FileExtension = Path.GetExtension(PhotoImageFile.FileName);
        //            FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
        //            string UploadPath = Path.Combine(Server.MapPath("~/ServiceLogo"), FileName);
        //            data.Logo = UploadPath.ToString();
        //            PhotoImageFile.SaveAs(data.Logo);
        //            obj.Logo = data.Logo.ToString();
        //          //  data.LogoFileName = FileName;
        //            obj.LogoFileName = FileName;
        //        }

        //        if (SubscriptionServicesBL.Add(obj) == true)
        //        {
        //        SweetAlert("Done", "Service added Successfully", NotificationType.success);
        //        return RedirectToAction("AllServices", "Services");
        //        }
        //    }

        //  return View();
        //}

        //[HttpGet]
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    SubscriptionService obj = SubscriptionServicesBL.GetById(Convert.ToInt32(id));
        //    if (obj == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    else
        //    {
        //        SubscriptionService data = new SubscriptionService();
        //        data.ServiceName = obj.ServiceName;
        //        data.ServiceAmount = obj.ServiceAmount;
        //        data.Quantity = obj.Quantity;
        //        data.Logo = obj.Logo;
        //        data.LogoFileName = obj.LogoFileName;
        //        data.IsDeleted = false;
        //        data.Description = obj.Description;
        //        return View("_EditService", data);
        //    }

        //}

        //[HttpPost]
        //public ActionResult Edit(SubscriptionService data, HttpPostedFileBase PhotoImageFile)
        //{
        //    if (data.Id == 0)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    SubscriptionService obj = SubscriptionServicesBL.GetById(Convert.ToInt32(data.Id));
        //    if (obj == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    else
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            obj.Description = data.Description;
        //            obj.IsDeleted = false;
        //            obj.Quantity = data.Quantity;
        //            obj.ServiceAmount = data.ServiceAmount;
        //            obj.ServiceName = data.ServiceName;
        //            if (PhotoImageFile != null && PhotoImageFile.ContentLength > 0)
        //            {
        //                string FileName = Path.GetFileNameWithoutExtension(PhotoImageFile.FileName);
        //                string FileExtension = Path.GetExtension(PhotoImageFile.FileName);
        //                FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
        //                string UploadPath = Path.Combine(Server.MapPath("~/ServiceLogo"), FileName);
        //                data.Logo = UploadPath.ToString();
        //                PhotoImageFile.SaveAs(data.Logo);
        //                obj.Logo = data.Logo.ToString();
        //                obj.LogoFileName = FileName;
        //            }
        //            if (SubscriptionServicesBL.Edit(obj) == true)
        //            {
        //            SweetAlert("Done", "Service updated Successfully", NotificationType.success);
        //            return RedirectToAction("AllServices", "Services");
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "description of model error found");
        //        }
        //    }
        //    return View();
        //}

        //[HttpGet]
        //public ActionResult View(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    SubscriptionService obj = SubscriptionServicesBL.GetById(Convert.ToInt32(id));
        //    if (obj == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    else
        //    {
        //        SubscriptionService data = new SubscriptionService();
        //        data.ServiceName = obj.ServiceName;
        //        data.ServiceAmount = obj.ServiceAmount;
        //        data.Quantity = obj.Quantity;
        //        data.Logo = obj.Logo;
        //        data.LogoFileName = obj.LogoFileName;
        //        data.IsDeleted = false;
        //        data.Description = obj.Description;
        //        return View("_ViewService", data);
        //    }
        //}

        //[HttpGet]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    SubscriptionService obj = SubscriptionServicesBL.GetById(Convert.ToInt32(id));
        //    if (obj == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    else
        //    {
        //        SubscriptionService data = new SubscriptionService();
        //        data.ServiceName = obj.ServiceName;
        //        data.ServiceAmount = obj.ServiceAmount;
        //        data.Quantity = obj.Quantity;
        //        data.Logo = obj.Logo;
        //        data.IsDeleted = false;
        //        data.Description = obj.Description;
        //        return View("_DeleteService", data);
        //    }
        //}
        //[HttpPost]
        //public ActionResult Delete(SubscriptionService data)
        //{
        //    if (data.Id == 0)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    SubscriptionService obj = SubscriptionServicesBL.GetById(Convert.ToInt32(data.Id));
        //    if (obj == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    else
        //    {
        //        SubscriptionServicesBL.Delete(obj);
        //        SweetAlert("Delete", "Service deleted Successfully", NotificationType.success);
        //        return RedirectToAction("AllServices","Services");
        //    }
        //}

        //[HttpPost]
        //public ActionResult ExistOrNot(string servicename)
        //{
        //    SubscriptionService service = SubscriptionServicesBL.GetByServiceName(servicename);
        //    return Json(service.Id, JsonRequestBehavior.AllowGet);
        //}




        private static string WebAPIURL = " http://webapi.asncomputer.com/";    /////"https://localhost:44325/";

        public async Task<ActionResult> ServiceLogin(int? id)
        {
            User model = TempData["UserObject"] as User;
            TempData.Keep();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string url;

            Institute instituteObj = InstituteBL.GetById(model.InstituteId);
            int serviceId = Convert.ToInt32(id);
            if (instituteObj.Status == "Trial")
            {
                if (serviceId == 1)
                {
                    var tokenBased = string.Empty;
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.BaseAddress = new Uri(WebAPIURL);
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(mediaType: "application/json"));
                        var responseMessage = await client.GetAsync(requestUri: "Account/ValidLogin?userName=" + model.Username + "&userPassword=" + model.Password);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var resultMessage = responseMessage.Content.ReadAsStringAsync().Result;
                            tokenBased = JsonConvert.DeserializeObject<string>(resultMessage);
                            Session["TokenNumber"] = tokenBased;
                            Session["UserName"] = model.Username;
                        }
                    }

                    url = "http://ims.asntechnosoft.com/Account/Login?TokenNumber=" + tokenBased.ToString();
                    return Redirect(url);
                }

                if (serviceId == 2)  //2003
                {
                    url = "http://panseva.asntechnosoft.com";
                    return Redirect(url);
                }

                if (serviceId == 3)///2006
                {
                    url = "https://pmjay.gov.in/";
                    return Redirect(url);
                }
            }

            if (instituteObj.Status == "Enable")
            {
                PurchasedSerivce serviceAccess = PurchasedServicesBL.GetBySubscriptionId(Convert.ToInt32(id), model.InstituteId);

                if (serviceAccess != null)
                {
                    if (serviceAccess.SubscriptionServiceId == 1 && serviceAccess.Status == "Enable")
                    {
                        var tokenBased = string.Empty;
                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Clear();
                            client.BaseAddress = new Uri(WebAPIURL);
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(mediaType: "application/json"));
                            var responseMessage = await client.GetAsync(requestUri: "Account/ValidLogin?userName=" + model.Username + "&userPassword=" + model.Password);
                            if (responseMessage.IsSuccessStatusCode)
                            {
                                var resultMessage = responseMessage.Content.ReadAsStringAsync().Result;
                                tokenBased = JsonConvert.DeserializeObject<string>(resultMessage);
                                Session["TokenNumber"] = tokenBased;
                                Session["UserName"] = model.Username;
                            }
                        }

                        url = "http://ims.asntechnosoft.com/Account/Login?TokenNumber=" + tokenBased.ToString();
                        return Redirect(url);
                    }
                    else
                    {
                        SweetAlert("Info", "Service status is not active. Please contact Administrator.", NotificationType.info);
                        return RedirectToAction("Index", "EOI");
                    }

                    if (serviceAccess.SubscriptionServiceId == 2)  //2003
                    {
                        url = "http://panseva.asntechnosoft.com";
                        return Redirect(url);
                    }

                    if (serviceAccess.SubscriptionServiceId == 3)///2006
                    {
                        url = "https://pmjay.gov.in/";
                        return Redirect(url);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "EOI");
                }
            }

            if (instituteObj.Status == "Disable")
            {
                SweetAlert("Info", "User status is not active. Please contact Administrator.", NotificationType.info);
                return RedirectToAction("Index", "EOI");
            }

            return View();
        }
    }

}