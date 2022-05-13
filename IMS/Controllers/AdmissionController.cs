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
using Paytm.Checksum;
using System.IO;

namespace IMS.Controllers
{
    public class AdmissionController : BaseController
    {
        // GET: InstituteAdmin/Admission

        public ActionResult Index()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<StudentListForAdmissionDetailsByInstituteId_Result> listItems = AdmissionBL.GetStudentAdmissionByInstituteId(Convert.ToInt32(Usermodel.InstituteId));
            System.Threading.Thread.Sleep(3000);
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
        public ActionResult Search(string firstname, string lastname)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<Student> listItems = StudentBL.SearchBy_firstname_lastname(firstname, lastname, Convert.ToInt32(Usermodel.InstituteId));
            ViewBag.firstname = firstname;
            ViewBag.lastname = lastname;

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
        public ActionResult AddAdmission(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            //  IMSDataModel.Student obj = StudentBL.GetById(Convert.ToInt32(id), Convert.ToInt32(Usermodel.InstituteId));
            StudentForEditById_Result obj = StudentBL.GetStudentForEditByStudentId(Convert.ToInt32(Usermodel.InstituteId), Convert.ToInt32(id));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                StudentModel data = new StudentModel();
                data.BatchNames = PopulateBatches();
                data.CourseFees = Populateinstallments();
                data.IdentityProofs = PopulateIdentityProofs();
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
                if (obj.MiddleName != null)
                {
                    data.NameOnCertificate = obj.LastName + " " + obj.FirstName + " " + obj.MiddleName;
                    data.Fullname = obj.LastName + " " + obj.FirstName + " " + obj.MiddleName;
                }
                else
                {
                    data.NameOnCertificate = obj.LastName + " " + obj.FirstName;
                    data.Fullname = obj.LastName + " " + obj.FirstName;
                }
                List<AllActiveBatchesOfSubCourses_Result> listItems = new List<AllActiveBatchesOfSubCourses_Result>();
                data.subcoursesWithBAtches = listItems;
                ViewBag.ShowDiv = false;
                return View("Admission", data);
            }
        }
        [HttpPost]
        public ActionResult AddAdmission(StudentModel data, string[] combocourse)
        {
            Admission obj = new Admission();
            obj.AdmissionDate = DateTime.Now;
            obj.BatchId = data.BatchId;
            obj.CourseId = data.CourseId;

            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();

            Student stuobj = StudentBL.GetById(data.StudentId, Usermodel.InstituteId);

            obj.FacultyID = Usermodel.Id;
            if (data.IdentityProofId != 0)
            {
                obj.IdentityProof = IdentityProofBL.GetById(data.IdentityProofId).Name;
            }
            if (data.CourseFeesId != 0)
                obj.InstallmentMode = CourseFeeBL.GetById(data.CourseFeesId).NoOfInstallments;
            obj.InstituteId = data.InstituteId;
            obj.IsComplete = false;
            obj.LanguageOfStudy = stuobj.MotherTongueLanguageId;
            obj.NameOnCertificate = data.Fullname;
            obj.StudentId = data.StudentId;

            CourseFee fees = CourseFeeBL.GetByCourse_Installment(obj.CourseId, Convert.ToInt32(obj.InstallmentMode));
            obj.Discount = Convert.ToDecimal(0);

            obj.TotalFees = fees.FeeAmount;
            obj.TotalPaid = Convert.ToDecimal(0);
            obj.TotalBalance = fees.FeeAmount - obj.TotalPaid;
            obj.IsDeleted = false;
            obj.IsComboCourseStudent = false;
            if (AdmissionBL.Add(obj) == true)
            {
                FeesFollowupHistory feesobj = new FeesFollowupHistory();
                feesobj.AdmissionId = obj.Id;
                feesobj.Comment = "First installment paid upto month end";
                feesobj.FacultyId = Usermodel.Id;
                feesobj.FollowupDate = DateTime.Now;
                feesobj.NextFollowupDate = DateTime.Today.AddDays(15);
                feesobj.IsDropout = false;
                feesobj.IsFollowup = true;
                feesobj.ReasonForDropOut = null;
                FeesFollowupHistoryBL.Add(feesobj);

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
                //string msgtext = "Dear " + stuobj.FirstName + " " + stuobj.LastName + Environment.NewLine + "Welcome to " + instituteObj.InstituteName + ", thanks for choosing us for Course " + CourseBL.GetById(data.CourseId, Usermodel.InstituteId).Name + Environment.NewLine + "Please feel free to contact for any query back with us." + Environment.NewLine + "Regards," + Environment.NewLine + instituteObj.InstituteName;
                //SMSBL.SendSMS(stuobj.Mobile, msgtext);

                SweetAlert("Done", "Admission added Successfully", NotificationType.success);
                return RedirectToAction("FeesReceipt", "Admission", new { id = obj.Id });
            }
            return View();
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
            //IMSDataModel.Student obj = StudentBL.GetById(Convert.ToInt32(id), instituteId);
            StudentForEditById_Result obj = StudentBL.GetStudentForEditByStudentId(instituteId, Convert.ToInt32(id));
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
                data.Email = obj.Email;
                data.MarritalStatus = obj.MaritalStatus;
                data.Qualification = obj.QualificationName;
                data.Langauage = obj.LanguageName;
                data.Mobile = obj.Mobile;
                data.ParentMobile = obj.ParentMobile;
                data.Address = obj.Address;
                data.Area = obj.suburb;
                data.PinCode = obj.Pincode;
                data.StudentsEnquires = EnquiryBL.GetAllEnquiryOfStudentByInstituteId(instituteId, studentid);
                data.StudentsAdmissions = AdmissionBL.GetAllAdmissionOfStudentByInstituteId(instituteId, studentid);
                return View("ViewAdmission", data);
            }
        }
        [HttpGet]
        public ActionResult FeesReceipt(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            Admission obj = AdmissionBL.GetById(Convert.ToInt32(id), Convert.ToInt32(Usermodel.InstituteId));
            // IMSDataModel.Student obj = StudentBL.GetById(Convert.ToInt32(id), Convert.ToInt32(Usermodel.InstituteId));
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                StudentModel data = new StudentModel();
                data.NameOnCertificate = obj.NameOnCertificate;
                //if (obj.MiddleName != null)
                //{
                //    data.NameOnCertificate = obj.LastName + " " + obj.FirstName + " " + obj.MiddleName;
                //}
                //else
                //{
                //    data.NameOnCertificate = obj.LastName + " " + obj.FirstName;
                //}
                data.CourseName = CourseBL.GetById(obj.CourseId, Convert.ToInt32(Usermodel.InstituteId)).Name;
                data.BatchName = BatchBL.GetById(obj.BatchId).Name;
                data.TotalFees = (obj.TotalFees - obj.Discount).ToString();
                data.TotalPaidFees = obj.TotalPaid.ToString();
                data.BalanceAmount = obj.TotalBalance.ToString();
                data.BatchId = obj.BatchId;
                data.InstituteId = obj.InstituteId;
                data.AdmissionId = Convert.ToInt32(id);
                return View("FeesReceipt", data);
            }
        }
        [HttpPost]
        public ActionResult FeesReceipt(StudentModel data)
        {
            FeeReceipt fee = new FeeReceipt();
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            fee.BatchId = data.BatchId;
            fee.Discount = Convert.ToDecimal(data.Discount);
            Admission obj = AdmissionBL.GetById(data.AdmissionId, Convert.ToInt32(Usermodel.InstituteId));
            fee.Installment = obj.InstallmentMode;
            fee.ModeOfPayment = data.ModeOfPayment;
            fee.Paid = Convert.ToDecimal(data.Paid);

            // Staff staffitem = StaffBL.GetByEmail(Usermodel.Username, Convert.ToInt32(Usermodel.InstituteId));
            fee.ReceivedBy = Usermodel.Id;
            fee.ReceivedDate = DateTime.Now;
            fee.StudentId = Convert.ToInt32(obj.StudentId);
           Student stuobj = StudentBL.GetById(Convert.ToInt32(obj.StudentId), Usermodel.InstituteId);
            if (data.ModeOfPayment == "1")
            {
                if (FeeReceiptBL.Add(fee) == true)
                {
                    // obj.TotalFees = Convert.ToDecimal(data.TotalFees);
                    obj.TotalPaid += Convert.ToDecimal(data.Paid);
                    obj.Discount += Convert.ToDecimal(data.Discount);
                    obj.TotalBalance = (obj.TotalFees - (obj.TotalPaid + obj.Discount));
                    Institute instituteObj = InstituteBL.GetById(Usermodel.InstituteId);
                    string msgtext = "";
                    if (obj.TotalBalance == 0)
                    {
                        msgtext = "Dear " + stuobj.FirstName + " " + stuobj.LastName + Environment.NewLine + "Your payment of Rs. " + data.Paid + " has been accepted on " + DateTime.Now + ", and your total Course paid fee " + obj.TotalPaid + " has been cleared. Thank you!" + Environment.NewLine + "Regards," + Environment.NewLine + instituteObj.InstituteName;
                        obj.IsComplete = true;
                    }
                    else
                    {
                        msgtext = "Dear " + stuobj.FirstName + " " + stuobj.LastName + Environment.NewLine + "Your payment of Rs. " + data.Paid + " has been accepted on " + DateTime.Now + ", your total paid amount upto todays date is " + obj.TotalPaid + Environment.NewLine + "Now your total balance fee is " + obj.TotalBalance + ", Thank you!" + Environment.NewLine + "Regards," + Environment.NewLine + instituteObj.InstituteName;
                    }
                    AdmissionBL.Edit(obj);
                    //  SMSBL.SendSMS(stuobj.Mobile, msgtext);

                    SweetAlertForReceipt("Done", "Fees receipt added Successfully", NotificationType.success);
                    return RedirectToAction("PrintReceipt", new
                    {
                        feesid = fee.Id,
                        admissionid = data.AdmissionId
                    });
                }
            }
            else
            {
                EWallet ewalletObj = EWalletBL.GetByInstituteId(Usermodel.InstituteId);
                int walletId = 0;
                if (ewalletObj == null)
                {
                    EWallet newwallet = new EWallet();
                    newwallet.AvailableAmount = 0;
                    newwallet.InstituteId = Usermodel.InstituteId;
                    newwallet.IsActive = false;
                    EWalletBL.Add(newwallet);
                    walletId = newwallet.Id;
                }
                else
                {
                    walletId = ewalletObj.Id;
                }
                EWalletTransation transaction = new EWalletTransation();
                transaction.AdminRemark = "Online Payment by Learner using PG";
                transaction.Amount = Convert.ToDecimal(data.Paid);
                transaction.EWalletId = walletId;
                transaction.IsApproved = false;
                transaction.IsDeleted = false;
                transaction.PaymentType = "Online";
                transaction.TransactionDate = DateTime.Now;
                transaction.TransactionType = "Credit";
                transaction.LearnerId = obj.Id;
                transaction.Discount = Convert.ToDecimal(data.Discount);
                EWalletTransactionBL.Add(transaction);
                // Generate random receipt number for order

                string transactionId = transaction.Id.ToString();

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("MID", Key.merchantId);
                parameters.Add("CHANNEL_ID", "WEB");
                parameters.Add("INDUSTRY_TYPE_ID", "Retail");
                parameters.Add("WEBSITE", "WEBSTAGING");
                parameters.Add("EMAIL", Usermodel.Username);
                parameters.Add("MOBILE_NO", stuobj.Mobile);
                parameters.Add("CUST_ID", obj.StudentId.ToString());
                parameters.Add("ORDER_ID", "ORDER" + transactionId);
                parameters.Add("TXN_AMOUNT", data.Paid.ToString());
                parameters.Add("CALLBACK_URL", "http://ims.asntechnosoft.com/InstituteAdmin/PaytmPayment/FeePaytmResponse");
                ////   https://localhost:44301/InstituteAdmin/PaytmPayment/FeePaytmResponse
                string checksum = CheckSum.GenerateCheckSum(Key.merchantKey, parameters);
                // string paytmURL = "https://securegw-stage.paytm.in/theia/processTransaction?orderid=" + parameters.FirstOrDefault(x => x.Key == "ORDER_ID").Value;

                string paytmURL = "https://securegw.paytm.in/theia/processTransaction?orderid=" + parameters.FirstOrDefault(x => x.Key == "ORDER_ID").Value;

                string outputHTML = "<html>";
                outputHTML += "<head>";
                outputHTML += "<title>Merchant Check Out Page</title>";
                outputHTML += "</head>";
                outputHTML += "<body>";
                outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
                outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
                outputHTML += "<table border='1'>";
                outputHTML += "<tbody>";

                foreach (string key in parameters.Keys)
                {
                    outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
                }

                outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
                outputHTML += "</tbody>";
                outputHTML += "</table>";
                outputHTML += "<script type='text/javascript'>";
                outputHTML += "document.f1.submit();";
                outputHTML += "</script>";
                outputHTML += "</form>";
                outputHTML += "</body>";
                outputHTML += "</html>";

                ViewBag.htmlData = outputHTML;

                return View("PaytmPaymentPage");
            }
            return View();
        }
        [HttpGet]
        public ActionResult PrintReceipt(int feesid, int admissionid)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            Admission Admobj = AdmissionBL.GetById(admissionid, Convert.ToInt32(Usermodel.InstituteId));
            FeeReceipt fees = FeeReceiptBL.GetById(feesid);
            StudentModel data = new StudentModel();
            data.PaidDate = fees.ReceivedDate.ToString();
            data.FeesId = feesid;

            data.NameOnCertificate = Admobj.NameOnCertificate;
            data.CourseName = CourseBL.GetById(Admobj.CourseId, Convert.ToInt32(Usermodel.InstituteId)).Name;
            Student stuObj = StudentBL.GetById(Convert.ToInt32(Admobj.StudentId), Convert.ToInt32(Usermodel.InstituteId));
            data.Address = stuObj.Address;
            data.Mobile = stuObj.Mobile;
            data.Email = stuObj.Email;
            data.BatchName = BatchBL.GetById(Admobj.BatchId).Name;
            data.TotalFees = Admobj.TotalFees.ToString();
            data.TotalPaidFees = Admobj.TotalPaid.ToString();
            data.BalanceAmount = Admobj.TotalBalance.ToString();
            data.Paid = fees.Paid.ToString();
            data.Discount = Admobj.Discount.ToString();
            // Staff staffitem = StaffBL.GetById(fees.ReceivedBy, Convert.ToInt32(Usermodel.InstituteId));
            data.StaffName = Usermodel.FirstName + " " + Usermodel.LastName;

            int instituteId = Convert.ToInt32(Usermodel.InstituteId);
            TempData.Keep();
            Institute obj = InstituteBL.GetById(instituteId);

            data.InstituteName = obj.InstituteName;
            string DistrictName = null;
            if (Usermodel.CDistrictId != null)
            {
                DistrictName = DistrictBL.GetById(Convert.ToInt32(Usermodel.CDistrictId)).Name;
            }

            string TalukaName = null;
            if (Usermodel.CTalukaId != null)
            {
                TalukaName = TalukaBL.GetById(Convert.ToInt32(Usermodel.CTalukaId)).Name;
            }
            data.ParentMobile = stuObj.ParentMobile;
            DateTime? myDate = stuObj.BirthDate;
            data.DOB = myDate.Value.ToString("MM/dd/yyyy");

            // data.StudentsPrintFeesReceipts = AdmissionBL.PrintFeesReceiptOfStudent(Convert.ToInt32(Usermodel.InstituteId), Convert.ToInt32(Admobj.StudentId), Admobj.BatchId);
            data.InstituteAddress = Usermodel.CAddress + ",\n" + TalukaName + ",\n" + DistrictName;
            if (obj.Logo != null)
            {
                data.LogoUpload = obj.Logo;
                data.Logofilename = obj.LogoFileName;
            }
            return View("PrintReceipt", data);
        }

        [HttpPost]
        public ActionResult FeePaytmResponse(PaytmResponse response)
        {
            string value = response.ORDERID;
            int startIndex = 5;
            string substring = value.Substring(startIndex, value.Length - 5);
            Console.WriteLine(substring);
            EWalletTransation walltxn = EWalletTransactionBL.GetById(Convert.ToInt32(substring));
            EWallet wallet = EWalletBL.GetById(walltxn.EWalletId);

            //Save Data in Database
            PaytmPaymentResponse dbResponse = new PaytmPaymentResponse();
            dbResponse.ORDERID = response.ORDERID;
            dbResponse.BANKNAME = response.BANKNAME;
            dbResponse.BANKTXNID = response.BANKTXNID;
            dbResponse.CHECKSUMHASH = response.CHECKSUMHASH;
            dbResponse.CURRENCY = response.CURRENCY;
            dbResponse.GATEWAYNAME = response.GATEWAYNAME;
            dbResponse.EWalletTransactionId = walltxn.Id;
            dbResponse.MID = response.MID;
            dbResponse.PAYMENTMODE = response.PAYMENTMODE;
            dbResponse.RESPCODE = response.RESPCODE;
            dbResponse.RESPMSG = response.RESPMSG;
            dbResponse.STATUS = response.STATUS;
            dbResponse.TXNAMOUNT = response.TXNAMOUNT;
            dbResponse.TXNDATE = response.TXNDATE;
            dbResponse.TXNID = response.TXNID;

            PaytmPaymentResponse paytmPaymentResponse = PaytmPaymentResponseBL.GetById(walltxn.Id);
            if (paytmPaymentResponse == null)
            {
                PaytmPaymentResponseBL.Add(dbResponse);
                if (response.RESPCODE == "01")
                {
                    if (wallet.Id != 0)
                    {
                        wallet.AvailableAmount += Convert.ToDecimal(response.TXNAMOUNT);
                        wallet.IsActive = true;
                        EWalletBL.Edit(wallet);
                    }
                    if (walltxn.Id != 0)
                    {
                        walltxn.Amount = Convert.ToDecimal(response.TXNAMOUNT);
                        walltxn.EWalletId = wallet.Id;
                        walltxn.IsApproved = true;
                        walltxn.IsDeleted = false;
                        walltxn.TransactionDate = DateTime.Now;
                        EWalletTransactionBL.Edit(walltxn);
                    }
                    Admission admobj = AdmissionBL.GetById(Convert.ToInt32(walltxn.LearnerId), wallet.InstituteId);
                    FeeReceipt newreceipt = new FeeReceipt();
                    newreceipt.BankBranch = response.BANKNAME;
                    newreceipt.BatchId = admobj.BatchId;
                    newreceipt.CustomerDetail_BankName = response.BANKNAME;
                    newreceipt.Installment = admobj.InstallmentMode;
                    newreceipt.ModeOfPayment = "2";
                    newreceipt.Paid = Convert.ToDecimal(response.TXNAMOUNT);
                    newreceipt.ReceivedBy = Convert.ToInt32(admobj.FacultyID);//here i added id as admission added by staff
                    newreceipt.ReceivedDate = response.TXNDATE;
                    newreceipt.StudentId = Convert.ToInt32(admobj.StudentId);
                    newreceipt.Transactio_ChequeDate = response.TXNDATE;
                    newreceipt.UTR_ChequeNo = response.TXNID;
                    if (FeeReceiptBL.Add(newreceipt) == true)
                    {
                        admobj.TotalPaid += Convert.ToDecimal(response.TXNAMOUNT);
                        admobj.Discount += walltxn.Discount;
                        admobj.TotalBalance = (admobj.TotalFees - (admobj.TotalPaid + admobj.Discount));
                        Student stuobj = StudentBL.GetById(Convert.ToInt32(admobj.StudentId), wallet.InstituteId);
                        string msgtext = "";
                        if (admobj.TotalBalance == 0)
                        {
                            msgtext = "Dear " + stuobj.FirstName + " " + stuobj.LastName + Environment.NewLine + "your payment of " + response.TXNAMOUNT + " has been accepted on " + DateTime.Now + ", and your total Course paid fee " + admobj.TotalPaid + " has been cleared. Thank you!";
                            admobj.IsComplete = true;
                        }
                        else
                        {
                            msgtext = "Dear " + stuobj.FirstName + " " + stuobj.LastName + Environment.NewLine + "your payment of " + response.TXNAMOUNT + " has been accepted on " + DateTime.Now + ", your total paid amount upto todays date is" + admobj.TotalPaid + Environment.NewLine + "Now your total balance fee is " + admobj.TotalBalance + " Thank you!" + Environment.NewLine + "Please feel free to contact with us.";
                        }
                        AdmissionBL.Edit(admobj);
                        // SMSBL.SendSMS(stuobj.Mobile, msgtext);

                        SweetAlert("Done", "Fees receipt added Successfully", NotificationType.success);
                        return RedirectToAction("PrintReceipt", new
                        {
                            feesid = newreceipt.Id,
                            admissionid = admobj.Id
                        });
                    }
                }
            }
            return View();
        }
        public ActionResult SearchByCourseBatch()
        {
            StudentModel data = new StudentModel();
            List<StudentFeesReceiptBy_Courseid_batchid_Result> listItems = new List<StudentFeesReceiptBy_Courseid_batchid_Result>();
            data.StudentsFeesReceipts = listItems;
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
            return View("FeesByCourseAndBatch", data);
        }
        [HttpPost]
        public ActionResult SearchByCourseBatch(StudentModel data)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<StudentFeesReceiptBy_Courseid_batchid_Result> listItems = AdmissionBL.GetFeesReceiptOfStudentByCourseidandBatchid(data.CourseId, data.BatchId, Convert.ToInt32(Usermodel.InstituteId));
            data.StudentsFeesReceipts = listItems.Where(a => a.IsComboCourseStudent == false);
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
            return View("FeesByCourseAndBatch", data);
        }
        [HttpGet]
        public ActionResult SearchForFileUpload()
        {
            StudentModel data = new StudentModel();
            List<StudentFeesReceiptBy_Courseid_batchid_Result> listItems = new List<StudentFeesReceiptBy_Courseid_batchid_Result>();
            data.StudentsFeesReceipts = listItems;
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
            return View("UploadPhotoAndSign", data);

        }
        [HttpPost]
        public ActionResult SearchForFileUploadBase(StudentModel data)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<StudentFeesReceiptBy_Courseid_batchid_Result> listItems = AdmissionBL.GetFeesReceiptOfStudentByCourseidandBatchid(data.CourseId, data.BatchId, Convert.ToInt32(Usermodel.InstituteId));
            data.StudentsFeesReceipts = listItems;
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
            return View("UploadPhotoAndSign", data);
        }

        [HttpGet]
        public ActionResult AddPhotoAndSign(int? id, int? courseid, int? batchid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
            int instituteId = Convert.ToInt32(Usermodel.InstituteId);
            TempData.Keep();
            Student obj = StudentBL.GetById(Convert.ToInt32(id), instituteId);
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                StudentModel data = new StudentModel();
                data.InstituteId = instituteId;
                data.StudentId = Convert.ToInt32(id);
                data.PhotoUpload = obj.Photo;
                string photoProofPath = obj.Photo;
                string Photofilename = null;
                Photofilename = Path.GetFileName(photoProofPath);
                data.Photofilename = Photofilename;
                data.SignUpload = obj.Signature;
                string signProofPath = obj.Signature;
                string signfilename = null;
                signfilename = Path.GetFileName(signProofPath);
                data.Signfilename = signfilename;
                //  data.PhotoImageFile = obj.Photo;
                data.CourseId = Convert.ToInt32(courseid);
                data.BatchId = Convert.ToInt32(batchid);

                return View("AddPhotoAndSign", data);
            }
            //return View("AddPhotoAndSign");
        }
        [HttpPost]
        public ActionResult AddPhotoAndSign(StudentModel data)
        {
            Student obj = StudentBL.GetById(Convert.ToInt32(data.StudentId), data.InstituteId);
            HttpPostedFileBase Photofile = data.PhotoImageFile;
            if (Photofile != null && Photofile.ContentLength > 0)
            {
                string FileName = Path.GetFileNameWithoutExtension(Photofile.FileName);
                string FileExtension = Path.GetExtension(Photofile.FileName);
                FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                string UploadPath = Path.Combine(Server.MapPath("~/UserImages"), FileName);
                data.PhotoUpload = UploadPath.ToString();
                Photofile.SaveAs(data.PhotoUpload);
                obj.Photo = data.PhotoUpload.ToString();
                data.Photofilename = FileName;
                obj.PhotoFileName = FileName;
            }


            HttpPostedFileBase Signfile = data.SignImageFile;
            if (Signfile != null && Signfile.ContentLength > 0)
            {
                string FileName = Path.GetFileNameWithoutExtension(Signfile.FileName);
                string FileExtension = Path.GetExtension(Signfile.FileName);
                FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                string UploadPath = Path.Combine(Server.MapPath("~/UserImages"), FileName);
                data.SignUpload = UploadPath.ToString();
                Signfile.SaveAs(data.SignUpload);
                obj.Signature = data.SignUpload.ToString();
                data.Signfilename = FileName;
                obj.SignFileNAme = FileName;
            }


            List<StudentFeesReceiptBy_Courseid_batchid_Result> listItems = AdmissionBL.GetFeesReceiptOfStudentByCourseidandBatchid(data.CourseId, data.BatchId, Convert.ToInt32(data.InstituteId));
            data.StudentsFeesReceipts = listItems;
            List<SelectListItem> CourseNames = new List<SelectListItem>();
            List<AllCoursesByInstituteId_Result> courses = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(data.InstituteId));
            courses.ForEach(x =>
            {
                CourseNames.Add(new SelectListItem { Text = x.COURSENAME.ToString(), Value = x.COURSEID.ToString() });
            });
            data.CourseNames = CourseNames;
            //data.BatchNames = PopulateBatches();

            List<SelectListItem> BatchNames = new List<SelectListItem>();
            List<BatchByCourseId_InstituteId_Result> batch = BatchBL.GetBatchesofcourseid_instituteid(data.CourseId, Convert.ToInt32(data.InstituteId));
            batch.ForEach(x =>
            {
                BatchNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            data.BatchNames = BatchNames;
            if (StudentBL.Edit(obj) == true)
            {
                SweetAlert("Done", "Photo and Sign added Successfully", NotificationType.success);
                // return RedirectToAction("SearchForFileUpload", data);

                return Redirect(Request.UrlReferrer.ToString());

            }

            return View();
        }

        [HttpPost]
        public ActionResult AddPhoto(int? StudentId, HttpPostedFileBase PhotoImageFile, int CourseId, int BatchId)
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
                    StudentModel data = FetchModelData(CourseId, BatchId, Usermodel);

                    SweetAlert("Done", "Photo uploaded Successfully", NotificationType.success);
                    return View("UploadPhotoAndSign", data);

                }
            }
            else
            {
                StudentModel data = FetchModelData(CourseId, BatchId, Usermodel);
                SweetAlert("Error", "Please select photo to upload.", NotificationType.error);
                return View("UploadPhotoAndSign", data);
            }


            return View();
        }

        private StudentModel FetchModelData(int CourseId, int BatchId, User Usermodel)
        {
            StudentModel data = new StudentModel();
            List<SelectListItem> CourseNames = new List<SelectListItem>();
            List<AllCoursesByInstituteId_Result> courses = CourseBL.GetAllCoursesByInstitutes(Convert.ToInt32(Usermodel.InstituteId));
            courses.ForEach(x =>
            {
                CourseNames.Add(new SelectListItem { Text = x.COURSENAME.ToString(), Value = x.COURSEID.ToString() });
            });
            data.CourseNames = CourseNames;
            data.CourseId = CourseId;
            List<SelectListItem> BatchNames = new List<SelectListItem>();

            List<BatchByCourseId_InstituteId_Result> batch = BatchBL.GetBatchesofcourseid_instituteid(CourseId, Convert.ToInt32(Usermodel.InstituteId));
            batch.ForEach(x =>
            {
                BatchNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            data.BatchNames = BatchNames;
            data.BatchId = BatchId;
            List<StudentFeesReceiptBy_Courseid_batchid_Result> listItems = AdmissionBL.GetFeesReceiptOfStudentByCourseidandBatchid(CourseId, BatchId, Convert.ToInt32(Usermodel.InstituteId));
            data.StudentsFeesReceipts = listItems;
            ViewBag.ShowDiv = true;
            return data;
        }

        [HttpPost]
        public ActionResult AddSign(int? StudentId, HttpPostedFileBase SignImageFile, int CourseId, int BatchId)
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
                    StudentModel data = FetchModelData(CourseId, BatchId, Usermodel);

                    SweetAlert("Done", "Sign uploaded Successfully", NotificationType.success);
                    return View("UploadPhotoAndSign", data);
                }
            }
            else
            {
                StudentModel data = FetchModelData(CourseId, BatchId, Usermodel);
                SweetAlert("Error", "Please select Sign to upload.", NotificationType.error);
                return View("UploadPhotoAndSign", data);
            }


            return View();
        }

        public ActionResult CoursewiseCount()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            List<AdmissionCountCoursewisebyInstituteid_Result> listItems = AdmissionBL.GetAllCoursewiseAdmissionCount(Convert.ToInt32(Usermodel.InstituteId));
            return View("ViewCoursewiseCount", listItems);
        }

        

        [HttpGet]
        public ActionResult FileUploadModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            Admission obj = AdmissionBL.GetById(Convert.ToInt32(id), Usermodel.InstituteId);
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                StudentModel data = new StudentModel();
                data.InstituteId = Usermodel.InstituteId;
                int studentid = Convert.ToInt32(obj.StudentId);
                data.StudentId = studentid;
                Student studobj = StudentBL.GetById(studentid, Usermodel.InstituteId);
                data.PhotoUpload = studobj.Photo;
                data.Photofilename = studobj.PhotoFileName;
                data.SignUpload = studobj.Signature;
                data.Signfilename = studobj.SignFileNAme;
                data.NameOnCertificate = studobj.FirstName + " " + studobj.MiddleName + " " + studobj.LastName;
                data.CourseId = obj.CourseId;
                data.BatchId = obj.BatchId;
                return View("_ImageUpload", data);
            }
        }

        [HttpPost]
        public ActionResult FileUploadModal(StudentModel data)
        {
            //User Usermodel = TempData["UserObject"] as User;
            //TempData.Keep();
            int instututeId = data.InstituteId;
            int studentId = data.StudentId;
            int courseId = data.CourseId;
            int batchId = data.BatchId;
            Student obj = StudentBL.GetById(studentId, instututeId);
            HttpPostedFileBase PhotoImageFile = data.PhotoImageFile;
            if (PhotoImageFile != null && PhotoImageFile.ContentLength > 0)
            {
                string FileName = Path.GetFileNameWithoutExtension(PhotoImageFile.FileName);
                string FileExtension = Path.GetExtension(PhotoImageFile.FileName);
                FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                string UploadPath = Path.Combine(Server.MapPath("~/UserImages"), FileName);
                PhotoImageFile.SaveAs(UploadPath);
                obj.Photo = UploadPath.ToString();
                obj.PhotoFileName = FileName;
            }
            //else
            //{
            //    FetchImageData(data, instututeId, courseId, batchId);
            //    SweetAlert("Error", "Please select photoand sign to upload.", NotificationType.error);
            //    return View("UploadPhotoAndSign", data);
            //}
            HttpPostedFileBase SignImageFile = data.SignImageFile;
            if (SignImageFile != null && SignImageFile.ContentLength > 0)
            {
                string FileName = Path.GetFileNameWithoutExtension(SignImageFile.FileName);
                string FileExtension = Path.GetExtension(SignImageFile.FileName);
                FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                string UploadPath = Path.Combine(Server.MapPath("~/UserImages"), FileName);
                SignImageFile.SaveAs(UploadPath);
                obj.Signature = UploadPath.ToString();
                obj.SignFileNAme = FileName;

            }
            if (StudentBL.Edit(obj) == true)
            {
                FetchImageData(data, instututeId, courseId, batchId);
                SweetAlert("Done", "Photo and Sign uploaded Successfully", NotificationType.success);
                return View("UploadPhotoAndSign", data);

            }

            return View();
        }

        private void FetchImageData(StudentModel data, int instututeId, int courseId, int batchId)
        {
            List<SelectListItem> CourseNames = new List<SelectListItem>();
            List<AllCoursesByInstituteId_Result> courses = CourseBL.GetAllCoursesByInstitutes(instututeId);
            courses.ForEach(x =>
            {
                CourseNames.Add(new SelectListItem { Text = x.COURSENAME.ToString(), Value = x.COURSEID.ToString() });
            });
            data.CourseNames = CourseNames;
            //   data.CourseId = CourseId;
            List<SelectListItem> BatchNames = new List<SelectListItem>();

            List<BatchByCourseId_InstituteId_Result> batch = BatchBL.GetBatchesofcourseid_instituteid(courseId, instututeId);
            batch.ForEach(x =>
            {
                BatchNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            });
            data.BatchNames = PopulateBatches();
            //  data.BatchId = BatchId;
            List<StudentFeesReceiptBy_Courseid_batchid_Result> listItems = AdmissionBL.GetFeesReceiptOfStudentByCourseidandBatchid(courseId, batchId, instututeId);
            data.StudentsFeesReceipts = listItems;
            ViewBag.ShowDiv = true;
        }

        [HttpPost]
        public ActionResult GetBatch(string CourseId)
        {
            int courseId;
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            int instituteId = Convert.ToInt32(Usermodel.InstituteId);
            List<SelectListItem> BatchNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(CourseId))
            {

                courseId = Convert.ToInt32(CourseId);
                List<BatchByCourseId_InstituteId_Result> batches = BatchBL.GetBatchesofcourseid_instituteid(courseId, instituteId);
                batches.ForEach(x =>
                {
                    BatchNames.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                });
            }

            return Json(BatchNames, JsonRequestBehavior.AllowGet);

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
                var data = new { courselst = lstdata, selectionLimit = courseitem.CourseSelectionLimit };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new { courselst = lstdata, selectionLimit = courseitem.CourseSelectionLimit };
                return Json(data, JsonRequestBehavior.AllowGet);
            }


        }


        [HttpPost]
        public ActionResult GetCourseFeeInstallments(string CourseId, string InstallmentMode, string InstituteId)
        {
            List<CourseFeeBy_Courseid_InstallmentMode_Result> data = CourseFeeBL.GetCourseFeeBy_CourseId_InstallmentMode(Convert.ToInt32(InstituteId), Convert.ToInt32(CourseId), Convert.ToInt32(InstallmentMode));
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}