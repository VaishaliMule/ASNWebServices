using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class FollowupHistoryBL
    {
        public static bool Add(FollowupHistory obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.FollowupHistories.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(FollowupHistory obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.FollowupHistories.Attach(obj);
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

      
        public static FollowupHistory GetById(int FollowupHistoryId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.FollowupHistories where obj.Id == FollowupHistoryId select obj).FirstOrDefault();
            }
        }
        //public static FollowupHistory GetByName(string Name, int instituteid)
        //{
        //    using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
        //    {
        //        return (from obj in context.FollowupHistories where obj.Name == Name && obj.InstitueId == instituteid select obj).FirstOrDefault();
        //    }
        //}
    }
}
