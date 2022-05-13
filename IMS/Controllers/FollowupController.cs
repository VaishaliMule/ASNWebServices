using IMS.Models;
using BussinessLayer;
using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static IMS.Models.Enum;
using IMS.Controllers;

namespace IMS.Controllers
{
    [CheckSessionOut]
    public class FollowupController : BaseController
    {
        // GET: InstituteAdmin/Followup
        public ActionResult Index()
        {
            FollowupModel data = new FollowupModel();
            List<AllEnquiryFollowupReportByInstituteId_Result> listItems = new List<AllEnquiryFollowupReportByInstituteId_Result>();
            data.EnquiryFollowuplist = listItems;
            ViewBag.ShowDiv = false;
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<SelectListItem> CourseNames = new List<SelectListItem>();
            List<AllCoursesByInstituteId_Result> courses = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));
            courses.ForEach(x =>
            {
                CourseNames.Add(new SelectListItem { Text = x.COURSENAME.ToString(), Value = x.COURSEID.ToString() });
            });
            data.CourseNames = CourseNames;
            data.InstituteId =Convert.ToInt32(Usermodel.InstituteId);
            data.Start_Date =DateTime.Today.AddMonths(-6).ToShortDateString();
            data.End_Date = DateTime.Today.ToShortDateString();
            return View("Index", data);//
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

        [HttpPost]
        public ActionResult Index(FollowupModel data)
        {
            if(data.CourseId != 0|| data.CourseId != Convert.ToInt32(null))
            {
                List<AllEnquiryFollowupReportByInstituteId_Result> listItems = EnquiryBL.GetStudentEnquiryFollowupListByInstituteId(Convert.ToDateTime(data.Start_Date), Convert.ToDateTime(data.End_Date), data.CourseId, data.InstituteId);
                data.EnquiryFollowuplist = listItems;
            }
            else
            {
                List<AllEnquiryFollowupReportByInstituteId_Result> listItems = EnquiryBL.GetStudentEnquiryFollowupListByInstituteId(Convert.ToDateTime(data.Start_Date), Convert.ToDateTime(data.End_Date), null, data.InstituteId);
                data.EnquiryFollowuplist = listItems;
            }
            
             List<SelectListItem> CourseNames = new List<SelectListItem>();
            List<AllCoursesByInstituteId_Result> courses = CourseBL.GetAllCoursesByInstitutes(data.InstituteId);
            courses.ForEach(x =>
            {
                CourseNames.Add(new SelectListItem { Text = x.COURSENAME.ToString(), Value = x.COURSEID.ToString() });
            });
            data.CourseNames = CourseNames;
            ViewBag.ShowDiv = true;
            return View("Index", data);
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
            Enquiry obj = EnquiryBL.GetById(Convert.ToInt32(id), instituteId);
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                FollowupModel data = new FollowupModel();
                int enquiryid = Convert.ToInt32(id);
                data.EnquiryId = enquiryid;
                data.InstituteId = instituteId;
                data.CourseId = obj.CourseId;
                data.StudentId = obj.StudentId;
                Student sutdObj = StudentBL.GetById(obj.StudentId, instituteId);
                data.Name = sutdObj.FirstName+" "+ sutdObj.MiddleName+" "+ sutdObj.LastName;
                data.Mobile = sutdObj.Mobile;
                data.ParentMobile = sutdObj.ParentMobile;
                data.DOEnquiry = obj.DateAndTime.ToString();
                data.FollowupDate = DateTime.Now.ToString();
                data.CourseName= CourseBL.GetById(obj.CourseId,instituteId).Name;
                data.StaffId = Usermodel.Id;
                data.Rejectionreasons = PopulateRejectionReason();
             //   data.NextFollowupDate = DateTime.Today.AddDays(7).ToShortDateString();
                return View("AddFollowup", data);
            }
        }
        [HttpPost]
        public ActionResult AddFollowup(FollowupModel data)
        {
            FollowupHistory obj = new FollowupHistory();
            obj.Comments = data.FollowUpComment;
            obj.EnquiryId = data.EnquiryId;
            obj.FacultyId = data.StaffId;
            obj.HistoryDate =Convert.ToDateTime(data.FollowupDate);
            if(data.Interested == true)
            {
                obj.Intrested = true;
                obj.ReasonIfNotIntrestedId = null;
            }
            else
            {
                obj.Intrested = false;
                obj.ReasonIfNotIntrestedId = data.RejectionReasonId;
                Enquiry enquiryobj = EnquiryBL.GetById(data.EnquiryId, data.InstituteId);
                enquiryobj.IsClosed = true;
                EnquiryBL.Edit(enquiryobj);
            }
            obj.NextFollowupDate =Convert.ToDateTime(data.NextFollowupDate);
            obj.IsFollowup = true;
            if(FollowupHistoryBL.Add(obj)==true)
            {
                SweetAlert("Done", "Followup added Successfully", NotificationType.success);
                return RedirectToAction("Index", "Followup");
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
            Enquiry obj = EnquiryBL.GetById(Convert.ToInt32(id), instituteId);
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                FollowupModel data = new FollowupModel();
                int enquiryid = Convert.ToInt32(id);
               
                Student sutdObj = StudentBL.GetById(obj.StudentId, instituteId);
                data.Name = sutdObj.FirstName + " " + sutdObj.MiddleName + " " + sutdObj.LastName;
                data.Mobile = sutdObj.Mobile;
                data.ParentMobile = sutdObj.ParentMobile;
                data.DOEnquiry = obj.DateAndTime.ToString();
               // data.FollowupDate = DateTime.Now.ToString();
                data.CourseName = CourseBL.GetById(obj.CourseId,instituteId).Name;
                data.StaffId = Usermodel.Id;
                List<FollowupofStudent_enquiryid_instituteid_Result> listItems = EnquiryBL.GetEnquiryFollowupListByInstituteId_enquiryid(enquiryid, instituteId);
                data.EnquiryFollowuplistofStudent = listItems;
                return View("ViewFollowup", data);
            }
        }
    }
}