

using ASNTechnosoft.Models;
using BussinessLayer;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TechnosoftModel;
using static ASNTechnosoft.Areas.ASNAdmin.Models.Enum;

namespace ASNTechnosoft.Controllers
{
    public class AccountController : BaseController
    {
       
        public ActionResult Index()
        {
            return View("Login");
        }

        public ActionResult PersonalDetails()
        {
            return View("PersonalDetails");
        }

        private Institute GetUser()
        {
            if (Session["user"] == null)
            {
                Session["user"] = new Institute();
            }
            return (Institute)Session["user"];
        }
        private void RemoveUser()
        {
            Session.Remove("user");
        }

        [HttpPost]
        public ActionResult PersonalDetails(InstituteRegistration model, string nextBtn)
        {
            if (nextBtn != null)
            {
                if (ModelState.IsValid)
                {
                    Institute obj = GetUser();
                    obj.FirstName = model.FirstName;
                    obj.Email = model.Email;
                    obj.InstituteName = model.InstituteName;
                    obj.LastName = model.LastName;
                    obj.MiddleName = model.MiddleName;
                    obj.Mobile = model.Mobile;

                    //string strNewPassword = GeneratePassword().ToString();
                    //obj.Password = strNewPassword;

                    string smsOtp = SendSMSToUSerForOTPVerification(model.Mobile);
                    obj.SMSOtp = smsOtp.ToString();
                    string emailOtp = SendEmailToUserForVerification(model.Email);
                    obj.EmailOTP = emailOtp.ToString();
                    return RedirectToAction("OTPVerification");
                }
            }
            return View();
        }

        public string GeneratePassword()
        {
            string PasswordLength = "8";
            string NewPassword = "";

            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";

            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string IDString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                NewPassword = IDString;
            }
            return NewPassword;
        }
        public string SendSMSToUSerForOTPVerification(string number)
        {
            Random r = new Random();
            string OTP = r.Next(1000, 9999).ToString();

            //Send message
            string userName = "ASNTECHNOSOFT";
            string password = "Password@2017";
            string senderId = "ASNTEC";
            string msgType = "TXT";
            // string number = "9876543210";
            string smsText = "Your OTP code for Mobile verification is : " + OTP + Environment.NewLine + "Regards," + Environment.NewLine + "ASN Technosoft" + Environment.NewLine + "7083528282";
            string uri = "http://www.smsjust.com/blank/sms/user/urlsms.php?username={0}&pass={1}&senderid={2}&dest_mobileno={3}&message={4}&msgtype={5}&response=Y";
            string url = string.Format(uri, userName, password, senderId, number, smsText, msgType);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();


            return OTP;
        }

        public string SendEmailToUserForVerification(string emailId)
        {
            Random r = new Random();
            string OTP = r.Next(1000, 9999).ToString();
            if (emailId != null)
            {
                string FROM_EMAIL = "asntechnosoft@gmail.com";
                string PASSWORD = "7066922020";
                string DISPLAY_NAME = "ASN Technosoft";

                MailMessage mail = new System.Net.Mail.MailMessage();
                mail.To.Add(emailId);
                mail.From = new MailAddress(FROM_EMAIL, DISPLAY_NAME, System.Text.Encoding.UTF8);
                mail.Subject = "Email Verification!";
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = "<br/> Your Email OTP is: " + OTP;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";   // We use gmail as our smtp client
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(FROM_EMAIL, PASSWORD);
                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return OTP;
        }

        [HttpGet]
        public ActionResult OTPVerification(InstituteRegistration model)
        {
            return View("OTPVerification");
        }

        [HttpPost]
        public ActionResult OTPVerification(InstituteRegistration model, string prevBtn, string nextBtn)
        {
            Institute obj = GetUser();
            if (prevBtn != null)
            {
                model.InstituteName = obj.InstituteName;
                model.FirstName = obj.FirstName;
                model.MiddleName = obj.MiddleName;
                model.LastName = obj.LastName;
                model.Email = obj.Email;
                model.Mobile = obj.Mobile;
                model.GeneratedEmailOTP = obj.EmailOTP;
                model.GeneratedMobileOTP = obj.SMSOtp;
                return View("PersonalDetails", model);
            }
            if (nextBtn != null)
            {
                if (ModelState.IsValid)
                {

                    if (obj.EmailOTP == model.EmailOTP && obj.SMSOtp == model.SMSOTP)
                    {
                        obj.EmailOTP = model.EmailOTP;
                        obj.SMSOtp = model.SMSOTP;
                        model.InstituteName = obj.InstituteName;
                        model.FirstName = obj.FirstName;
                        model.MiddleName = obj.MiddleName;
                        model.LastName = obj.LastName;
                        model.Email = obj.Email;
                        model.Mobile = obj.Mobile;
                        TempData["Review"] = model;
                        
                       SweetAlert("Done", "Email amd Mobile number Verified successfully.!", NotificationType.success);
                        return RedirectToAction("PreviewAndSubmit");
                    }
                    else
                    {
                       
                        SweetAlert("Error", "Email and mobile number verification failed for " + obj.Email + " And " + obj.Mobile, NotificationType.error);
                        return View("OTPVerification");
                    }

                }
            }
            return View();
        }

        public ActionResult PreviewAndSubmit(string prevBtn, string nextBtn)
        {
            Institute obj = GetUser();
            InstituteRegistration model = TempData["Review"] as InstituteRegistration;
            if (nextBtn != null)
            {
                if (ModelState.IsValid)
                {
                    obj.EmailVerified = true;
                    obj.CenterType = "Parent";
                    obj.CenterTypeId = null;
                    obj.RegistrationDate = DateTime.Now;
                    obj.IsActive = true;
                    obj.IsApproved = true;
                    obj.IsDeleted = false;
                    obj.MobileVerified = true;
                    obj.IsPayment = false;
                    obj.Status = "Trial";
                    string strNewPassword = GeneratePassword().ToString();
                    obj.Password = strNewPassword;
                    if (InstituteBL.Add(obj) == true)
                    {
                        User userObj = new User();
                        userObj.InstituteId = obj.Id;
                        userObj.IsActive = true;
                        userObj.Password = obj.Password;
                        userObj.RoleId = 1;
                        userObj.Username = obj.Email;
                        userObj.FirstName = obj.FirstName;
                        userObj.MiddleName = obj.MiddleName;
                        userObj.LastName = obj.LastName;
                        userObj.MobileNo = obj.Mobile;
                        userObj.IsDeleted = false;
                        userObj.DesignationId = 2;
                       

                        UsersBL.Add(userObj);
                        string smsText = "Sir/Madam," + Environment.NewLine + "Thank you for Registration! For Channel Partner Login Details Visit Your Email id." + Environment.NewLine + Environment.NewLine + "Regards," + Environment.NewLine + "ASN Technosoft" + Environment.NewLine + "7083528282";
                        SMSBL.SendSMS(obj.Mobile, smsText);
                        EmailBL.SendRegistrationEmail(userObj);
                        RemoveUser();
                        return RedirectToAction("Success");
                    }


                }
            }

            if (prevBtn != null)
            {
                InstituteRegistration Viewmodel = new InstituteRegistration();
                Viewmodel.EmailOTP = obj.EmailOTP;
                Viewmodel.SMSOTP = obj.SMSOtp;
                return View("OTPVerification", Viewmodel);
            }
            else
            {
                return View(model);
            }



        }


        public ActionResult Success()
        {
            return View("Success");
        }


        //        public ActionResult PreviewAndSubmit(string prevBtn, string nextBtn)
        //        {
        //            Institute obj = GetUser();
        //            InstituteRegistration model = TempData["Review"] as InstituteRegistration;
        //            if (nextBtn != null)
        //            {
        //                if (ModelState.IsValid)
        //                {
        //                    obj.EmailVerified = true;
        //                    obj.CenterType = "Parent";
        //                    obj.CenterTypeId = null;
        //                    obj.RegistrationDate = DateTime.Now;
        //                    obj.IsActive = true;
        //                    obj.IsApproved = true;
        //                    //obj.IsActive = false;
        //                    //obj.IsApproved = false;
        //                    obj.IsPayment = false;
        //                    obj.IsDeleted = false;
        //                    obj.MobileVerified = true;
        //                    string strNewPassword = GeneratePassword().ToString();
        //                    obj.Password = strNewPassword;
        //                    if (InstituteBL.Add(obj) == true)
        //                    {
        //                        User userObj = new User();
        //                        userObj.InstituteId = obj.Id;
        //                        userObj.IsActive = true;
        //                        //userObj.IsActive = false;
        //                        userObj.Password = obj.Password;
        //                        userObj.RoleId = 1;
        //                        userObj.Username = obj.Email;
        //                        userObj.FirstName = obj.FirstName;
        //                        userObj.MiddleName = obj.MiddleName;
        //                        userObj.LastName = obj.LastName;
        //                        userObj.MobileNo = obj.Mobile;
        //                        userObj.IsDeleted = false;
        //                        userObj.DesignationId = 2;
        //                        UsersBL.Add(userObj);


        //                        string smsText = "Thank you for Registration!" + Environment.NewLine + "For Login Details visit your Email id." + Environment.NewLine + "Regards," + Environment.NewLine + "ASN Technosoft" + Environment.NewLine + "7083528282";
        //                        SMSBL.SendSMS(obj.Mobile, smsText);
        //                        EmailBL.SendRegistrationEmail(userObj);

        //                        EWallet ewalletObj = EWalletBL.GetByInstituteId(obj.Id);
        //                        int walletId = 0;
        //                        if (ewalletObj == null)
        //                        {
        //                            EWallet newwallet = new EWallet();
        //                            newwallet.AvailableAmount = 0;
        //                            newwallet.InstituteId = obj.Id;
        //                            newwallet.IsActive = false;
        //                            EWalletBL.Add(newwallet);
        //                            walletId = newwallet.Id;
        //                        }
        //                        else
        //                        {
        //                            // ewalletObj.AvailableAmount += Convert.ToDecimal(_requestData.amount);
        //                            // EWalletBL.Edit(ewalletObj);
        //                            walletId = ewalletObj.Id;
        //                        }
        //                        EWalletTransation transaction = new EWalletTransation();
        //                        transaction.AdminRemark = "Online Payment by Paytm Gateway";
        //                        transaction.Amount = 5000;
        //                        transaction.EWalletId = walletId;
        //                        transaction.IsApproved = false;
        //                        transaction.IsDeleted = false;
        //                        transaction.PaymentType = "Online";
        //                        transaction.TransactionDate = DateTime.Now;
        //                        transaction.TransactionType = "Credit";
        //                        EWalletTransactionBL.Add(transaction);
        //                        // Generate random receipt number for order

        //                        string transactionId = transaction.Id.ToString();

        //                        Dictionary<string, string> parameters = new Dictionary<string, string>();
        //                        parameters.Add("MID", Key.merchantId);
        //                        parameters.Add("CHANNEL_ID", "WEB");
        //                        parameters.Add("INDUSTRY_TYPE_ID", "Retail");
        //                        parameters.Add("WEBSITE", "WEBSTAGING");
        //                        parameters.Add("EMAIL", obj.Email);
        //                        parameters.Add("MOBILE_NO", obj.Mobile);
        //                        parameters.Add("CUST_ID", obj.Id.ToString());
        //                        parameters.Add("ORDER_ID", "ORDER" + transactionId);
        //                        parameters.Add("TXN_AMOUNT", "5000");

        //                        parameters.Add("CALLBACK_URL", "http://ims.asntechnosoft.com/Account/PaytmResponse");

        //                        //   parameters.Add("CALLBACK_URL", "https://localhost:44301/Account/PaytmResponse");

        //                        string checksum = CheckSum.GenerateCheckSum(Key.merchantKey, parameters);
        //                        //   string paytmURL = "https://securegw-stage.paytm.in/theia/processTransaction?orderid=" + parameters.FirstOrDefault(x => x.Key == "ORDER_ID").Value;

        //                        string paytmURL = "https://securegw.paytm.in/theia/processTransaction?orderid=" + parameters.FirstOrDefault(x => x.Key == "ORDER_ID").Value;
        //                        string outputHTML = "<html>";
        //                        outputHTML += "<head>";
        //                        outputHTML += "<title>Merchant Check Out Page</title>";
        //                        outputHTML += "</head>";
        //                        outputHTML += "<body>";
        //                        outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
        //                        outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
        //                        outputHTML += "<table border='1'>";
        //                        outputHTML += "<tbody>";

        //                        foreach (string key in parameters.Keys)
        //                        {
        //                            outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
        //                        }

        //                        outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
        //                        outputHTML += "</tbody>";
        //                        outputHTML += "</table>";
        //                        outputHTML += "<script type='text/javascript'>";
        //                        outputHTML += "document.f1.submit();";
        //                        outputHTML += "</script>";
        //                        outputHTML += "</form>";
        //                        outputHTML += "</body>";
        //                        outputHTML += "</html>";

        //                        ViewBag.htmlData = outputHTML;

        //                        // return View("PaytmPaymentPage");











        //                    }



        //                    // RemoveUser();
        //                    //   return RedirectToAction("Success");
        //                    return View("PaytmPaymentPage");
        //                }
        //            }

        //            if (prevBtn != null)
        //            {
        //                InstituteRegistration Viewmodel = new InstituteRegistration();
        //                Viewmodel.EmailOTP = obj.EmailOTP;
        //                Viewmodel.SMSOTP = obj.SMSOtp;
        //                return View("OTPVerification", Viewmodel);
        //            }
        //            //else
        //            //{
        //            //    return View(model);
        //            //}

        //            return View(model);

        //        }





        [HttpPost]
        public ActionResult ExistOrNot(string Email)
        {
            Institute data = InstituteBL.GetByEmailId(Email);
            return Json(data.Id, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult MobileExistOrNot(string Mobile)
        {
            Institute data = InstituteBL.GetByMobileNumber(Mobile);
            if(!string.IsNullOrEmpty(data.Id.ToString()))
            {
                return Json(data.Id, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
           
        }



        //        [HttpPost]
        //        public ActionResult PaytmResponse(PaytmResponse response)
        //        {
        //            string value = response.ORDERID;
        //            int startIndex = 5;
        //            string substring = value.Substring(startIndex, value.Length - 5);
        //            Console.WriteLine(substring);
        //            EWalletTransation walltxn = EWalletTransactionBL.GetById(Convert.ToInt32(substring));
        //            EWallet wallet = EWalletBL.GetById(walltxn.EWalletId);
        //            Institute instObj = InstituteBL.GetById(wallet.InstituteId);
        //            PaytmResponse paytmResponse = new PaytmResponse();
        //            paytmResponse.ORDERID = response.ORDERID;
        //            paytmResponse.BANKNAME = response.BANKNAME;
        //            paytmResponse.BANKTXNID = response.BANKTXNID;
        //            paytmResponse.CHECKSUMHASH = response.CHECKSUMHASH;
        //            paytmResponse.CURRENCY = response.CURRENCY;
        //            paytmResponse.GATEWAYNAME = response.GATEWAYNAME;
        //            paytmResponse.InstituteId = wallet.InstituteId;
        //            paytmResponse.InstituteName = instObj.InstituteName;
        //            paytmResponse.MID = response.MID;
        //            paytmResponse.PAYMENTMODE = response.PAYMENTMODE;
        //            paytmResponse.RESPCODE = response.RESPCODE;
        //            paytmResponse.RESPMSG = response.RESPMSG;
        //            paytmResponse.STATUS = response.STATUS;
        //            paytmResponse.TXNAMOUNT = response.TXNAMOUNT;
        //            paytmResponse.TXNDATE = response.TXNDATE;
        //            paytmResponse.TXNID = response.TXNID;


        //            //Save Data in Database
        //            PaytmPaymentResponse dbResponse = new PaytmPaymentResponse();
        //            dbResponse.ORDERID = response.ORDERID;
        //            dbResponse.BANKNAME = response.BANKNAME;
        //            dbResponse.BANKTXNID = response.BANKTXNID;
        //            dbResponse.CHECKSUMHASH = response.CHECKSUMHASH;
        //            dbResponse.CURRENCY = response.CURRENCY;
        //            dbResponse.GATEWAYNAME = response.GATEWAYNAME;
        //            dbResponse.EWalletTransactionId = walltxn.Id;
        //            dbResponse.MID = response.MID;
        //            dbResponse.PAYMENTMODE = response.PAYMENTMODE;
        //            dbResponse.RESPCODE = response.RESPCODE;
        //            dbResponse.RESPMSG = response.RESPMSG;
        //            dbResponse.STATUS = response.STATUS;
        //            dbResponse.TXNAMOUNT = response.TXNAMOUNT;
        //            dbResponse.TXNDATE = response.TXNDATE;
        //            dbResponse.TXNID = response.TXNID;

        //            PaytmPaymentResponse paytmPaymentResponse = PaytmPaymentResponseBL.GetById(walltxn.Id);
        //            if (paytmPaymentResponse == null)
        //            {
        //                PaytmPaymentResponseBL.Add(dbResponse);
        //                if (response.RESPCODE == "01")
        //                {
        //                    if (wallet.Id != 0)
        //                    {
        //                        // wallet.AvailableAmount += Convert.ToDecimal(response.TXNAMOUNT);
        //                        wallet.IsActive = true;
        //                        EWalletBL.Edit(wallet);
        //                    }
        //                    if (walltxn.Id != 0)
        //                    {
        //                        walltxn.Amount = Convert.ToDecimal(response.TXNAMOUNT);
        //                        walltxn.EWalletId = wallet.Id;
        //                        walltxn.IsApproved = true;
        //                        walltxn.IsDeleted = false;
        //                        walltxn.TransactionDate = DateTime.Now;
        //                        EWalletTransactionBL.Edit(walltxn);
        //                    }



        //                    PurchasedSerivce obj = new PurchasedSerivce();
        //                    obj.Amount = Convert.ToDecimal(5000);
        //                    obj.ApproveDate = DateTime.Now;

        //                    obj.InstituteId = wallet.InstituteId;
        //                    obj.IsApprove = true;
        //                    obj.IsDeleted = false;
        //                    obj.PurchaseDate = DateTime.Now;
        //                    obj.Quantity = 1;
        //                    obj.Remark = "IMS Service Subscribed";
        //                    obj.RenewalDate = DateTime.Today.AddYears(1);
        //                    obj.SubscriptionServiceId = 1;

        //                    if (PurchasedServicesBL.Add(obj) == true)
        //                    {
        //                        //  EWallet wallet = EWalletBL.GetByInstituteId(Usermodel.InstituteId);
        //                        if (wallet != null)
        //                        {
        //                            //wallet.AvailableAmount -= obj.Amount;
        //                            //EWalletBL.Edit(wallet);

        //                            EWalletTransation txn = new EWalletTransation();
        //                            txn.AdminRemark = "IMS Service Subscription.";
        //                            txn.Amount = obj.Amount;
        //                            txn.EWalletId = wallet.Id;
        //                            txn.IsApproved = true;
        //                            txn.IsDeleted = false;
        //                            txn.PaymentType = "Online";
        //                            txn.TransactionDate = DateTime.Now;
        //                            txn.TransactionType = "Debit";
        //                            EWalletTransactionBL.Add(txn);
        //                            //  Institute instituteObj = InstituteBL.GetById(Usermodel.InstituteId);


        //                            instObj.IsPayment = true;
        //                            InstituteBL.Edit(instObj);



        //                            // string smsText = "Thank you for Registration!" + Environment.NewLine + "For Login Details visit your Email id." + Environment.NewLine + "Regards," + Environment.NewLine + "ASN Technosoft" + Environment.NewLine + "7083528282";
        //                            //  SMSBL.SendSMS(instObj.Mobile, smsText);
        //                            // User userObj = UsersBL.GetByUserName(instObj.Email,instObj.Id);
        //                            //userObj.IsActive = true;
        //                            //UsersBL.Edit(userObj);
        //                            //  EmailBL.SendRegistrationEmail(userObj);



        //                            //string smsText = "Dear " + instObj.InstituteName + Environment.NewLine + "Thank you, " + SubscriptionServicesBL.GetById(1).ServiceName + " EOI Successfully Submitted!" + Environment.NewLine + "ASN Technosoft" + Environment.NewLine + "7083528282";
        //                            //SMSBL.SendSMS(instObj.Mobile, smsText);
        //                        }
        //                        // SweetAlert("Done", "Service added Successfully", NotificationType.success);
        //                        // return Json(obj, JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //            }
        //            return View("paytmResponse", paytmResponse);
        //        }

        //        public ActionResult Success()
        //        {
        //            return View("Success");
        //        }
        [HttpGet]
        public ActionResult login(string returnUrl)
        {
            UserLogin userModel = new UserLogin();
            try
            {
                // Verification.    
                if (this.Request.IsAuthenticated)
                {
                    // Info.    
                    return RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info    
                Console.Write(ex);
            }
            return this.View(userModel);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserLogin UserModel)
        {
            try
            {
                bool isAuthenticated = false;

                //if(!ModelState.IsValid)
                //{
                //    SweetAlert("Error", "Authentication errror.", NotificationType.error);
                //    return View(UserModel);
                //}
                // Logger.Debug("Log before going to database");

                #region DB Check and Authentication Cookie
                User user = UsersBL.GetByUserNamePassword(UserModel.EmailId, UserModel.Password);
                //  Logger.Error("After database genarated log");
                Institute instituteObj = InstituteBL.GetById(user.InstituteId);
                if(user != null)
                {
                    if (user.Password == UserModel.Password)
                    {
                        /// Check status of user if it is trial based 11/03/2022
                      

                        if (user.IsActive == false)
                        {
                            //Access error
                            isAuthenticated = false;
                            // ModelState.AddModelError("", "Inactive User.Invalid login attempt.");
                            // TempData["message"] = "Error Occured. User status is not active. Please contact administrator.";
                            SweetAlert("Info", "Error Occured. User status is not active. Please contact administrator.", NotificationType.warning);
                            return View(UserModel);
                        }
                        else
                        {
                            isAuthenticated = true;
                            #region Success   : set authentication cookie
                            FormsAuthentication.SetAuthCookie(UserModel.EmailId, false);
                            var authTicket = new FormsAuthenticationTicket(1, UserModel.EmailId, DateTime.Now, DateTime.Now.AddMinutes(720), false, user.RoleId.ToString());
                            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                            HttpContext.Response.Cookies.Add(authCookie);


                            TempData["RoleId"] = user.RoleId;
                            TempData["UserObject"] = user;
                            Session["UserId"] = user.Id;

                            #endregion
                        }
                    }
                    else
                    {
                        //Access error
                        isAuthenticated = false;
                        //ModelState.AddModelError("", "Invalid login attempt.");
                        //TempData["message"] = "Invalid login attempt.";

                        SweetAlert("Error", "Invalid login attempt.", NotificationType.error);
                        return View(UserModel);
                    }

                }
                else
                {
                    isAuthenticated = false;
                    ModelState.AddModelError("", "Invalid login attempt.");
                }

                if (isAuthenticated)
                {
                    switch (RoleBL.GetById(user.RoleId).Role1)
                    {
                        case "InstituteAdmin":
                            {
                                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                                break;
                            }
                        case "ASNAdmin":
                            {
                                return RedirectToAction("Index", "Dashboard", new { area = "ASNAdmin" });
                                break;
                            }
                        case "Student":
                            {
                                return RedirectToAction("Index", "Home", new { area = "Student" });
                                break;
                            }
                        case "Faculty":
                            {
                                return RedirectToAction("Index", "Home", new { area = "InstituteAdmin" });
                                break;
                            }
                        default:
                            {
                                ModelState.AddModelError("", "Invalid login attempt.");
                                TempData["message"] = "Invalid login attempt.";
                                return View(UserModel);
                                break;
                            }
                    }

                }
                else
                {
                    // ModelState.AddModelError("", "Invalid login attempt.");
                    //TempData["message"] = "Invalid username or password.";
                    SweetAlert("Error", "Invalid username or password.", NotificationType.error);
                    return View(UserModel);
                }
                #endregion

            }
            catch (Exception ex)
            {
                //// Info    
                Console.Write(ex.InnerException.Message + ex.StackTrace);
              //  Logger.Error(ex, "Hi I am NLog Error Level");


            }

            return View(UserModel);
        }



        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account");
        }
        
        

       
        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                // Verification.    
                if (Url.IsLocalUrl(returnUrl))
                {
                    // Info.    
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
            // Info.    
            return this.RedirectToAction("Index", "Account");
        }




        //        [HttpGet]
        //        public ActionResult ForgotPassword()
        //        {
        //            return View("ForgotPassword");
        //        }
        //        [HttpPost]
        //        public ActionResult ForgotPassword(UserLogin data)
        //        {
        //            User userobj = UsersBL.GetByEmail(data.EmailId);
        //            if (userobj != null)
        //            {
        //                Institute obj = InstituteBL.GetById(Convert.ToInt32(userobj.InstituteId));
        //                string newPassword = GeneratePassword().ToString();
        //                if (userobj.Username == obj.Email)
        //                {

        //                    obj.Password = newPassword;
        //                    if (InstituteBL.Edit(obj) == true)
        //                    {

        //                        userobj.Password = newPassword;
        //                        UsersBL.Edit(userobj);
        //                        EmailBL.SendResetPasswordMail(userobj);
        //                        SweetAlert("Done", "Password Reset Successfully! Please visit your mail id for login details", NotificationType.success);
        //                        return
        //                            RedirectToAction("Login", "Account", new { area = "" });//RedirectToAction("ChangePassword", "Institute");
        //                    }
        //                }
        //                else
        //                {

        //                    userobj.Password = newPassword;
        //                    UsersBL.Edit(userobj);
        //                    EmailBL.SendResetPasswordMail(userobj);
        //                    SweetAlert("Done", "Password changed Successfully", NotificationType.success);
        //                    return
        //                        RedirectToAction("Login", "Account", new { area = "" });//RedirectToAction("ChangePassword", "Institute");
        //                }

        //            }
        //            else
        //            {
        //                SweetAlert("Error", "Invalid email id.!", NotificationType.error);
        //                return View();
        //            }




        //            return View();
        //        }




    }
}


