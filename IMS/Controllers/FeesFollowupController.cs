using IMS.Models;
using BussinessLayer;
using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IMS.Controllers;
using static IMS.Models.Enum;

namespace IMS.Controllers
{
    [CheckSessionOut]
    public class FeesFollowupController : BaseController
    {
        // GET: InstituteAdmin/FeesFollowup
        public ActionResult Index()
        {
            FeesFollowupModel data = new FeesFollowupModel();
            List<FeesFollowupReportByBatch_Result> listItems = new List<FeesFollowupReportByBatch_Result>();
            data.FeesFollowuplist = listItems;
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<SelectListItem> CourseNames = new List<SelectListItem>();
            List<AllCoursesByInstituteId_Result> courses = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));
            courses.ForEach(x =>
            {
                CourseNames.Add(new SelectListItem { Text = x.COURSENAME.ToString(), Value = x.COURSEID.ToString() });
            });
            data.CourseNames = CourseNames;
            data.BatchNames = PopulateBatches();
            ViewBag.ShowDiv = false;
            return View("Index", data);
        }
        private static List<SelectListItem> PopulateBatches()
        {
            List<SelectListItem> BatchNames = new List<SelectListItem>();

            List<Batch> batch = BatchBL.GetAll();
            batch.ForEach(x =>
            {
                BatchNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            return BatchNames;
        }

        [HttpPost]
        public ActionResult Index(FeesFollowupModel data)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<FeesFollowupReportByBatch_Result> listItems = FeesFollowupHistoryBL.GetFeesFollowupListByInstituteId_batchid(data.CourseId, data.BatchId, Convert.ToInt32(Usermodel.InstituteId));
            data.FeesFollowuplist = listItems.Where(a => a.IsComboCourseStudent == false);
            List<SelectListItem> CourseNames = new List<SelectListItem>();
            List<AllCoursesByInstituteId_Result> courses = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));
            courses.ForEach(x =>
            {
                CourseNames.Add(new SelectListItem { Text = x.COURSENAME.ToString(), Value = x.COURSEID.ToString() });
            });
            data.CourseNames = CourseNames;
            List<SelectListItem> BatchNames = new List<SelectListItem>();
            List<BatchByCourseId_InstituteId_Result> batch = BatchBL.GetBatchesofcourseid_instituteid(data.CourseId, Convert.ToInt32(Usermodel.InstituteId));
            batch.ForEach(x =>
            {
                BatchNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            data.BatchNames = BatchNames;
            ViewBag.ShowDiv = true;
            return View("Index", data);
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
        [HttpGet]
        public ActionResult AddFollowup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
            int instituteId = Convert.ToInt32(Usermodel.InstituteId);
            TempData.Keep();
            Admission obj = AdmissionBL.GetById(Convert.ToInt32(id), instituteId);
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                FeesFollowupModel data = new FeesFollowupModel();
                int Admissionid = Convert.ToInt32(id);
                data.AdmissionId = Admissionid;
                data.InstituteId = instituteId;
                data.CourseId = obj.CourseId;
                data.StudentId =Convert.ToInt32(obj.StudentId);
                Student sutdObj = StudentBL.GetById(Convert.ToInt32(obj.StudentId), instituteId);
                data.Name = sutdObj.FirstName + " " + sutdObj.MiddleName + " " + sutdObj.LastName;
                data.Mobile = sutdObj.Mobile;
                data.ParentMobile = sutdObj.ParentMobile;
                data.DOAdmission = obj.AdmissionDate.ToString();
                data.FollowupDate = DateTime.Now.ToString();
                data.CourseName = CourseBL.GetById(obj.CourseId,instituteId).Name;
                data.BatchName = BatchBL.GetById(obj.BatchId).Name;
                data.StaffId = Usermodel.Id;
              //  data.NextFollowupDate = DateTime.Today.AddDays(7).ToShortDateString();
                data.TotalBalance =Convert.ToDecimal(obj.TotalBalance);
                data.TotalFees = Convert.ToDecimal(obj.TotalFees);
                data.TotalPaid = Convert.ToDecimal(obj.TotalPaid);
                data.Discount =Convert.ToDecimal(obj.Discount);
                data.Rejectionreasons = PopulateRejectionReason();
                return View("AddFeesFollowup", data);
            }
        }

        [HttpPost]
        public ActionResult AddFollowup(FeesFollowupModel data)
        {
            FeesFollowupHistory obj = new FeesFollowupHistory();
            obj.AdmissionId = data.AdmissionId;
            obj.Comment = data.FollowUpComment;
            obj.FacultyId = data.StaffId;
            obj.FollowupDate = Convert.ToDateTime(data.FollowupDate);
            obj.NextFollowupDate = Convert.ToDateTime(data.NextFollowupDate);

            if (data.DropOut == true)
            {
                obj.IsDropout = true;
                obj.ReasonForDropOut = data.RejectionReasonId.ToString();
                Admission admobj = AdmissionBL.GetById(data.AdmissionId, data.InstituteId);
                admobj.IsDeleted = true;
                AdmissionBL.Edit(admobj);
            }
            else
            {
                obj.IsDropout = false;
                obj.ReasonForDropOut = null;
            }
            
           obj.IsFollowup = true;
          
            if (FeesFollowupHistoryBL.Add(obj) == true)
            {
                SweetAlert("Done", "Fees Followup added Successfully", NotificationType.success);
                return RedirectToAction("Index", "FeesFollowup");
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
            int instituteId = Convert.ToInt32(Usermodel.InstituteId);
            TempData.Keep();
            Admission obj = AdmissionBL.GetById(Convert.ToInt32(id), instituteId);
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                FeesFollowupModel data = new FeesFollowupModel();
                int Admissionid = Convert.ToInt32(id);
              
                Student sutdObj = StudentBL.GetById(Convert.ToInt32(obj.StudentId), instituteId);
                data.Name = sutdObj.FirstName + " " + sutdObj.MiddleName + " " + sutdObj.LastName;
                data.Mobile = sutdObj.Mobile;
                data.ParentMobile = sutdObj.ParentMobile;
                data.DOAdmission = obj.AdmissionDate.ToString();
              
                data.CourseName = CourseBL.GetById(obj.CourseId,instituteId).Name;
                data.BatchName = BatchBL.GetById(obj.BatchId).Name;
             
             
                data.TotalBalance = Convert.ToDecimal(obj.TotalBalance);
                data.TotalFees = Convert.ToDecimal(obj.TotalFees);
                data.TotalPaid = Convert.ToDecimal(obj.TotalPaid);
                data.Discount = Convert.ToDecimal(obj.Discount);
                List<StudentFeesFollowupBy_Admissionid_Result> listItems = FeesFollowupHistoryBL.GetFeesFollowupListByInstituteId_Admissionid(Admissionid, instituteId);
                data.FeesFollowuplistofStudent = listItems;
                return View("ViewFeesFollowup", data);
            }
        }

        public ActionResult PendingFees()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<AllPendingFeesForFollowup_Result> listItems = FeesFollowupHistoryBL.GetAllPendingFees(Convert.ToInt32(Usermodel.InstituteId));
            return View("ViewPendingFees", listItems);
        }

        public ActionResult OverdueFees()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<AllOverdueFeesForFollowup_Result> listItems = FeesFollowupHistoryBL.GetAllOverdueFees(Convert.ToInt32(Usermodel.InstituteId));
            return View("ViewOverdueFees", listItems);
        }

        public ActionResult TodaysFees()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<AllTodaysFeesForFollowup_Result> listItems = FeesFollowupHistoryBL.GetAllTodaysFollowupFees(Convert.ToInt32(Usermodel.InstituteId));
            return View("ViewTodaysFees", listItems);
        }
    }
}