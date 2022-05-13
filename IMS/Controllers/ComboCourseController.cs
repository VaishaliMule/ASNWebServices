using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnosoftModel;
using BussinessLayer;
using System.Net;
using IMS.Models;
using static IMS.Models.Enum;

namespace IMS.Controllers
{
    public class ComboCourseController : BaseController
    {
        // GET: InstituteAdmin/ComboCourse
        public ActionResult Index()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            //List<GetAllCoursesByInstituteId_Result> listItems = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));
            //List<GetAllCoursesByInstituteId_Result> NewlistItems = new List<GetAllCoursesByInstituteId_Result>();
            //foreach (var item in listItems)
            //{
            //    if (item.COURSEPATTERN == "Combo")
            //    {
            //        NewlistItems.Add(item);
            //    }
            //}
            //return View(NewlistItems);
            List<AllComboCourses_Result> listItems = ComboCourseSubjectBL.GetAllComboCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));
            List<ComboCourseModel> model = new List<ComboCourseModel>();
            foreach (AllComboCourses_Result combocourse in listItems)
            {
                model.Add(new ComboCourseModel
                {
                    ID = combocourse.ID,
                    COURSEID = Convert.ToInt32(combocourse.COURSEID),
                    INSTITUTEID = Convert.ToInt32(combocourse.INSTITUTEID),
                    CODE = combocourse.CODE,
                    COURSETYPE = combocourse.COURSETYPE,
                    COURSENAME = combocourse.COURSENAME,
                    DURATION = combocourse.DURATION,
                    COURSEDESCRIPTION = combocourse.COURSEDESCRIPTION,
                    COURSESTATUS = Convert.ToInt32(combocourse.COURSESTATUS),
                    COURSEPATTERN = combocourse.COURSEPATTERN,

                    SubCourses = ComboCourseSubjectBL.GetAllSubCourses(Convert.ToInt32(Usermodel.InstituteId), Convert.ToInt32(combocourse.COURSEID))
                });

            }


            return View(model);

        }

        [HttpGet]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            Course obj = CourseBL.GetById(Convert.ToInt32(id), Convert.ToInt32(Usermodel.InstituteId));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                CourseModel data = new CourseModel();
                data.CoursePattern = obj.IsCombo;
                data.CourseTypeId = obj.CourseTypeId;
                data.Name = obj.Name;
                data.CourseId = Convert.ToInt32(id);
                data.InstituteId = obj.InstituteId;

                List<AllCoursesByInstituteId_Result> listItems = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));

                List<SelectListItem> items = new List<SelectListItem>();


                foreach (var item in listItems)
                {
                    if (item.COURSEPATTERN == "Single")
                    {
                        ComboCourseSubject selectedorNot = ComboCourseSubjectBL.GetSelectedComboCourse(Convert.ToInt32(item.INSTITUTEID), data.CourseId, Convert.ToInt32(item.COURSEID));
                        if (selectedorNot != null)
                        {
                            items.Add(new SelectListItem
                            {
                                Text = item.COURSENAME.ToString(),
                                Value = item.COURSEID.ToString(),
                                Selected = true
                            });
                        }
                        else
                        {
                            items.Add(new SelectListItem
                            {
                                Text = item.COURSENAME.ToString(),
                                Value = item.COURSEID.ToString()
                            });
                        }

                    }
                }

                data.AllCoursesForCombo = items;
                return View("ComboCourseSubject", data);
            }
        }

        [HttpGet]
        public ActionResult Add(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            Course obj = CourseBL.GetById(Convert.ToInt32(id), Convert.ToInt32(Usermodel.InstituteId));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                CourseModel data = new CourseModel();
                data.CoursePattern = obj.IsCombo;
                data.CourseTypeId = obj.CourseTypeId;
                data.Name = obj.Name;
                data.CourseId = Convert.ToInt32(id);
                data.InstituteId = obj.InstituteId;
                List<AllCoursesByInstituteId_Result> listItems = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));

                List<SelectListItem> items = new List<SelectListItem>();


                foreach (var item in listItems)
                {
                    if (item.COURSEPATTERN == "Single")
                    {
                        ComboCourseSubject selectedorNot = ComboCourseSubjectBL.GetSelectedComboCourse(Convert.ToInt32(item.INSTITUTEID), data.CourseId, Convert.ToInt32(item.COURSEID));
                        if (selectedorNot != null)
                        {
                            items.Add(new SelectListItem
                            {
                                Text = item.COURSENAME.ToString(),
                                Value = item.COURSEID.ToString(),
                                Selected = true,
                                Disabled = true
                            });
                        }
                        else
                        {
                            items.Add(new SelectListItem
                            {
                                Text = item.COURSENAME.ToString(),
                                Value = item.COURSEID.ToString()
                            });
                        }

                    }
                }

                data.AllCoursesForCombo = items;
                return View("_AddComboCourse", data);
            }
        }

        [HttpPost]
        public ActionResult Add(CourseModel data, string[] combocourse)
        {
            // List<ComboCourseSubject> lstOfSubCourses = ComboCourseSubjectBL.GetListofComboCourse(data.InstituteId, data.CourseId);

            if (combocourse != null)
            {
                foreach (var item in combocourse)
                {
                    int courseId = CourseBL.GetByName(item, data.InstituteId).Id;
                    ComboCourseSubject selectedorNot = ComboCourseSubjectBL.GetSelectedComboCourse(Convert.ToInt32(data.InstituteId), data.CourseId, courseId);
                    if (selectedorNot == null)
                    {
                        ComboCourseSubject obj = new ComboCourseSubject();
                        obj.ComboCourseId = data.CourseId;
                        obj.InstituteId = data.InstituteId;
                        obj.IsDeleted = false;
                        obj.SubCourseId = courseId;
                        ComboCourseSubjectBL.Add(obj);
                    }
                    else
                    {
                        ComboCourseSubjectBL.Edit(selectedorNot);
                    }

                }

                SweetAlert("Done", "SubCourse added Successfully to Course " + data.Name, NotificationType.success);
                // return RedirectToAction("Index", "ComboCourse");
                return RedirectToAction("TestCourseFees", "Course", new { id = data.CourseId });
            }
            else
            {
                SweetAlert("Error", "Something went wronge!", NotificationType.error);
                return View();
            }
        }
        private static List<SelectListItem> PopulateCourseType()
        {
            List<SelectListItem> courseTypeNames = new List<SelectListItem>();
            List<CourseType> courseTypes = CourseTypeBL.GetAll();
            courseTypes.ForEach(x =>
            {
                courseTypeNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            return courseTypeNames;
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
        public ActionResult CreateCombo()
        {
            CourseModel dataModel = new CourseModel();
            dataModel.CourseTypeNames = PopulateCourseType();
            dataModel.DurationNames = PopulateDuration();
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            dataModel.InstituteId = Convert.ToInt32(Usermodel.InstituteId);
            return View("_CreateCourse", dataModel);
        }

        [HttpPost]
        public ActionResult CreateCombo(CourseModel data)
        {

            Course obj = new Course();
            obj.Code = data.Code;
            obj.CourseTypeId = data.CourseTypeId;
            obj.Description = data.Description;
            obj.DurationId = data.DurationId;
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            obj.InstituteId = Convert.ToInt32(Usermodel.InstituteId);
            obj.IsActive = true;
            obj.IsCombo = "Combo";
            obj.CourseSelectionLimit = data.CourseSelectionLimit;
            obj.IsDeleted = false;
            obj.Name = data.Name;

            if (CourseBL.Add(obj) == true)
            {
                SweetAlert("Done", "Course added Successfully", NotificationType.success);
                return RedirectToAction("Add", "ComboCourse", new { id = obj.Id });
            }


            return View();
        }
    }
}