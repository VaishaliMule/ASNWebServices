
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnosoftModel;

namespace BussinessLayer
{
    public class SubscriptionServicesBL
    {
        public static bool Add(SubscriptionService obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.SubscriptionServices.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(SubscriptionService obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.SubscriptionServices.Attach(obj);
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

        public static bool Delete(SubscriptionService obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.SubscriptionServices.Attach(obj);
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
        public static List<SubscriptionService> GetAll()
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.SubscriptionServices where obj.IsDeleted == false select obj).ToList();
            }
        }
        public static SubscriptionService GetById(int serviceId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.SubscriptionServices where obj.Id == serviceId && obj.IsDeleted == false select obj).FirstOrDefault();
            }
        }
        public static SubscriptionService GetByServiceName(string servicename)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.SubscriptionServices where obj.ServiceName == servicename && obj.IsDeleted == false select obj).FirstOrDefault();
            }
        }
    }
}
