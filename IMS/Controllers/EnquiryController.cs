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
    public class EnquiryController : BaseController
    {
        // GET: InstituteAdmin/Enquiry
        public ActionResult Index()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<StudentListForEnquiryDetailsByInstituteId_Result> listItems = EnquiryBL.GetStudentEnquiryByInstituteId(Convert.ToInt32(Usermodel.InstituteId));
            return View(listItems);
        }
        [HttpGet]
        public ActionResult Search()
        {
            List<Student> listItems = new List<Student>();
            ViewBag.ShowDiv = false;
            return View("SearchStudent", listItems);
           
        }
        [HttpPost]
        public ActionResult Search(string firstname,string lastname)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            ViewBag.firstname = firstname;
            ViewBag.lastname = lastname;
            List<Student> listItems = StudentBL.SearchBy_firstname_lastname(firstname,lastname,Convert.ToInt32(Usermodel.InstituteId));
            if (listItems.Count != 0)
            {
                ViewBag.ShowDiv = true;
                return View("SearchStudent", listItems);
            }
            else
            {
                return RedirectToAction("AddStudent", "Students", new { Firstname = firstname, Lastname = lastname });
            }
            
        }
        [HttpPost]
        public ActionResult SearchForRecord(string firstname, string lastname)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<Student> listItems = StudentBL.SearchBy_firstname_lastname(firstname, lastname, Convert.ToInt32(Usermodel.InstituteId));
            return View("SearchStudent", listItems);
            // return Json(listItems, JsonRequestBehavior.AllowGet);
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
        private static List<SelectListItem> PopulateReference()
        {
            List<SelectListItem> sourceofinfo = new List<SelectListItem>();
            List<InformationSource> sources = InformationSourceBL.GetAll();
            sources.ForEach(x =>
            {
                sourceofinfo.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            return sourceofinfo;
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
        [HttpGet]
        public ActionResult AddEnquiry(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            // IMSDataModel.Student obj = StudentBL.GetById(Convert.ToInt32(id), Convert.ToInt32(Usermodel.InstituteId));
            StudentForEditById_Result obj = StudentBL.GetStudentForEditByStudentId(Convert.ToInt32(Usermodel.InstituteId), Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                StudentModel data = new StudentModel();
               
                data.SourceOfInformation = PopulateReference();
               
                data.InstituteId = Convert.ToInt32(Usermodel.InstituteId);
                data.StudentId = Convert.ToInt32(id);
                List<SelectListItem> CourseNames = new List<SelectListItem>();
                List<AllCoursesByInstituteId_Result> courses = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));
                courses.ForEach(x =>
                {
                    CourseNames.Add(new SelectListItem { Text = x.COURSENAME.ToString(), Value = x.COURSEID.ToString() });
                });
                data.CourseNames = CourseNames;
                data.Aadhar = obj.AadharNo;
                data.Address = obj.Address;
               
                data.Area = obj.suburb;
                data.District = obj.DistrictName;
               
                DateTime? myDate = obj.BirthDate;
                data.DOB = myDate.Value.ToString("MM/dd/yyyy");
                data.Email = obj.Email;
                data.FirstName = obj.FirstName;
                data.Gender = obj.Gender;
              
                data.Langauage = obj.LanguageName;
                data.LastName = obj.LastName;
                data.MarritalStatus = obj.MaritalStatus;
                data.MiddleName = obj.MiddleName;
                data.Mobile = obj.Mobile;
                data.ParentMobile = obj.ParentMobile;
                data.PhysicallyChallenged = obj.PhysicallyChallenged.ToString();
                data.PinCode = obj.Pincode;
               
                data.Qualification = obj.QualificationName;
             
                data.State = obj.StateName;
          
                data.Tahsil = obj.TalukaName;
                return View("AddEnquiry", data);
            }
       
        }
        [HttpPost]
        public ActionResult AddEnquiry(StudentModel data)
        {
            Enquiry obj = new Enquiry();
           
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            obj.InstituteId = Convert.ToInt32(Usermodel.InstituteId);
            obj.ConvertedToAdmission =false;
            obj.CourseId = data.CourseId;
            obj.DateAndTime = DateTime.Now;
           // Staff staffitem= StaffBL.GetByEmail(Usermodel.Username, Convert.ToInt32(Usermodel.InstituteId));
            obj.FacultyId = Usermodel.Id;
            obj.FollowUpDays = data.FollowupDays;
            obj.FollowUpDescription = data.FollowupDescription;
            obj.FollowUpPreferedTime = data.FollowupPreferedTime;
            obj.IsClosed = false;
            obj.Reference = InformationSourceBL.GetById(Convert.ToInt32(data.InformationId)).Name;
            obj.StudentId =data.StudentId;
       
           if (EnquiryBL.Add(obj) == true)
            {
                FollowupHistory followupHistory = new FollowupHistory();
                followupHistory.Comments = "Followup to given date";
                followupHistory.EnquiryId = obj.Id;
                followupHistory.Intrested = true;
                followupHistory.NextFollowupDate = DateTime.Today.AddDays(data.FollowupDays);
                followupHistory.HistoryDate = DateTime.Now;
                followupHistory.FacultyId = Usermodel.Id;
                followupHistory.IsFollowup = false;
                FollowupHistoryBL.Add(followupHistory);
                Student stuobj = StudentBL.GetById(data.StudentId, Usermodel.InstituteId);
                Institute instituteObj = InstituteBL.GetById(Usermodel.InstituteId);
                string msgtext = "Dear " +stuobj.FirstName+" "+stuobj.LastName+Environment.NewLine+ "Thank you for Enquiry regarding to Course " + CourseBL.GetById(data.CourseId, Usermodel.InstituteId).Name+Environment.NewLine+ "Please feel free to contact for any query back with us." + Environment.NewLine + "Regards," + Environment.NewLine + instituteObj.InstituteName;
                //SMSBL.SendSMSEnquiryConfirm(stuobj.Mobile, msgtext);

                SweetAlert("Done", "Enquiry added Successfully", NotificationType.success);
                return RedirectToAction("Index", "Enquiry");
            }
           
            return View();
        }
        [HttpPost]
        public ActionResult NewEnquiry(StudentModel data)
        {
            Student obj = new Student();
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
            if (StudentBL.Add(obj) == true)
            {
                SweetAlert("Done", "Enquiry added Successfully", NotificationType.success);
                return RedirectToAction("Index", "Enquiry");
            }

            return View();
        }
        [HttpPost]
        public ActionResult ExistOrNot(string CourseId, string StudentId)
        {
            Enquiry data = EnquiryBL.GetEnquiriesByCourseId_StudentId(Convert.ToInt32(CourseId), Convert.ToInt32(StudentId));
            return Json(data.Id, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
            int instituteId = Convert.ToInt32(Usermodel.InstituteId);
            TempData.Keep();
            //  IMSDataModel.Student obj = StudentBL.GetById(Convert.ToInt32(id),instituteId);
            StudentForEditById_Result obj = StudentBL.GetStudentForEditByStudentId(Convert.ToInt32(Usermodel.InstituteId), Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                StudentModel data = new StudentModel();
                int studentid = Convert.ToInt32(id);
                data.StudentId = studentid;
                data.InstituteId = instituteId;
                data.FirstName = obj.FirstName;
                data.MiddleName = obj.MiddleName;
                data.LastName = obj.LastName;
                data.Gender = obj.Gender;
                //data.DOB = obj.BirthDate.ToString();
                DateTime? myDate = obj.BirthDate;
                data.DOB = myDate.Value.ToString("MM/dd/yyyy");
                data.Email =obj.Email;
                data.MarritalStatus = obj.MaritalStatus;
                data.Qualification = obj.QualificationName;//QualificationBL.GetById(Convert.ToInt32(obj.QualificationId)).Name;
                data.Langauage = obj.LanguageName;//LanguageBL.GetById(Convert.ToInt32(obj.MotherTongueLanguageId)).Name;
                data.Mobile =obj.Mobile;
                data.ParentMobile =obj.ParentMobile;
                data.Address =obj.Address;
                data.Area =obj.suburb;
                data.PinCode =obj.Pincode;
                data.StudentsEnquires = EnquiryBL.GetAllEnquiryOfStudentByInstituteId(instituteId, studentid);
                return View("ViewEnquiry", data);
            }

        }
        [HttpPost]
        public ActionResult GetBatch(string CourseId)
        {
            int courseId;
            List<SelectListItem> BatchNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(CourseId))
            {
                User Usermodel = TempData["UserObject"] as User;
                TempData.Keep();
                courseId = Convert.ToInt32(CourseId);
                List<BatchByCourseId_InstituteId_Result> batches = BatchBL.GetBatchesofcourseid_instituteid(courseId, Convert.ToInt32(Usermodel.InstituteId));
                batches.ForEach(x =>
                {
                    BatchNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                });
            }
            return Json(BatchNames, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetInstallments(string CourseId)
        {
            int courseId;
            List<SelectListItem> installments = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(CourseId))
            {
                courseId = Convert.ToInt32(CourseId);
                List<CourseFee> feesinstallment = CourseFeeBL.GetInstallmentByCourseId(courseId);
                feesinstallment.ForEach(x =>
                {
                    installments.Add(new SelectListItem { Text = x.NoOfInstallments.ToString(), Value = x.Id.ToString() });
                });
            }
            return Json(installments, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AdmissionThroughEnquiry(int? id)
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
                StudentModel data = new StudentModel();
                int enquiryid = Convert.ToInt32(id);
                data.EnquiryId = enquiryid;

                //IMSDataModel.Student studObj = StudentBL.GetById(obj.StudentId, instituteId);
                StudentForEditById_Result studObj = StudentBL.GetStudentForEditByStudentId(instituteId, obj.StudentId);
                data.StudentId = obj.StudentId;
                data.InstituteId = instituteId;
                data.FirstName = studObj.FirstName;
                data.MiddleName = studObj.MiddleName;
                data.LastName = studObj.LastName;
                data.Gender = studObj.Gender;
                // data.DOB = studObj.BirthDate.ToString();
                DateTime? myDate = studObj.BirthDate;
                data.DOB = myDate.Value.ToString("MM/dd/yyyy");
                data.Email = studObj.Email;
                data.MarritalStatus = studObj.MaritalStatus;
                data.Qualification = studObj.QualificationName;
                data.Langauage = studObj.LanguageName;
                data.Mobile = studObj.Mobile;
                data.ParentMobile = studObj.ParentMobile;
                data.Address = studObj.Address;
                data.State = studObj.StateName;
                data.District = studObj.DistrictName;
                data.Tahsil = studObj.TalukaName;
                data.Area =studObj.suburb;
                data.PinCode = studObj.Pincode;
                data.Aadhar = studObj.AadharNo;
                List<SelectListItem> CourseNames = new List<SelectListItem>();
                List<AllCoursesByInstituteId_Result> courses = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));
                courses.ForEach(x =>
                {
                    CourseNames.Add(new SelectListItem { Text = x.COURSENAME.ToString(), Value = x.COURSEID.ToString() });
                });
                data.CourseNames = CourseNames;
                data.CourseId = obj.CourseId;
                List<SelectListItem> BatchNames = new List<SelectListItem>();
                if (!string.IsNullOrEmpty(obj.CourseId.ToString()))
                {
                   int courseId = Convert.ToInt32(obj.CourseId);
                    List<BatchByCourseId_InstituteId_Result> batches = BatchBL.GetBatchesofcourseid_instituteid(courseId, Convert.ToInt32(Usermodel.InstituteId));
                    batches.ForEach(x =>
                    {
                        BatchNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                    });
                }
                data.BatchNames = BatchNames;
                List<SelectListItem> courseFees = new List<SelectListItem>();
                if (!string.IsNullOrEmpty(obj.CourseId.ToString()))
                {
                    int courseId = Convert.ToInt32(obj.CourseId);
                    List<CourseFee> fees = CourseFeeBL.GetInstallmentByCourseId(courseId);
                    fees.ForEach(x =>
                    {
                        courseFees.Add(new SelectListItem { Text = x.NoOfInstallments.ToString(), Value = x.Id.ToString() });
                    });
                }
                data.CourseFees = courseFees;
               
                data.IdentityProofs = PopulateIdentityProofs();
                //  data.NameOnCertificate = studObj.LastName + " " + studObj.FirstName + " " + studObj.MiddleName;

                Course courseObj = CourseBL.GetById(obj.CourseId, instituteId);
                if(courseObj.IsCombo=="Combo")
                {
                    ViewBag.ShowDiv = true;
                    List<AllActiveBatchesOfSubCourses_Result> lstdata = ComboCourseSubjectBL.GetAllActiveBatchesOfComboCourses(instituteId, obj.CourseId);
                    data.subcoursesWithBAtches =lstdata;
                    ViewBag.SpanText = courseObj.CourseSelectionLimit;
                }
                else
                {
                    ViewBag.ShowDiv = false;
                }


                if (studObj.MiddleName != null)
                {
                    data.NameOnCertificate = studObj.LastName + " " + studObj.FirstName + " " + studObj.MiddleName;
                    data.Fullname = studObj.LastName + " " + studObj.FirstName + " " + studObj.MiddleName;
                }
                else
                {
                    data.NameOnCertificate = studObj.LastName + " " + studObj.FirstName;
                    data.Fullname = studObj.LastName + " " + studObj.FirstName;
                }
                return View("Admission", data);
            }

        }
        [HttpPost]
        public ActionResult AdmissionThroughEnquiry(StudentModel data, string[] combocourse)
        {
            Admission obj = new Admission();
            obj.AdmissionDate = DateTime.Now;
            obj.BatchId = data.BatchId;
            obj.CourseId = data.CourseId;

            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
           Student stuobj = StudentBL.GetById(data.StudentId, Usermodel.InstituteId);
            //   Staff staffitem = StaffBL.GetByEmail(Usermodel.Username,Convert.ToInt32(Usermodel.InstituteId));
            obj.FacultyID = Usermodel.Id;

            //obj.IdentityProof = IdentityProofBL.GetById(data.IdentityProofId).Name;
            //obj.InstallmentMode =CourseFeeBL.GetById(data.CourseFeesId).NoOfInstallments;

            if (data.IdentityProofId != 0)
            {
                obj.IdentityProof = IdentityProofBL.GetById(data.IdentityProofId).Name;
            }
            if (data.CourseFeesId != 0)
                obj.InstallmentMode = CourseFeeBL.GetById(data.CourseFeesId).NoOfInstallments;


            obj.InstituteId = data.InstituteId;
            obj.IsComplete = false;
            obj.IsDeleted = false;
            obj.LanguageOfStudy = stuobj.MotherTongueLanguageId;
            obj.NameOnCertificate = data.Fullname;
            obj.StudentId = data.StudentId;
            obj.IsComboCourseStudent = false;
            CourseFee fees = CourseFeeBL.GetByCourse_Installment(obj.CourseId, Convert.ToInt32(obj.InstallmentMode));
            obj.Discount = Convert.ToDecimal(0);
           
            obj.TotalFees =fees.FeeAmount;
            obj.TotalPaid = Convert.ToDecimal(0);
            obj.TotalBalance =fees.FeeAmount-obj.TotalPaid;
            obj.IsDeleted = false;
            obj.IsComboCourseStudent = false;
            if (AdmissionBL.Add(obj) == true)
            {
               Enquiry enqItem = EnquiryBL.GetById(Convert.ToInt32(data.EnquiryId), Convert.ToInt32(data.InstituteId));
                enqItem.CourseId = data.CourseId;
                enqItem.ConvertedToAdmission = true;
                EnquiryBL.Edit(enqItem);
                if (combocourse != null)
                {
                    foreach (var item in combocourse)
                    {
                        int batchid = Convert.ToInt32(item);
                        Batch batchitem = BatchBL.GetById(batchid);
                        Admission SubAdmobj = new Admission();
                        SubAdmobj.AdmissionDate = obj.AdmissionDate;
                        SubAdmobj.BatchId = batchid;
                        SubAdmobj.CourseId = batchitem.CourseId;
                        SubAdmobj.FacultyID = Usermodel.Id;
                        SubAdmobj.IdentityProof = obj.IdentityProof;
                        SubAdmobj.InstallmentMode = obj.InstallmentMode;
                        SubAdmobj.InstituteId = data.InstituteId;
                        SubAdmobj.IsComplete = false;
                        SubAdmobj.LanguageOfStudy = obj.LanguageOfStudy;
                        SubAdmobj.NameOnCertificate = data.Fullname;
                        SubAdmobj.StudentId = data.StudentId;
                        SubAdmobj.Discount = 0;
                        SubAdmobj.TotalFees = 0;
                        SubAdmobj.TotalPaid = Convert.ToDecimal(0);
                        SubAdmobj.TotalBalance = 0;
                        SubAdmobj.IsDeleted = false;
                        SubAdmobj.ComboCourseId = data.CourseId;
                        SubAdmobj.IsComboCourseStudent = true;
                        AdmissionBL.Add(SubAdmobj);
                    }
                }

                Institute instituteObj = InstituteBL.GetById(Usermodel.InstituteId);
                string msgtext = "Dear " + stuobj.FirstName + " " + stuobj.LastName + Environment.NewLine + "Welcome to " + instituteObj.InstituteName + ", thanks for choosing us for Course " + CourseBL.GetById(data.CourseId, Usermodel.InstituteId).Name + Environment.NewLine + "Please feel free to contact for any query back with us." + Environment.NewLine + "Regards," + Environment.NewLine + instituteObj.InstituteName;
              //  SMSBL.SendSMS(stuobj.Mobile, msgtext);

                SweetAlert("Done", "Admission added Successfully", NotificationType.success);
                // return RedirectToAction("Index", "Enquiry");

                return RedirectToAction("FeesReceipt", "Admission", new { id = obj.Id });
            }
            return View();
        }
        [HttpPost]
        public ActionResult AdmissionExistOrNot(string CourseId, string StudentId,string BatchId, string InstituteId)
        {
            Admission data = AdmissionBL.GetAdmissionByCourseId_batchId_StudentId(Convert.ToInt32(CourseId), Convert.ToInt32(BatchId), Convert.ToInt32(StudentId), Convert.ToInt32(InstituteId));
            return Json(data.Id, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PendingEnquiries()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<AllPendingEnquiresByInstituteid_Result> listItems = EnquiryBL.GetAllPendingEnquiries(Convert.ToInt32(Usermodel.InstituteId));
            return View("ViewPendingEnquiries", listItems);
        }

        public ActionResult OverdueEnquiries()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<AllOverdueEnquiriesForFollowup_Result> listItems = EnquiryBL.GetAllOverdueEnquiries(Convert.ToInt32(Usermodel.InstituteId));
            return View("ViewOverdueEnquiries", listItems);
        }

        public ActionResult TodaysEnquiries()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<TodaysEnquiryFollowupList_Result> listItems = EnquiryBL.GetAllTodaysFollowupEnquiries(Convert.ToInt32(Usermodel.InstituteId));
            return View("ViewTodaysEnquiries", listItems);
        }

        [HttpPost]
        public ActionResult GetSubCourseswithBatch(string CourseId)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            int instituteId = Convert.ToInt32(Usermodel.InstituteId);
            List<AllActiveBatchesOfSubCourses_Result> lstdata = ComboCourseSubjectBL.GetAllActiveBatchesOfComboCourses(instituteId, Convert.ToInt32(CourseId));
            Course courseitem = CourseBL.GetById(Convert.ToInt32(CourseId), instituteId);
            if (courseitem.IsCombo == "Combo")
            {
                ViewBag.ShowDiv = false;
                var data = new { courselst = lstdata, selectionLimit = courseitem.CourseSelectionLimit };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new { courselst = lstdata, selectionLimit = courseitem.CourseSelectionLimit };
                return Json(data, JsonRequestBehavior.AllowGet);
            }


        }
    }
}