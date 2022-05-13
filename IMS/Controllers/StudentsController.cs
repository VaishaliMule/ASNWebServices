using IMS.Models;
using BussinessLayer;
using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static IMS.Models.Enum;

namespace IMS.Controllers
{
    [CheckSessionOut]
    public class StudentsController : BaseController
    {
        // GET: InstituteAdmin/Student
        public ActionResult Index()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<AllStudentByInstituteId_Result> listItems = StudentBL.GetAllStudentByInstituteId(Convert.ToInt32(Usermodel.InstituteId));
            return View(listItems);
        }

        private static List<SelectListItem> PopulateQualification()
        {
            List<SelectListItem> qualificationNames = new List<SelectListItem>();
            List<Qualification> qualifications = QualificationBL.GetAll();
            qualifications.ForEach(x =>
            {
                qualificationNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            return qualificationNames;
        }
        private static List<SelectListItem> PopulateLanguages()
        {
            List<SelectListItem> languageNames = new List<SelectListItem>();
            List<Language> language = LanguageBL.GetAll();
            language.ForEach(x =>
            {
                languageNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            return languageNames;
        }
        private static List<SelectListItem> PopulateStateNames()
        {
            List<SelectListItem> stateNames = new List<SelectListItem>();
            List<State> states = StateBL.GetByCountryId(1);
            states.ForEach(x =>
            {
                stateNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            return stateNames;
        }
        private static List<SelectListItem> PopulateDistrictNames()
        {
            List<SelectListItem> districtNames = new List<SelectListItem>();
            List<District> districts = DistrictBL.GetAll();
            districts.ForEach(x =>
            {
                districtNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            return districtNames;
        }
        private static List<SelectListItem> PopulateTalukaNames()
        {
            List<SelectListItem> talukaNames = new List<SelectListItem>();
            List<Taluka> talukas = TalukaBL.GetAll();
            talukas.ForEach(x =>
            {
                talukaNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            return talukaNames;
        }
        private static List<SelectListItem> PopulateAreaNames()
        {
            List<SelectListItem> areaNames = new List<SelectListItem>();
            List<Suburb> areas = SuburbBL.GetAll();
            areas.ForEach(x =>
            {
                areaNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            return areaNames;
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
        private static List<SelectListItem> PopulateIdentityProofs()
        {
            List<SelectListItem> IdentityProofs = new List<SelectListItem>();

            List<IdentityProof> prrofs = IdentityProofBL.GetAll();
            prrofs.ForEach(x =>
            {
                IdentityProofs.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            return IdentityProofs;
        }
        private static List<SelectListItem> Populateinstallments()
        {
            List<SelectListItem> courseFees = new List<SelectListItem>();

            List<CourseFee> installments = CourseFeeBL.GetAll();
            installments.ForEach(x =>
            {
                courseFees.Add(new SelectListItem { Text = x.NoOfInstallments.ToString(), Value = x.Id.ToString() });
            });
            return courseFees;
        }

        [HttpPost]
        public ActionResult GetState(string CountryId)
        {

            int countryId;
            List<SelectListItem> stateNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(CountryId))
            {
                countryId = Convert.ToInt32(CountryId);
                List<State> states = StateBL.GetByCountryId(countryId);
                states.ForEach(x =>
                {
                    stateNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                });
            }

            return Json(stateNames, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDistrict(string stateId)
        {

            int statId;
            List<SelectListItem> districtNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(stateId))
            {
                statId = Convert.ToInt32(stateId);
                List<District> districts = DistrictBL.GetByStateId(statId);
                districts.ForEach(x =>
                {
                    districtNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                });
            }
            return Json(districtNames, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTahsil(string districtId)
        {

            int DistrictId;
            List<SelectListItem> tahsilNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(districtId))
            {
                DistrictId = Convert.ToInt32(districtId);
                List<Taluka> tahsils = TalukaBL.GetTalukaByDistrictId(DistrictId);
                tahsils.ForEach(x =>
                {
                    tahsilNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                });
            }
            return Json(tahsilNames, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSubUrbanArea(string talukaId)
        {

            int TalukaId;
            List<SelectListItem> subUrbanAreas = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(talukaId))
            {
                TalukaId = Convert.ToInt32(talukaId);
                List<Suburb> areas = SuburbBL.GetSubUrbanByCity(TalukaId);
                areas.ForEach(x =>
                {
                    subUrbanAreas.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                });
            }
            return Json(subUrbanAreas, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetBatch(string CourseId)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            int courseId;
            List<SelectListItem> BatchNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(CourseId))
            {
                courseId = Convert.ToInt32(CourseId);
                List<BatchByCourseId_InstituteId_Result> batches = BatchBL.GetBatchesofcourseid_instituteid(courseId, Convert.ToInt32(Usermodel.InstituteId));
                batches.ForEach(x =>
                {
                    BatchNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                });
            }

            return Json(BatchNames, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddStudent(string Firstname, string Lastname)
        {
            StudentModel data = new StudentModel();
            data.QualificationNames = PopulateQualification();
            data.LanguageNames = PopulateLanguages();
            data.StateNames = PopulateStateNames();
            data.DistrictNames = PopulateDistrictNames();
            data.TahsilNames = PopulateTalukaNames();
            data.AreaNames = PopulateAreaNames();

            data.FirstName = Firstname;
            data.LastName = Lastname;
         
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            data.InstituteId =Convert.ToInt32(Usermodel.InstituteId);
            return View("RegisterStudent",data);
        }

        [HttpPost]
        public ActionResult AddStudent(StudentModel data)
        {
            Student obj = new Student();
            obj.AadharNo = data.Aadhar;
            obj.AddedDate = DateTime.Now;
            obj.BirthDate =Convert.ToDateTime(data.DOB);
            obj.Email = data.Email;
            obj.FirstName = data.FirstName;
            obj.Gender = data.Gender.ToString();
            obj.LastName = data.LastName;
            obj.MaritalStatus = data.MarritalStatus.ToString();
            obj.MiddleName = data.MiddleName;
            obj.Mobile = data.Mobile;
            obj.MotherTongueLanguageId = data.LangauageId;
            obj.ParentMobile = data.ParentMobile;
            obj.QualificationId = data.QualificationId;
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            obj.InstituteId =Convert.ToInt32(Usermodel.InstituteId);
            obj.Address = data.Address;
            obj.CountryId = 1;
            obj.DistrictId = data.DistrictId;
            if(data.PhysicallyChallenged=="Yes")
            {
                obj.PhysicallyChallenged =true ;
                obj.Disability = data.Disability;
            }
            else
            {
                obj.PhysicallyChallenged = false;
                obj.Disability = null;
            }
            obj.Pincode = data.PinCode;
            obj.StateId = data.StateId;
            obj.suburb = SuburbBL.GetById(data.AreaId).Name;
            obj.TalukaId = data.TahsilId;
            obj.Photo = null;
            obj.SignFileNAme = null;
            obj.Signature =null;
            obj.PhotoFileName = null;
            if (StudentBL.Add(obj) == true)
            {
                SweetAlert("Done", "Student added Successfully", NotificationType.success);
                return RedirectToAction("Index", "Students");
            }

            return View();
        }

        [HttpPost]
        public ActionResult ExistOrNot(string FirstName, string LastName,DateTime DOB,string instituteid)
        {   
           Student data = StudentBL.GetBy_firstname_lastname_dob(FirstName, LastName,Convert.ToDateTime(DOB),Convert.ToInt32(instituteid));
            return Json(data.Id, JsonRequestBehavior.AllowGet);
        }
         [HttpGet]
        public ActionResult EditStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
            int instituteId = Convert.ToInt32(Usermodel.InstituteId);
            TempData.Keep();
            //  Student obj = StudentBL.GetById(Convert.ToInt32(id), instituteId);
            StudentForEditById_Result obj = StudentBL.GetStudentForEditByStudentId(instituteId, Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                StudentModel data = new StudentModel();
                int studentid = Convert.ToInt32(id);
                data.QualificationNames = PopulateQualification();
                data.LanguageNames = PopulateLanguages();
                data.StateNames = PopulateStateNames();
              
                int stateId = obj.StateId;
                List<SelectListItem> districtNames = new List<SelectListItem>();
                if (!string.IsNullOrEmpty(stateId.ToString()))
                {
                    List<District> districts = DistrictBL.GetByStateId(stateId);
                    districts.ForEach(x =>
                    {
                        districtNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                    });
                }
                data.DistrictNames = districtNames;

                int DistrictId=obj.DistrictId;
                List<SelectListItem> tahsilNames = new List<SelectListItem>();
                if (!string.IsNullOrEmpty(DistrictId.ToString()))
                {
                    List<Taluka> tahsils = TalukaBL.GetTalukaByDistrictId(DistrictId);
                    tahsils.ForEach(x =>
                    {
                        tahsilNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                    });
                }
                data.TahsilNames = tahsilNames;

                int TalukaId=obj.TalukaId;
                List<SelectListItem> subUrbanAreas = new List<SelectListItem>();
                if (!string.IsNullOrEmpty(TalukaId.ToString()))
                {
                    List<Suburb> areas = SuburbBL.GetSubUrbanByCity(TalukaId);
                    areas.ForEach(x =>
                    {
                        subUrbanAreas.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                    });
                }
                data.AreaNames = subUrbanAreas;

                data.StudentId = studentid;
                data.InstituteId = instituteId;
                data.FirstName = obj.FirstName;
                data.MiddleName = obj.MiddleName;
                data.LastName = obj.LastName;
                data.Gender = obj.Gender;
                DateTime? myDate = obj.BirthDate;
                data.DOB = myDate.Value.ToString("MM/dd/yyyy");
                data.Email = obj.Email;
                data.Aadhar = obj.AadharNo;
                data.MarritalStatus = obj.MaritalStatus;
                data.QualificationId = obj.QualificationId;
                data.LangauageId = obj.MotherTongueLanguageId;
                data.StateId = obj.StateId;
                data.DistrictId = obj.DistrictId;
                data.TahsilId = obj.TalukaId;
                
                if (obj.suburb != null)
                {
                    data.AreaId = SuburbBL.GetByName_TalukaId(obj.suburb,obj.TalukaId).Id;
                }
                data.Mobile = obj.Mobile;
                data.ParentMobile = obj.ParentMobile;
                data.Address = obj.Address;
                data.PinCode = obj.Pincode;
                if(obj.PhysicallyChallenged==true)
                {
                    data.PhysicallyChallenged = "Yes";
                }
                else
                {
                    data.PhysicallyChallenged = "No";
                }
                return View("EditStudent", data);
            }

        }
        [HttpPost]
        public ActionResult EditStudent(StudentModel data)
        {
            Student obj = StudentBL.GetById(data.StudentId, data.InstituteId);
            obj.AadharNo = data.Aadhar;
            obj.AddedDate = DateTime.Now;
            obj.BirthDate = Convert.ToDateTime(data.DOB);
            obj.Email = data.Email;
            obj.FirstName = data.FirstName;
            obj.Gender = data.Gender.ToString();
            obj.LastName = data.LastName;
            obj.MaritalStatus = data.MarritalStatus.ToString();
            obj.MiddleName = data.MiddleName;
            obj.Mobile = data.Mobile;
            obj.MotherTongueLanguageId = data.LangauageId;
            obj.ParentMobile = data.ParentMobile;
            obj.QualificationId = data.QualificationId;
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            obj.InstituteId = Convert.ToInt32(Usermodel.InstituteId);
            obj.Address = data.Address;
            obj.CountryId = 1;
            obj.DistrictId = data.DistrictId;
            if (data.PhysicallyChallenged == "Yes")
            {
                obj.PhysicallyChallenged = true;
            }
            else
            {
                obj.PhysicallyChallenged = false;
            }
            obj.Pincode = data.PinCode;
            obj.StateId = data.StateId;
            obj.suburb = SuburbBL.GetById(data.AreaId).Name;
            obj.TalukaId = data.TahsilId;
            if (StudentBL.Edit(obj) == true)
            {
                SweetAlert("Done", "Student Updated Successfully", NotificationType.success);
                return RedirectToAction("Index", "Students");
            }

            return View();
        }

        public ActionResult BirthdayCount()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<TodaysBirthdaylist_Result> listItems = StudentBL.GetAllTodaysBirthday(Convert.ToInt32(Usermodel.InstituteId));
            return View("ViewTodaysBirthday", listItems);
        }

        public ActionResult UploadPhotoAndSignCount()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<StudentListForAdmissionDetailsByInstituteId_Result> allStudents = AdmissionBL.GetStudentAdmissionByInstituteId(Convert.ToInt32(Usermodel.InstituteId)).Where(a => a.Photo == null || a.Signature == null).ToList();
            return View("UploadPhotoAndSign", allStudents);
        }
        [HttpPost]
        public ActionResult AddPhoto(int? StudentId, HttpPostedFileBase PhotoImageFile)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            Student obj = StudentBL.GetById(Convert.ToInt32(StudentId), Convert.ToInt32(Usermodel.InstituteId));
            if (PhotoImageFile != null && PhotoImageFile.ContentLength > 0)
            {
                string FileName = Path.GetFileNameWithoutExtension(PhotoImageFile.FileName);
                string FileExtension = Path.GetExtension(PhotoImageFile.FileName);
                FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                string UploadPath = Path.Combine(Server.MapPath("~/UserImages"), FileName);
                PhotoImageFile.SaveAs(UploadPath);
                obj.Photo = UploadPath.ToString();
                obj.PhotoFileName = FileName;
                if (StudentBL.Edit(obj) == true)
                {
                    List<StudentListForAdmissionDetailsByInstituteId_Result> allStudents = AdmissionBL.GetStudentAdmissionByInstituteId(Convert.ToInt32(Usermodel.InstituteId)).Where(a => a.Photo == null || a.Signature == null).ToList();
                    SweetAlert("Done", "Photo uploaded Successfully", NotificationType.success);
                    return View("UploadPhotoAndSign", allStudents);

                }
            }
            else
            {
                List<StudentListForAdmissionDetailsByInstituteId_Result> allStudents = AdmissionBL.GetStudentAdmissionByInstituteId(Convert.ToInt32(Usermodel.InstituteId)).Where(a => a.Photo == null || a.Signature == null).ToList();
                SweetAlert("Error", "PLease select Photo to upload", NotificationType.error);
                return View("UploadPhotoAndSign", allStudents);
            }


            return View();
        }

        [HttpPost]
        public ActionResult AddSign(int? StudentId, HttpPostedFileBase SignImageFile)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            Student obj = StudentBL.GetById(Convert.ToInt32(StudentId), Convert.ToInt32(Usermodel.InstituteId));
            if (SignImageFile != null && SignImageFile.ContentLength > 0)
            {
                string FileName = Path.GetFileNameWithoutExtension(SignImageFile.FileName);
                string FileExtension = Path.GetExtension(SignImageFile.FileName);
                FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                string UploadPath = Path.Combine(Server.MapPath("~/UserImages"), FileName);
                SignImageFile.SaveAs(UploadPath);
                obj.Signature = UploadPath.ToString();
                obj.SignFileNAme = FileName;
                if (StudentBL.Edit(obj) == true)
                {
                    List<StudentListForAdmissionDetailsByInstituteId_Result> allStudents = AdmissionBL.GetStudentAdmissionByInstituteId(Convert.ToInt32(Usermodel.InstituteId)).Where(a => a.Photo == null || a.Signature == null).ToList();
                    SweetAlert("Done", "Sign uploaded Successfully", NotificationType.success);
                    return View("UploadPhotoAndSign", allStudents);
                }
            }
            else
            {
                List<StudentListForAdmissionDetailsByInstituteId_Result> allStudents = AdmissionBL.GetStudentAdmissionByInstituteId(Convert.ToInt32(Usermodel.InstituteId)).Where(a => a.Photo == null || a.Signature == null).ToList();
                SweetAlert("Error", "PLease select sign to upload", NotificationType.error);
                return View("UploadPhotoAndSign", allStudents);
            }


            return View();
        }



    }
}