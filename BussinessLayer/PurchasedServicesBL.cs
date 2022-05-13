
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnosoftModel;

namespace BussinessLayer
{
    public class PurchasedServicesBL
    {
        public static bool Add(PurchasedSerivce obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.PurchasedSerivces.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(PurchasedSerivce obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.PurchasedSerivces.Attach(obj);
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
        public static bool Delete(PurchasedSerivce obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.PurchasedSerivces.Attach(obj);
                    obj.IsDeleted = true;
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

        public static List<PurchasedSerivce> GetAll()
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.PurchasedSerivces where obj.IsApprove==true && obj.IsDeleted==false select obj).ToList();
            }
        }
        public static PurchasedSerivce GetById(int purchasedId,int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.PurchasedSerivces where obj.Id == purchasedId && obj.IsApprove == true && obj.IsDeleted == false && obj.InstituteId==instituteid  select obj).FirstOrDefault();
            }
        }
        public static List<PurchasedSerivce> GetByInstituteId(int instituteId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.PurchasedSerivces where obj.InstituteId == instituteId && obj.IsApprove == true && obj.IsDeleted == false select obj).ToList();
            }
        }

        public static PurchasedSerivce GetBySubscriptionId(int subscriptionServiceId,int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
               ///string date = DateTime.Today.ToShortDateString();
                DateTime dtFrom = DateTime.Today;
                return (from obj in context.PurchasedSerivces where obj.SubscriptionServiceId == subscriptionServiceId && obj.IsApprove == true && obj.IsDeleted == false && obj.InstituteId==instituteid && obj.RenewalDate>dtFrom select obj).FirstOrDefault();
            }
        }

        public static List<AllEOIDetailsByInstituteid_Result> GetAllEOIByInstituteId(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllEOIDetailsByInstituteid(instituteid)).ToList();
            }
        }
    }
}
