using IMS.Models;
using BussinessLayer;
using TechnosoftModel;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.Controllers
{
    public class PaymentController : Controller
    {
        // GET: InstituteAdmin/Payment
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
        public ActionResult CreateOrder(PaymentInitiateModel _requestData)
        {
            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            EWallet ewalletObj = EWalletBL.GetByInstituteId(Usermodel.InstituteId);
            int walletId=0;
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
            transaction.AdminRemark = "Online Payment by razorpay";
            transaction.Amount = _requestData.amount;
            transaction.EWalletId = walletId;
            transaction.IsApproved = false;
            transaction.IsDeleted = false;
            transaction.PaymentType = "Online";
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = "Credit";
            EWalletTransactionBL.Add(transaction);
            // Generate random receipt number for order
       
            string transactionId = transaction.Id.ToString();
            RazorpayClient client = new RazorpayClient("rzp_test_r6dutiobsnB7SA", "FZbODT1LCwkDCd8bxrbpBWwL");
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", _requestData.amount * 100);  // Amount will in paise
            options.Add("receipt", transactionId);
            options.Add("currency", "INR");
            options.Add("payment_capture", 1);

            Razorpay.Api.Order orderResponse = client.Order.Create(options);
            string orderId = orderResponse["id"].ToString();
            // Create order model for return on view
            OrderModel orderModel = new OrderModel
            {
                orderId = orderResponse.Attributes["id"],
                razorpayKey = "rzp_test_r6dutiobsnB7SA",
                amount = _requestData.amount * 100,
                currency = "INR",
                name = _requestData.name,
                email = _requestData.email,
                contactNumber = _requestData.contactNumber,
                description = "Purchase description",
                InstituteId = _requestData.InstituteId,
                TransactionId = Convert.ToInt32(transactionId)
            };
          
            return View("PaymentPage", orderModel);
        }
        public class OrderModel
        {
            public string orderId { get; set; }
            public string razorpayKey { get; set; }
            public int amount { get; set; }
            public string currency { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string contactNumber { get; set; }
            public string description { get; set; }
            public int InstituteId { get; set; }
            public int TransactionId { get; set; }
        }
        [HttpPost]
        public ActionResult Save(OrderModel orderdata)
        {

            User Usermodel = TempData["UserObject"] as User;
            TempData.Keep();
            
          
         
            // Payment data comes in url so we have to get it from url
            // This id is razorpay unique payment id which can be use to get the payment details from razorpay server
            string paymentId = Request.Params["rzp_paymentid"];
            string amount = orderdata.amount.ToString() ; 
            // This is orderId
            string orderId = Request.Params["rzp_orderid"];
            RazorpayClient client = new RazorpayClient("rzp_test_r6dutiobsnB7SA", "FZbODT1LCwkDCd8bxrbpBWwL");
            Payment payment = client.Payment.Fetch(paymentId);
            // This code is for capture the payment 
            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", amount);//payment.Attributes["amount"]
            input.Add("currency", "INR");
            Payment paymentCaptured = payment.Capture(input);
            string amt = paymentCaptured.Attributes["amount"];


            EWallet ewalletObj = EWalletBL.GetByInstituteId(Usermodel.InstituteId);

            if (ewalletObj.Id != 0)
            {
                ewalletObj.AvailableAmount += Convert.ToDecimal(amt);
                ewalletObj.IsActive = true;
                EWalletBL.Edit(ewalletObj);
            }
            if (orderdata.TransactionId != 0)
            {
                EWalletTransation transaction = EWalletTransactionBL.GetById(orderdata.TransactionId);
                transaction.Amount = Convert.ToDecimal(amt);
                transaction.EWalletId = ewalletObj.Id;
                transaction.IsApproved = true;
                transaction.IsDeleted = false;
                transaction.TransactionDate = DateTime.Now;
                EWalletTransactionBL.Edit(transaction);
            }


            RazorPayment razorPayment = new RazorPayment();
            try
            {
                razorPayment.EWalletTransactionId = orderdata.TransactionId;
                razorPayment.RazorId = paymentCaptured.Attributes["id"];
                razorPayment.RazorEntity = paymentCaptured.Attributes["entity"];
                razorPayment.Amount = paymentCaptured.Attributes["amount"];
                razorPayment.Currency = paymentCaptured.Attributes["currency"];
                razorPayment.Status = paymentCaptured.Attributes["status"];
                razorPayment.OrderId = paymentCaptured.Attributes["order_id"];
                razorPayment.Method = paymentCaptured.Attributes["method"];
                razorPayment.Description = paymentCaptured.Attributes["description"];
                razorPayment.CardId = paymentCaptured.Attributes["card_id"];
                razorPayment.Bank = paymentCaptured.Attributes["bank"];
                razorPayment.Wallet = paymentCaptured.Attributes["wallet"];
                razorPayment.Email = paymentCaptured.Attributes["email"];
                razorPayment.Contact = paymentCaptured.Attributes["contact"];
                razorPayment.Fee = paymentCaptured.Attributes["fee"];
                razorPayment.ServiceTax = paymentCaptured.Attributes["service_tax"];
                razorPayment.ErrorCode = paymentCaptured.Attributes["error_code"];
                razorPayment.ErrorDescription = paymentCaptured.Attributes["error_description"];
                razorPayment.CreatedAt = paymentCaptured.Attributes["created_at"];

                RazorPaymentBL.Add(razorPayment);
            }
            catch (Exception ex)
            {
                //TODO:
            }



            if (razorPayment.ErrorCode == null)
            {
                TransactionDetails transactionDetails = new TransactionDetails
                {
                    InstituteId = "ASNIMS" +Usermodel.InstituteId,
                    InstituteName = InstituteBL.GetById(Usermodel.InstituteId).InstituteName,
                    TransactionId = orderdata.TransactionId,
                    GatewayTransactionId = "pay_GsWlzHb5CzD1W1",// paymentCaptured.Attributes["id"],
                    TransactionMethod = "netbanking",// paymentCaptured.Attributes["method"],
                    TransactionDate = DateTime.Now.ToString(),
                    TransactionAmount = "1",//amt
                    Narration = "Purchase description"//paymentCaptured.Attributes["description"],

                };
                //lblMessage.Text = "Your wallet transaction for INR. " + transaction.Amount + "/- is approved by admin. Now your total available balance is INR." + WalletBL.GetById(transaction.WalletId).AvailableAmount.ToString() + "/-";

                return RedirectToAction("Success", transactionDetails);
            }
            else
            {
                //  "Payment Failed. " + razorPayment.ErrorDescription;
                return RedirectToAction("Failed");
            }
        }
        public class TransactionDetails
        {
            public string InstituteId { get; set; }
            public string InstituteName { get; set; }
            public int TransactionId { get; set; }
            public string GatewayTransactionId { get; set; }
            public string TransactionMethod { get; set; }
            public string TransactionDate { get; set; }
            public string TransactionAmount { get; set; }
            public string Narration { get; set; }
        }
        public ActionResult Success(TransactionDetails transactionDetails)
        {
            return View(transactionDetails);
        }
        public ActionResult Failed()
        {
            return View();
        }
    }
}