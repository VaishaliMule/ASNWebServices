using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class ShareDesignationBL
    {
        public static bool Add(ShareDesignation obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.ShareDesignations.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(ShareDesignation obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.ShareDesignations.Attach(obj);
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

        public static List<ShareDesignation> GetAll(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.ShareDesignations where obj.InstituteId == instituteid select obj).ToList();
            }
        }
        public static ShareDesignation GetById(int shareDegnId, int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.ShareDesignations where obj.Id == shareDegnId && obj.InstituteId == instituteid select obj).FirstOrDefault();
            }
        }
        
    }
}
