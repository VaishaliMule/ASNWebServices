using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class FeesFollowupHistoryBL
    {
        public static bool Add(FeesFollowupHistory obj)
        {
            try
            {
                using(TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.FeesFollowupHistories.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(FeesFollowupHistory obj)
        {
            try
            {
                using(TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.FeesFollowupHistories.Attach(obj);
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

        public static List<FeesFollowupHistory> GetAll()
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.FeesFollowupHistories select obj).ToList();
            }
        }
        public static FeesFollowupHistory GetById(int FeeFollowupHistoryId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.FeesFollowupHistories where obj.Id == FeeFollowupHistoryId select obj).FirstOrDefault();
            }
        }
        public static List<FeesFollowupReportByBatch_Result> GetFeesFollowupListByInstituteId_batchid( int courseid,int batchid, int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetFeesFollowupReportByBatch(courseid,batchid,instituteid)).ToList();
            }
        }
        public static List<StudentFeesFollowupBy_Admissionid_Result> GetFeesFollowupListByInstituteId_Admissionid(int admissionid, int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetStudentFeesFollowupBy_Admissionid(admissionid, instituteid)).ToList();
            }
        }

        public static List<AllPendingFeesForFollowup_Result> GetAllPendingFees(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllPendingFeesForFollowup( instituteid)).ToList();
            }
        }
        public static List<AllOverdueFeesForFollowup_Result> GetAllOverdueFees(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllOverdueFeesForFollowup(instituteid)).ToList();
            }
        }
        public static List<AllTodaysFeesForFollowup_Result> GetAllTodaysFollowupFees( int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllTodaysFeesForFollowup(instituteid)).ToList();
            }
        }
    }
}
