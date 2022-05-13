using ASNTechnosoft.Areas.ASNAdmin.Models;
using BussinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TechnosoftModel;
using static ASNTechnosoft.Areas.ASNAdmin.Models.Enum;

namespace ASNTechnosoft.Areas.ASNAdmin.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: ASNAdmin/Dashboard
        public ActionResult Index()
        {
            List<InstituteDetails_All_Result> lstItems = InstituteBL.GetAllInstitutes();
            List<InstituteDetails_All_Result> lstEnablefinal = lstItems.Where(x => x.IMSSTATUS == "Enable").ToList();
            List<InstituteDetails_All_Result> lstTrialfinal = lstItems.Where(x => x.IMSSTATUS == "Trial").ToList();
            List<InstituteDetails_All_Result> lstDropfinal = lstItems.Where(x => x.IMSSTATUS == "Disable").ToList();
            ViewBag.TotalInstituteCount = lstItems.Count();// InstituteBL.GetAll().Count();
            ViewBag.ActiveInstituteCount = lstEnablefinal.Count();// InstituteBL.GetAllActiveInstitute().Count();
            ViewBag.InActiveInstituteCount = lstTrialfinal.Count();// InstituteBL.GetAllInActiveInstitute().Count();
            ViewBag.DropoutInstituteCount = lstDropfinal.Count();
            return View();
        }

        public ActionResult AllInstitute()
        {
            //List<Institute> lstItems = InstituteBL.GetAll();
            List<InstituteDetails_All_Result> lstItems = InstituteBL.GetAllInstitutes();
            return View(lstItems);
        }

        public ActionResult ActiveInstitute()
        {

            // List<Institute> lstItems = InstituteBL.GetAllActiveInstitute();
            List<InstituteDetails_All_Result> lstItems = InstituteBL.GetAllInstitutes();
            List<InstituteDetails_All_Result> lstfinal = lstItems.Where(x => x.IMSSTATUS == "Enable").ToList();
            return View(lstfinal);
        }

        public ActionResult InActiveInstitute()
        {

            ////List<Institute> lstItems = InstituteBL.GetAllInActiveInstitute();
            List<InstituteDetails_All_Result> lstItems = InstituteBL.GetAllInstitutes();
            List<InstituteDetails_All_Result> lstfinal = lstItems.Where(x => x.IMSSTATUS == "Trial").ToList();
            return View(lstfinal);
        }

        public ActionResult DropoutInstitute()
        {

            ////List<Institute> lstItems = InstituteBL.GetAllInActiveInstitute();
            List<InstituteDetails_All_Result> lstItems = InstituteBL.GetAllInstitutes();
            List<InstituteDetails_All_Result> lstfinal = lstItems.Where(x => x.IMSSTATUS == "Disable").ToList();
            return View(lstfinal);
        }


        public ActionResult PartialUserPanel()
        {
            User model = TempData["UserObject"] as User;
            TempData.Keep();
            if (model.RoleId == 2)
            {
                ViewBag.Designation = RoleBL.GetById(model.RoleId).Role1;
            }
            else
            {
                if (model.DesignationId != 0)
                {
                    ViewBag.Designation = DesignationBL.GetById(Convert.ToInt32(model.DesignationId)).Name;
                }

            }

            return PartialView(model);
        }
        public ActionResult PartialMenuContent()
        {
            User user = TempData["UserObject"] as User;
            TempData.Keep();
            List<PurchasedSerivce> menuaccess = PurchasedServicesBL.GetByInstituteId(user.InstituteId);
            //here if purchased item found then operation to do....
            if (menuaccess.Count() != 0)
            {
                ViewBag.MenuContent = true;
            }
            else
            {
                ViewBag.MenuContent = false;
            }


            return PartialView(user);
        }

        public ActionResult PartialTopbar()
        {
            User model = TempData["UserObject"] as User;
            TempData.Keep();
            if (model.RoleId == 2)
            {
                ViewBag.Designation = RoleBL.GetById(model.RoleId).Role1;
            }
            else
            {
                if (model.DesignationId != 0)
                {
                    ViewBag.Designation = DesignationBL.GetById(Convert.ToInt32(model.DesignationId)).Name;
                }

            }

            return PartialView(model);
        }

        private static List<SelectListItem> PopulateRejectionReason()
        {
            List<SelectListItem> RejectionReasons = new List<SelectListItem>();
            List<ReasonOfRejection> reason = ReasonOfRejectionBL.GetAll();
            reason.ForEach(x =>
            {
                RejectionReasons.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            return RejectionReasons;
        }
        public ActionResult AddFollowup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
           
            TempData.Keep();
            Institute obj = InstituteBL.GetById(Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                InstituteFollowupModel data = new InstituteFollowupModel();
                int instituteid = Convert.ToInt32(id);
                data.InstituteId = instituteid;
                data.InstituteName = obj.InstituteName;
                
                data.OwnerName = obj.FirstName + " " + obj.MiddleName + " " + obj.LastName;
                data.Mobile = obj.Mobile;
           
                data.DORegistration = obj.RegistrationDate.ToString();
                data.FollowupDate = DateTime.Now.ToString();
               
                data.StaffId = Usermodel.Id;
                data.Rejectionreasons = PopulateRejectionReason();
           
                return View("AddFollowup", data);
            }
        }

        [HttpPost]
        public ActionResult AddFollowup(InstituteFollowupModel data, string Interested)
        {
            InstituteFollowupHistory obj = new InstituteFollowupHistory();
            obj.Comment = data.FollowUpComment;
            obj.InstituteId = data.InstituteId;
            obj.FollowupDate = Convert.ToDateTime(data.FollowupDate);
            obj.UserId = data.StaffId;
            if (Interested != "on")
            {
                obj.IsDropout = false;
                obj.ReasonForDropOut = null;
            }
            else
            {
                obj.IsDropout = true;
                obj.ReasonForDropOut = data.RejectionReasonId.ToString();

                Institute instituteobj = InstituteBL.GetById(data.InstituteId);
                instituteobj.IsActive = false;
                instituteobj.Status = "Disable";
                InstituteBL.Edit(instituteobj);
            }
            if (data.NextFollowupDate != null)
            {
                obj.NextFollowupDate = Convert.ToDateTime(data.NextFollowupDate);
            }
            else
            {
                obj.NextFollowupDate = null;
            }

            obj.IsFollowup = true;
            if (InstituteFollowupHistoryBL.Add(obj) == true)
            {
                SweetAlert("Done", "Followup added Successfully", NotificationType.success);
                return RedirectToAction("InActiveInstitute", "Dashboard");
            }
            return View();
        }

      
        [HttpGet]
        public ActionResult ViewFollowup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
           
            TempData.Keep();
            Institute obj = InstituteBL.GetById(Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                InstituteFollowupModel data = new InstituteFollowupModel();
                
                
               
                data.OwnerName = obj.FirstName + " " + obj.MiddleName + " " + obj.LastName;
                data.Mobile = obj.Mobile;
                data.DORegistration = obj.RegistrationDate.ToString();
                data.InstituteName = obj.InstituteName; 
                data.StaffId = Usermodel.Id;
                List<InstituteFollowupHistory> listItems = InstituteFollowupHistoryBL.GetByInstituteId(Convert.ToInt32(id));
                data.InstituteFollowuplist = listItems;
                return View("ViewFollowup", data);
            }
        }
    }
}