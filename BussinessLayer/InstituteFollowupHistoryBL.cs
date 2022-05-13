using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnosoftModel;

namespace BussinessLayer
{
    public class InstituteFollowupHistoryBL
    {
        public static bool Add(InstituteFollowupHistory obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.InstituteFollowupHistories.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(InstituteFollowupHistory obj)
        {
            try
            {
                using(TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.InstituteFollowupHistories.Attach(obj);
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

        public static List<InstituteFollowupHistory> GetAll()
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.InstituteFollowupHistories select obj).ToList();
            }
        }
        public static InstituteFollowupHistory GetById(int Id)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.InstituteFollowupHistories where obj.Id == Id select obj).FirstOrDefault();
            }
        }

        public static List<InstituteFollowupHistory> GetByInstituteId(int instituteId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.InstituteFollowupHistories where obj.InstituteId == instituteId select obj).ToList();
            }
        }

    }
}
