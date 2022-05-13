
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnosoftModel;

namespace BussinessLayer
{
    public class EWalletBL
    {
        public static bool Add(EWallet obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.EWallets.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(EWallet obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.EWallets.Attach(obj);
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }

        public static List<EWallet> GetAll()
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.EWallets select obj).ToList();
            }
        }
        public static EWallet GetById(int walletId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.EWallets where obj.Id == walletId select obj).FirstOrDefault();
            }
        }


        public static EWallet GetByInstituteId(int instituteId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.EWallets where obj.InstituteId == instituteId select obj).FirstOrDefault();
            }
        }


        public static decimal GetTotalDeposited(int InstituteId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                EWallet wallet = EWalletBL.GetByInstituteId(InstituteId);
                decimal totalDeposited = 0;
                var approvedCredits = (from obj in context.EWalletTransations where obj.TransactionType == "Credit" && obj.IsApproved == true && obj.EWalletId == wallet.Id && obj.IsDeleted==false select obj).ToList();
                foreach (var item in approvedCredits)
                {
                    totalDeposited += item.Amount;
                }
                return totalDeposited;
            }
        }

        public static decimal GetTotalUtilised(int InstituteId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                EWallet wallet = EWalletBL.GetByInstituteId(InstituteId);
                decimal totalUtilized = 0;
                var approvedCredits = (from obj in context.EWalletTransations where obj.TransactionType == "Debit" && obj.EWalletId == wallet.Id && obj.IsDeleted == false select obj).ToList();
                foreach (var item in approvedCredits)
                {
                    totalUtilized += item.Amount;
                }
                return totalUtilized;
            }
        }
        public static decimal GetAvailableAmount(int InstituteId)
        {
            decimal availableAmount = 0;
            EWallet wallet = EWalletBL.GetByInstituteId(InstituteId);
            if (wallet != null)
            {
                availableAmount = wallet.AvailableAmount;
            }
            return availableAmount;
        }

        public static List<EWalletTransation> GetMyTransactions(int InstituteId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                List<EWalletTransation> myTransactions = new List<EWalletTransation>();
                int walletId = EWalletBL.GetByInstituteId(InstituteId).Id;
                myTransactions.AddRange((from obj in context.EWalletTransations where obj.TransactionType == "Credit" && obj.IsApproved==true && obj.EWalletId == walletId && obj.IsDeleted == false select obj).ToList());
                myTransactions.AddRange((from obj in context.EWalletTransations where obj.TransactionType == "Debit" && obj.EWalletId == walletId && obj.IsDeleted == false select obj).ToList());
                return myTransactions.OrderBy(t => t.TransactionDate).ToList();
            }
        }
    }
}
