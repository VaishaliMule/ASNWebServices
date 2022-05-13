
using BussinessLayer;
using IMS.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using TechnosoftModel;
using static IMS.Models.Enum;

namespace IMS.Controllers
{
    [CheckSessionOut]
    public class CourseController : BaseController
    {
        // GET: InstituteAdmin/Course
        public ActionResult Index()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<AllCoursesByInstituteId_Result> listItems = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));
            List<CourseType> courseTypes = CourseTypeBL.GetAll();

            ViewBag.CourseType = new SelectList(courseTypes, "Id", "Name"); ;
            return View(listItems);
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
        public ActionResult Create()
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
        public ActionResult Create(CourseModel data)
        {
            ////if (ModelState.IsValid)
            ////{
                Course obj = new Course();
                obj.Code = data.Code;
                obj.CourseTypeId = data.CourseTypeId;
                obj.Description = data.Description;
                obj.DurationId = data.DurationId;
                User Usermodel = TempData["UserObject"] as User;
                TempData.Keep();
                obj.InstituteId = Convert.ToInt32(Usermodel.InstituteId);
                obj.IsActive = true;
                obj.IsCombo = "Single";
              //  obj.CourseSelectionLimit = data.CourseSelectionLimit;

                obj.IsDeleted = false;
                obj.Name = data.Name;

                if (CourseBL.Add(obj) == true)
                {
                    SweetAlert("Done", "Course added Successfully", NotificationType.success);
                        return RedirectToAction("TestCourseFees", "Course", new { id = obj.Id });
                }
            //}
              
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
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
                data.CourseId = Convert.ToInt32(id);
                data.Code = obj.Code;
               // data.CoursePattern = obj.IsCombo;
                data.CourseTypeId = obj.CourseTypeId;
                data.CourseTypeNames = PopulateCourseType();
                data.Description = obj.Description;
                data.DurationId = obj.DurationId;
                data.DurationNames = PopulateDuration();
                data.InstituteId = obj.InstituteId;
                data.Name = obj.Name;
                data.CourseSelectionLimit =Convert.ToInt32(obj.CourseSelectionLimit);
                return View("_EditCourse", data);
            }

        }

        [HttpPost]
        public ActionResult Edit(CourseModel data)
        {
            if (data.CourseId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            Course obj = CourseBL.GetById(Convert.ToInt32(data.CourseId), Convert.ToInt32(Usermodel.InstituteId));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                //if (ModelState.IsValid)
                //{
                    obj.Code = data.Code;
                    obj.CourseTypeId = data.CourseTypeId;
                    obj.Description = data.Description;
                    obj.DurationId = data.DurationId;
                  
                    obj.InstituteId = Convert.ToInt32(Usermodel.InstituteId);
                    obj.IsActive = true;
                    obj.IsCombo = data.CoursePattern;
                   
                    obj.IsDeleted = false;
                    obj.Name = data.Name;
                    obj.CourseSelectionLimit = data.CourseSelectionLimit;

                    if (CourseBL.Edit(obj) == true)
                    {
                        SweetAlert("Done", "Course updated Successfully", NotificationType.success);
                        return RedirectToAction("Index", "Course");
                    }
                //}
                //else
                //{
                //    ModelState.AddModelError("", "description of model error found");
                //}
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
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            int courseid = Convert.ToInt32(id);
            Course obj = CourseBL.GetById(courseid, Convert.ToInt32(Usermodel.InstituteId));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                CourseModel data = new CourseModel();
                data.CourseId = courseid;
                data.Code = obj.Code;
                data.CoursePattern = obj.IsCombo;
                data.CourseTypeId = obj.CourseTypeId;
                CourseType type = CourseTypeBL.GetById(obj.CourseTypeId);
                data.CourseType =type.Name;
                data.Description = obj.Description;
                data.DurationId = obj.DurationId;
                Duration duration = DurationBL.GetById(obj.DurationId);
                data.Duration = duration.DurationInMonths.ToString();
                data.InstituteId = obj.InstituteId;
                data.Name = obj.Name;
                data.CourseSelectionLimit = Convert.ToInt32(obj.CourseSelectionLimit);
                data.SubCourses = ComboCourseSubjectBL.GetAllSubCourses(Convert.ToInt32(Usermodel.InstituteId), courseid);
                return View("_ViewCourse", data);
            }
        }

        [HttpGet]
        public ActionResult Delete(int? id)
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
                data.CourseId = Convert.ToInt32(id);
                data.Code = obj.Code;
                data.CoursePattern = obj.IsCombo;
                data.CourseTypeId = obj.CourseTypeId;
                CourseType type = CourseTypeBL.GetById(obj.CourseTypeId);
                data.CourseType = type.Name;
                data.Description = obj.Description;
                data.DurationId = obj.DurationId;
                Duration duration = DurationBL.GetById(obj.DurationId);
                data.Duration = duration.DurationInMonths.ToString();
                data.InstituteId = obj.InstituteId;
                data.Name = obj.Name;
                data.CourseSelectionLimit = Convert.ToInt32(obj.CourseSelectionLimit);
                return View("_DeleteCourse", data);
            }
        }
        [HttpPost]
        public ActionResult Delete(CourseModel data)
        {
            if (data.CourseId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            Course obj = CourseBL.GetById(Convert.ToInt32(data.CourseId), Convert.ToInt32(Usermodel.InstituteId));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                CourseBL.Delete(obj);
                SweetAlert("Delete", "Course deleted Successfully", NotificationType.success);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ExistOrNot(string coursename, string instituteid)
        {
            Course course = CourseBL.GetByName(coursename,Convert.ToInt32(instituteid));
            return Json(course.Id, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CourseFees(int? id)
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
                CourseFeesModel data = new CourseFeesModel();
                int courseId = Convert.ToInt32(id);
                data.CourseId = courseId;
                data.CourseName = obj.Name;
                
                int instituteId = Convert.ToInt32(Usermodel.InstituteId);
                TempData.Keep();
             
                data.CourseFees = CourseFeeBL.GetAllCourseFessByInstitutes(instituteId, courseId); 
                return View("_CourseFees",data);
            }
            
        }

        [HttpPost]
        public ActionResult CourseFees(CourseFeesModel data,string[] DynamicTextBox)
        {
            if (ModelState.IsValid)
            {
                CourseFee cfeesObj = new CourseFee();
                cfeesObj.CourseId = data.CourseId;
                int CalFeeAmount = 0;
                foreach (string textboxvalue in DynamicTextBox)
                {
                    CalFeeAmount +=Convert.ToInt32(textboxvalue);
                }
                 cfeesObj.FeeAmount = CalFeeAmount;
                cfeesObj.NoOfInstallments = data.NoOfInstallment;

                if (CourseFeeBL.Add(cfeesObj) == true)
                {
                    int installment = 0;
                   
                    foreach (string textboxvalue in DynamicTextBox)
                    {
                        CourseFeeStructure feeStructure = new CourseFeeStructure();
                        feeStructure.CourseFeeId = cfeesObj.Id;
                        feeStructure.CourseId = data.CourseId;
                        installment = installment + 1;
                        feeStructure.Installment = installment;
                        feeStructure.InstallmentAmount = Convert.ToDecimal(textboxvalue);
                       
                        CourseFeeStructureBL.Add(feeStructure);
                        
                    }
                    SweetAlert("Done", "Course Fees added Successfully", NotificationType.success);
                    return RedirectToAction("Index", "Course");
                }
            }
            else
            {
                ModelState.AddModelError("", "description of model error found");
            }

            return View();
        }

        [HttpPost]
        public ActionResult CourseFeesExistOrNot(string courseId, string installment)
        {
            CourseFee fees = CourseFeeBL.GetByCourse_Installment(Convert.ToInt32(courseId), Convert.ToInt32(installment));
            return Json(fees.Id, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditCourseFees(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseFee obj = CourseFeeBL.GetById(Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                CourseFeesModel data = new CourseFeesModel();

                User Usermodel = TempData["UserObject"] as User;
                int instituteId = Convert.ToInt32(Usermodel.InstituteId);
                TempData.Keep();


                data.CourseId =obj.CourseId;
                data.CourseName = CourseBL.GetById(obj.CourseId,Convert.ToInt32(Usermodel.InstituteId)).Name;
                data.FeesAmount = obj.FeeAmount;
                data.NoOfInstallment = obj.NoOfInstallments;
                data.CourseFeesId = Convert.ToInt32(id);
                

                data.CourseFees = CourseFeeBL.GetAllCourseFessByInstitutes(instituteId, obj.CourseId);
                string[] DynamicTextBox=new string[obj.NoOfInstallments];
                for (int i=0; i<obj.NoOfInstallments; i++)
                {
                    int installment = i + 1;
                    CourseFeeStructure feeStructure = CourseFeeStructureBL.GetByCourseFeesId_installment(Convert.ToInt32(id), installment);
                    DynamicTextBox[i] = feeStructure.InstallmentAmount.ToString();
                }
                data.DyanamicTextbox = DynamicTextBox;

                return View("_EditCourseFees", data);
            }

        }

        [HttpPost]
        public ActionResult EditCourseFees(CourseFeesModel data, string[] DynamicTextBox)
        {
            if (data.CourseFeesId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseFee obj = CourseFeeBL.GetById(Convert.ToInt32(data.CourseFeesId));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    obj.CourseId = data.CourseId;
                    // obj.FeeAmount = data.FeesAmount;
                    decimal CalFeeAmount = 0;
                    foreach (string textboxvalue in DynamicTextBox)
                    {
                        CalFeeAmount += Convert.ToDecimal(textboxvalue);
                    }
                    obj.FeeAmount =Convert.ToDecimal(CalFeeAmount);


                   // obj.NoOfInstallments = data.NoOfInstallment;

                    

                    if (CourseFeeBL.Edit(obj) == true)
                    {
                        int installment = 0;
                        foreach (string textboxvalue in DynamicTextBox)
                        {
                            installment = installment + 1;
                            CourseFeeStructure feeStructure = CourseFeeStructureBL.GetByCourseFeesId_installment(obj.Id, installment);
                            if (feeStructure != null)
                            {
                                feeStructure.CourseFeeId = obj.Id;
                                feeStructure.CourseId = data.CourseId;
                                feeStructure.Installment = installment;
                                feeStructure.InstallmentAmount = Convert.ToDecimal(textboxvalue);
                                CourseFeeStructureBL.Edit(feeStructure);
                            }
                            else
                            {
                                CourseFeeStructure newfeeStructure = new CourseFeeStructure();
                                newfeeStructure.CourseFeeId = obj.Id;
                                newfeeStructure.CourseId = data.CourseId;
                                newfeeStructure.Installment = installment;
                                newfeeStructure.InstallmentAmount = Convert.ToDecimal(textboxvalue);
                                CourseFeeStructureBL.Add(newfeeStructure);
                            }

                        }
                        SweetAlert("Done", "Course Fee updated for course "+ data.CourseName +" Successfully", NotificationType.success);
                        // return RedirectToAction("Index", "Course");
                        return RedirectToAction("TestCourseFees", "Course", new { id = data.CourseId });
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
        public ActionResult TestCourseFees(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
            int instituteId = Convert.ToInt32(Usermodel.InstituteId);
            TempData.Keep();
            Course obj = CourseBL.GetById(Convert.ToInt32(id),instituteId);
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                CourseFeesModel data = new CourseFeesModel();
                int courseId = Convert.ToInt32(id);
                data.CourseId = courseId;
                data.CourseName = obj.Name;
                data.ComboOrSingle = obj.IsCombo;
                List<AllCourseFeesByInstituteId_Result> lst= CourseFeeBL.GetAllCourseFessByInstitutes(instituteId, courseId);
                //  data.CourseFees = CourseFeeBL.GetAllCourseFessByInstitutes(instituteId, courseId);
                data.CourseFees = lst;
                if (lst.Count==0)
                {
                    ViewBag.BatchFlag = false;
                }
                else
                {
                    ViewBag.BatchFlag = true;
                    
                }
                return View("_TestCourseFees", data);
            }

        }

        [HttpPost]
        public ActionResult TestCourseFees(CourseFeesModel data, string[] DynamicTextBox)
        {
            if (ModelState.IsValid)
            {
                CourseFee cfeesObj = new CourseFee();
                cfeesObj.CourseId = data.CourseId;
                int CalFeeAmount = 0;
                foreach (string textboxvalue in DynamicTextBox)
                {
                    CalFeeAmount += Convert.ToInt32(textboxvalue);
                }
                cfeesObj.FeeAmount = CalFeeAmount;
                cfeesObj.NoOfInstallments = data.NoOfInstallment;

                if (CourseFeeBL.Add(cfeesObj) == true)
                {
                    int installment = 0;
                    foreach (string textboxvalue in DynamicTextBox)
                    {
                        CourseFeeStructure feeStructure = new CourseFeeStructure();
                        feeStructure.CourseFeeId = cfeesObj.Id;
                        feeStructure.CourseId = data.CourseId;
                        installment = installment + 1;
                        feeStructure.Installment = installment;
                        feeStructure.InstallmentAmount = Convert.ToDecimal(textboxvalue);
                        CourseFeeStructureBL.Add(feeStructure);

                    }
                    SweetAlert("Done", "Course Fees added Successfully", NotificationType.success);
                    return RedirectToAction("TestCourseFees", "Course", new { id = data.CourseId });
                  

                }
            }
            else
            {
                ModelState.AddModelError("", "description of model error found");
            }

            return View();
        }


        public ActionResult LoadData(string courseType)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<AllCoursesByInstituteId_Result> listItems = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));
            List<AllCoursesByInstituteId_Result> filterLst =new List<AllCoursesByInstituteId_Result>();



            foreach (var item in listItems)
            {
                if (item.COURSETYPE == courseType)
                {
                    filterLst.Add(item);
                }
            }





            //WHERE Country = @Country OR @Country IS NULL
           
           
            return Json(new { data = filterLst }, JsonRequestBehavior.AllowGet);
        }


    }
}