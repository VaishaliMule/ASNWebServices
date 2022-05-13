using IMS.Models;
using BussinessLayer;
using TechnosoftModel;
using Newtonsoft.Json;
using Paytm;
using Paytm.Checksum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static IMS.Models.Enum;
using IMS.Controllers;
using IMS;

namespace IMS.Controllers
{
    public class PaytmPaymentController : BaseController
    {
        // GET: InstituteAdmin/PaytmPayment
        public ActionResult Index()
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            PaymentInitiateModel data = new PaymentInitiateModel();
            data.InstituteId = Usermodel.InstituteId;
            data.name = Usermodel.FirstName + " " + Usermodel.MiddleName + " " + Usermodel.LastName;
            data.email = Usermodel.Username;
            data.contactNumber = Usermodel.MobileNo;
            return View(data);
        }
        [HttpPost]
        public ActionResult CreatePayment(PaymentInitiateModel requestData)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
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
                // ewalletObj.AvailableAmount += Convert.ToDecimal(_requestData.amount);
                // EWalletBL.Edit(ewalletObj);
                walletId = ewalletObj.Id;
            }
            EWalletTransation transaction = new EWalletTransation();
            transaction.AdminRemark = "Online Payment by Paytm Gateway";
            transaction.Amount = requestData.amount;
            transaction.EWalletId = walletId;
            transaction.IsApproved = false;
            transaction.IsDeleted = false;
            transaction.PaymentType = "Online";
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = "Credit";
            EWalletTransactionBL.Add(transaction);
            // Generate random receipt number for order

            string transactionId = transaction.Id.ToString();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("MID", Key.merchantId);
            parameters.Add("CHANNEL_ID", "WEB");
            parameters.Add("INDUSTRY_TYPE_ID", "Retail");
            parameters.Add("WEBSITE", "WEBSTAGING");
            parameters.Add("EMAIL", requestData.email);
            parameters.Add("MOBILE_NO", requestData.contactNumber);
            parameters.Add("CUST_ID", requestData.InstituteId.ToString());
            parameters.Add("ORDER_ID", "ORDER"+transactionId);
            parameters.Add("TXN_AMOUNT", requestData.amount.ToString()); 
            parameters.Add("CALLBACK_URL", "http://ims.asntechnosoft.com/InstituteAdmin/PaytmPayment/PaytmResponse");
            //https://localhost:44301/InstituteAdmin/PaytmPayment/PaytmResponse  
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

        [HttpPost]
        public ActionResult PaytmResponse(PaytmResponse response)
        {
            string value = response.ORDERID;
            int startIndex = 5;
            string substring = value.Substring(startIndex, value.Length - 5);
            Console.WriteLine(substring);
            EWalletTransation walltxn = EWalletTransactionBL.GetById(Convert.ToInt32(substring));
            EWallet wallet = EWalletBL.GetById(walltxn.EWalletId);

            PaytmResponse paytmResponse = new PaytmResponse();
            paytmResponse.ORDERID = response.ORDERID;
            paytmResponse.BANKNAME = response.BANKNAME;
            paytmResponse.BANKTXNID = response.BANKTXNID;
            paytmResponse.CHECKSUMHASH = response.CHECKSUMHASH;
            paytmResponse.CURRENCY = response.CURRENCY;
            paytmResponse.GATEWAYNAME = response.GATEWAYNAME;
            paytmResponse.InstituteId = wallet.InstituteId;
            paytmResponse.InstituteName = InstituteBL.GetById(wallet.InstituteId).InstituteName;
            paytmResponse.MID = response.MID;
            paytmResponse.PAYMENTMODE = response.PAYMENTMODE;
            paytmResponse.RESPCODE = response.RESPCODE;
            paytmResponse.RESPMSG = response.RESPMSG;
            paytmResponse.STATUS = response.STATUS;
            paytmResponse.TXNAMOUNT = response.TXNAMOUNT;
            paytmResponse.TXNDATE = response.TXNDATE;
            paytmResponse.TXNID = response.TXNID;


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
            if(response.STATUS=="TXN_SUCCESS")
            {
                ViewBag.Show = true;
            }
            else
            {
                ViewBag.Show = false;
            }
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
                }
            }
            return View("paytmResponse", paytmResponse);
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
                    newreceipt.Discount = walltxn.Discount;
                    if (FeeReceiptBL.Add(newreceipt) == true)
                    {
                       // admobj.TotalFees -= walltxn.Discount;
                        admobj.TotalPaid += Convert.ToDecimal(response.TXNAMOUNT);
                        admobj.Discount += walltxn.Discount;
                        admobj.TotalBalance = (admobj.TotalFees - (admobj.TotalPaid+admobj.Discount) );
                        
                        Student stuobj = StudentBL.GetById(Convert.ToInt32(admobj.StudentId), wallet.InstituteId);
                        Institute instituteObj = InstituteBL.GetById(wallet.InstituteId);
                        string msgtext = "";
                        if (admobj.TotalBalance == 0)
                        {
                            msgtext = "Dear " + stuobj.FirstName + " " + stuobj.LastName + Environment.NewLine + "Your payment of Rs. " + response.TXNAMOUNT + " has been accepted on " + DateTime.Now + ", and your total Course paid fee " + admobj.TotalPaid + " has been cleared. Thank you!" + Environment.NewLine + "Regards," + Environment.NewLine + instituteObj.InstituteName;
                            admobj.IsComplete = true;
                        }
                        else
                        {
                            msgtext = "Dear " + stuobj.FirstName + " " + stuobj.LastName + Environment.NewLine + "Your payment of Rs. " + response.TXNAMOUNT + " has been accepted on " + DateTime.Now + ", your total paid amount upto todays date is" + admobj.TotalPaid + Environment.NewLine + "Now your total balance fee is " + admobj.TotalBalance + ", Thank you!" + Environment.NewLine + "Regards," + Environment.NewLine + instituteObj.InstituteName;
                        }
                        AdmissionBL.Edit(admobj);
                        SMSBL.SendSMS(stuobj.Mobile, msgtext);

                        SweetAlertForReceipt("Done", "Fees receipt added Successfully", NotificationType.success);
                        TempData["UserObject"] =UsersBL.GetById(Convert.ToInt32(admobj.FacultyID),wallet.InstituteId) ;
                        return RedirectToAction("PrintReceipt", new
                        {
                            feesid = newreceipt.Id,
                            admissionid = admobj.Id,
                            instituteid=admobj.InstituteId
                        });
                        
                    }
                }
            }
            return View();
        }
        
        [HttpGet]
        public ActionResult PrintReceipt(int feesid, int admissionid,int instituteid)
        {
           
            Admission Admobj = AdmissionBL.GetById(admissionid, instituteid);
            FeeReceipt fees = FeeReceiptBL.GetById(feesid);
            StudentModel data = new StudentModel();
            data.PaidDate = fees.ReceivedDate.ToString();
            data.FeesId = feesid;

            data.NameOnCertificate = Admobj.NameOnCertificate;
            data.CourseName = CourseBL.GetById(Admobj.CourseId, instituteid).Name;
            Student stuObj = StudentBL.GetById(Convert.ToInt32(Admobj.StudentId), instituteid);
            data.Address = stuObj.Address;
            data.Mobile = stuObj.Mobile;
            data.Email = stuObj.Email;
            data.BatchName = BatchBL.GetById(Admobj.BatchId).Name;
            data.TotalFees =Admobj.TotalFees.ToString();
            data.TotalPaidFees = Admobj.TotalPaid.ToString();
            data.BalanceAmount = Admobj.TotalBalance.ToString();
            data.Paid = fees.Paid.ToString();
            data.Discount = Admobj.Discount.ToString();
            User staffitem = UsersBL.GetById(fees.ReceivedBy, instituteid);
            data.StaffName = staffitem.FirstName+staffitem.LastName;

          
            Institute obj = InstituteBL.GetById(instituteid);

            data.InstituteName = obj.InstituteName;
            string DistrictName = null;
            if (staffitem.CDistrictId != null)
            {
                DistrictName = DistrictBL.GetById(Convert.ToInt32(staffitem.CDistrictId)).Name;
            }

            string TalukaName = null;
            if (staffitem.CTalukaId != null)
            {
                TalukaName = TalukaBL.GetById(Convert.ToInt32(staffitem.CTalukaId)).Name;
            }
            data.ParentMobile = stuObj.ParentMobile;
            DateTime? myDate = stuObj.BirthDate;
            data.DOB = myDate.Value.ToString("MM/dd/yyyy");
            if (obj.Logo != null)
            {
                data.LogoUpload = obj.Logo;
                data.Logofilename = obj.LogoFileName;
            }
            //   data.StudentsPrintFeesReceipts = AdmissionBL.PrintFeesReceiptOfStudent(instituteid, Convert.ToInt32(Admobj.StudentId), Admobj.BatchId);
            data.InstituteAddress = staffitem.CAddress + ",\n" + TalukaName + ",\n" + DistrictName;
            return View("PrintReceipt", data);
        }




        public class TransactionModel
        {
            public int InstituteId { get; set; }
            public string InstituteName { get; set; }
            public string DepositedAmount { get; set; }
            public string UtilizedAmount { get; set; }
            public string AvailableAmount { get; set; }
            public IEnumerable<EWalletTransation> walletTransaction { get; set; }
        }

        [HttpGet]
        public ActionResult MyTransactions()
        {
            User model = TempData["UserObject"] as User;
            TempData.Keep();

            TransactionModel data = new TransactionModel
            {
                InstituteId = model.InstituteId,
                InstituteName = InstituteBL.GetById(Convert.ToInt32(model.InstituteId)).InstituteName,
                DepositedAmount = EWalletBL.GetTotalDeposited(Convert.ToInt32(model.InstituteId)).ToString(),
                UtilizedAmount = EWalletBL.GetTotalUtilised(Convert.ToInt32(model.InstituteId)).ToString(),
                AvailableAmount = EWalletBL.GetAvailableAmount(Convert.ToInt32(model.InstituteId)).ToString(),
                walletTransaction = EWalletBL.GetMyTransactions(Convert.ToInt32(model.InstituteId))
            };

            ViewBag.ShowDiv = false;
            return View("MyTransactions",data);
        }

        public ActionResult ViewStatement(int? instituteid)
        {
            if (instituteid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                TransactionModel data = new TransactionModel();
                data.InstituteId = Convert.ToInt32(instituteid);
                data.InstituteName = InstituteBL.GetById(Convert.ToInt32(instituteid)).InstituteName;
                data.DepositedAmount = EWalletBL.GetTotalDeposited(Convert.ToInt32(instituteid)).ToString();
                data.UtilizedAmount = EWalletBL.GetTotalUtilised(Convert.ToInt32(instituteid)).ToString();
                data.AvailableAmount = EWalletBL.GetAvailableAmount(Convert.ToInt32(instituteid)).ToString();
                data.walletTransaction = EWalletBL.GetMyTransactions(Convert.ToInt32(instituteid));
                if (data.walletTransaction!=null)
                {
                    ViewBag.ShowDiv = true;
                }
                else
                {
                    ViewBag.ShowDiv = false;
                }
                return View("MyTransactions", data);
            }
           
        }
    }
}