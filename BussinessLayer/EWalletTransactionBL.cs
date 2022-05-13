
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnosoftModel;

namespace BussinessLayer
{
    public class EWalletTransactionBL
    {
        public static bool Add(EWalletTransation obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.EWalletTransations.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(EWalletTransation obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.EWalletTransations.Attach(obj);
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

        public static List<EWalletTransation> GetAllByWalletId(int walletId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.EWalletTransations where obj.EWalletId==walletId select obj).ToList();
            }
        }
        public static EWalletTransation GetById(int walletTxnId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.EWalletTransations where obj.Id == walletTxnId select obj).FirstOrDefault();
            }
        }
        
    }
}
