using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BussinessLayer;
using IMS.Models;
using TechnosoftModel;
using static IMS.Models.Enum;

namespace IMS.Controllers
{
    public class BatchController : BaseController
    {
        // GET: InstituteAdmin/Batch
        public ActionResult Index()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<AllBatchesByInstituteId_Result> listItems = BatchBL.GetAllBatchesByInstituteId(Convert.ToInt32(Usermodel.InstituteId));
            return View(listItems);
        }

        private static List<SelectListItem> PopulateDuration()
        {
            List<SelectListItem> DurationNames = new List<SelectListItem>();
            List<Duration> durations = DurationBL.GetAll();
            durations.ForEach(x =>
            {
                DurationNames.Add(new SelectListItem { Text = x.DurationInMonths.ToString(), Value = x.Id.ToString() });
            });
            return DurationNames;
        }

        [HttpGet]
        public ActionResult Create()
        {
            BatchModel dataModel = new BatchModel();
            dataModel.DurationNames = PopulateDuration();
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            dataModel.InstituteId = Convert.ToInt32(Usermodel.InstituteId);

            List<SelectListItem> CourseNames = new List<SelectListItem>();

            List<AllCoursesByInstituteId_Result> courses = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));
            courses.ForEach(x =>
            {
                CourseNames.Add(new SelectListItem { Text = x.COURSENAME.ToString(), Value = x.COURSEID.ToString() });
            });
            dataModel.CourseNames = CourseNames;

            ViewBag.Months = new SelectList(Enumerable.Range(1, 12).Select(x =>
             new SelectListItem()
             {
                 Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[x - 1],
                 Value = x.ToString()
             }), "Value", "Text");

            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 01);
            dataModel.Start_Date = startDate.ToString("MM/dd/yyyy");

            var lastDate = startDate.AddMonths(1).AddDays(-1);
            dataModel.AdmissionLast_Date = lastDate.ToShortDateString();

            ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year, 1).Select(x =>
               new SelectListItem()
               {
                   Text = x.ToString(),
                   Value = x.ToString()
               }), "Value", "Text");

            dataModel.BatchMonth = DateTime.Today.Month.ToString();
            dataModel.BatchYear = DateTime.Today.Year;

            return View("_CreateBatch", dataModel);
        }

        [HttpPost]
        public ActionResult Create(BatchModel data)
        {
            if (ModelState.IsValid)
            {
                Batch obj = new Batch();
                obj.AdmissionLastDate = Convert.ToDateTime(data.AdmissionLast_Date);
                obj.BatchMonth = data.BatchMonth;
                obj.BatchYear = data.BatchYear;
                obj.CourseId = data.CourseId;
                obj.Durationid = data.DurationId;
                obj.EndDate = Convert.ToDateTime(data.End_Date);
                obj.IntakeCapacity = data.InTakeCapacity;
                obj.IsClosed = false;
                obj.IsLocked = false;
                obj.Name = data.Name;
                obj.StartDate = Convert.ToDateTime(data.Start_Date);
                obj.Target = data.Target;
                obj.IsDeleted = false;
                if (BatchBL.Add(obj) == true)
                {
                    SweetAlert("Done", "Batch added Successfully", NotificationType.success);
                    return RedirectToAction("Index", "Batch");
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult ExistOrNot(string batchname, string instituteid)
        {
            BatchesDetails_Batchid_InstituteId_Result batch = BatchBL.GetBatchbyName(Convert.ToInt32(instituteid), batchname);
            return Json(batch, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Batch obj = BatchBL.GetById(Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                BatchModel data = new BatchModel();
                data.BatchId = Convert.ToInt32(id);
                data.AdmissionLast_Date = obj.AdmissionLastDate.ToShortDateString();
                data.DurationId = obj.Durationid;
                data.DurationNames = PopulateDuration();
                User Usermodel = TempData["UserObject"] as User;
                TempData.Keep();
                data.InstituteId = Convert.ToInt32(Usermodel.InstituteId);
                data.Name = obj.Name;
                data.End_Date = obj.EndDate.ToShortDateString();
                data.InTakeCapacity = Convert.ToInt32(obj.IntakeCapacity);
                data.Start_Date = obj.StartDate.ToShortDateString();
                data.Target = Convert.ToInt32(obj.Target);

                List<SelectListItem> CourseNames = new List<SelectListItem>();
                List<AllCoursesByInstituteId_Result> courses = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));
                courses.ForEach(x =>
                {
                    CourseNames.Add(new SelectListItem { Text = x.COURSENAME.ToString(), Value = x.COURSEID.ToString() });
                });
                data.CourseNames = CourseNames;
                data.CourseId = obj.CourseId;


                ViewBag.Months = new SelectList(Enumerable.Range(1, 12).Select(x =>
                 new SelectListItem()
                 {
                     Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[x - 1],
                     Value = x.ToString()
                 }), "Value", "Text");
                data.BatchMonth = obj.BatchMonth;

                ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year, 1).Select(x =>
                   new SelectListItem()
                   {
                       Text = x.ToString(),
                       Value = x.ToString()
                   }), "Value", "Text");
                data.BatchYear = obj.BatchYear;


                return View("_EditBatch", data);
            }

        }

        [HttpPost]
        public ActionResult Edit(BatchModel data)
        {
            if (data.BatchId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Batch obj = BatchBL.GetById(Convert.ToInt32(data.BatchId));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    obj.AdmissionLastDate = Convert.ToDateTime(data.AdmissionLast_Date);
                    obj.BatchMonth = data.BatchMonth;
                    obj.BatchYear = data.BatchYear;
                    obj.CourseId = data.CourseId;
                    obj.Durationid = data.DurationId;
                    obj.IsDeleted = false;
                    obj.EndDate = Convert.ToDateTime(data.End_Date);
                    obj.IntakeCapacity = data.InTakeCapacity;
                    obj.IsClosed = false;
                    obj.IsLocked = false;
                    obj.Name = data.Name;
                    obj.StartDate = Convert.ToDateTime(data.Start_Date);
                    obj.Target = data.Target;


                    if (BatchBL.Edit(obj) == true)
                    {
                        SweetAlert("Done", "Batch updated Successfully", NotificationType.success);
                        return RedirectToAction("Index", "Batch");
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
            Batch obj = BatchBL.GetById(Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                BatchModel data = new BatchModel();
                data.BatchId = Convert.ToInt32(id);
                data.AdmissionLast_Date = obj.AdmissionLastDate.ToShortDateString();
                data.DurationId = obj.Durationid;
                Duration duration = DurationBL.GetById(obj.Durationid);
                data.Duration = duration.DurationInMonths.ToString();
                User Usermodel = TempData["UserObject"] as User;
                TempData.Keep();
                data.InstituteId = Convert.ToInt32(Usermodel.InstituteId);
                data.Name = obj.Name;
                data.End_Date = obj.EndDate.ToShortDateString();
                data.InTakeCapacity = Convert.ToInt32(obj.IntakeCapacity);
                data.Start_Date = obj.StartDate.ToShortDateString();
                data.Target = Convert.ToInt32(obj.Target);
                data.CourseId = obj.CourseId;
                Course courseObj = CourseBL.GetById(obj.CourseId, Convert.ToInt32(Usermodel.InstituteId));
                data.CourseName = courseObj.Name;
                data.BatchMonth = obj.BatchMonth;
                data.BatchYear = obj.BatchYear;
                return View("_ViewBatch", data);
            }
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Batch obj = BatchBL.GetById(Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                BatchModel data = new BatchModel();
                data.BatchId = Convert.ToInt32(id);
                data.AdmissionLast_Date = obj.AdmissionLastDate.ToShortDateString();
                data.DurationId = obj.Durationid;
                Duration duration = DurationBL.GetById(obj.Durationid);
                data.Duration = duration.DurationInMonths.ToString();
                //  data.DurationNames = PopulateDuration();
                User Usermodel = TempData["UserObject"] as User;
                TempData.Keep();
                data.InstituteId = Convert.ToInt32(Usermodel.InstituteId);
                data.Name = obj.Name;
                data.End_Date = obj.EndDate.ToShortDateString();
                data.InTakeCapacity = Convert.ToInt32(obj.IntakeCapacity);
                data.Start_Date = obj.StartDate.ToShortDateString();
                data.Target = Convert.ToInt32(obj.Target);

                //List<SelectListItem> CourseNames = new List<SelectListItem>();

                //List<AllCoursesByInstituteId_Result> courses = ReportBLcs.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));
                //courses.ForEach(x =>
                //{
                //    CourseNames.Add(new SelectListItem { Text = x.CourseName.ToString(), Value = x.Id.ToString() });
                //});
                //data.CourseNames = CourseNames;
                data.CourseId = obj.CourseId;
                Course courseObj = CourseBL.GetById(obj.CourseId, Convert.ToInt32(Usermodel.InstituteId));
                data.CourseName = courseObj.Name;
                //ViewBag.Months = new SelectList(Enumerable.Range(1, 12).Select(x =>
                // new SelectListItem()
                // {
                //     Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[x - 1],
                //     Value = x.ToString()
                // }), "Value", "Text");
                data.BatchMonth = obj.BatchMonth;

                //ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year, 1).Select(x =>
                //   new SelectListItem()
                //   {
                //       Text = x.ToString(),
                //       Value = x.ToString()
                //   }), "Value", "Text");
                data.BatchYear = obj.BatchYear;


                return View("_DeleteBatch", data);
            }
        }

        [HttpPost]
        public ActionResult Delete(BatchModel data)
        {
            if (data.BatchId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Batch obj = BatchBL.GetById(Convert.ToInt32(data.BatchId));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                BatchBL.Delete(obj);
                SweetAlert("Delete", "Batch deleted Successfully", NotificationType.success);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult CreateByBatch(int? courseid)
        {
            BatchModel dataModel = new BatchModel();
            dataModel.DurationNames = PopulateDuration();
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            int instituteid = Convert.ToInt32(Usermodel.InstituteId);
            int Courseid = Convert.ToInt32(courseid);
            dataModel.InstituteId = instituteid;
            dataModel.CourseId = Courseid;
            dataModel.CourseName = CourseBL.GetById(Courseid, instituteid).Name;
            ViewBag.Months = new SelectList(Enumerable.Range(1, 12).Select(x =>
             new SelectListItem()
             {
                 Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[x - 1],
                 Value = x.ToString()
             }), "Value", "Text");

            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 01);
            dataModel.Start_Date = startDate.ToString("MM/dd/yyyy");

            var lastDate = startDate.AddMonths(1).AddDays(-1);
            dataModel.AdmissionLast_Date = lastDate.ToShortDateString();

            ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year, 1).Select(x =>
               new SelectListItem()
               {
                   Text = x.ToString(),
                   Value = x.ToString()
               }), "Value", "Text");

            dataModel.BatchMonth = DateTime.Today.Month.ToString();
            dataModel.BatchYear = DateTime.Today.Year;
            dataModel.Name = dataModel.CourseName + "-" + getFullName(DateTime.Today.Month) + "-" + DateTime.Today.Year;
            return View("CreateBatchByCourseFee", dataModel);
        }
        static string getFullName(int month)
        {
            return CultureInfo.CurrentCulture.
                DateTimeFormat.GetMonthName
                (month);
        }
        [HttpPost]
        public ActionResult CreateByBatch(BatchModel data)
        {
            if (ModelState.IsValid)
            {
                Batch obj = new Batch();
                obj.AdmissionLastDate = Convert.ToDateTime(data.AdmissionLast_Date);
                obj.BatchMonth = data.BatchMonth;
                obj.BatchYear = data.BatchYear;
                obj.CourseId = data.CourseId;
                obj.Durationid = data.DurationId;
                obj.EndDate = Convert.ToDateTime(data.End_Date);
                obj.IntakeCapacity = data.InTakeCapacity;
                obj.IsClosed = false;
                obj.IsLocked = false;
                obj.Name = data.Name;
                obj.StartDate = Convert.ToDateTime(data.Start_Date);
                obj.Target = data.Target;
                obj.IsDeleted = false;
                if (BatchBL.Add(obj) == true)
                {
                    SweetAlert("Done", "Batch added Successfully", NotificationType.success);
                    return RedirectToAction("Index", "Batch");
                }
            }

            return View();
        }
    }
}