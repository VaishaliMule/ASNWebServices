using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class BatchBL
    {
        public static bool Add(Batch obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Batches.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(Batch obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Batches.Attach(obj);
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

        public static bool Delete(Batch obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Batches.Attach(obj);
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

        public static List<Batch> GetAllBatchesByCourseid(int courseid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Batches where obj.CourseId == courseid && obj.IsDeleted==false select obj).ToList();
            }
        }
        public static Batch GetById(int BatchId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Batches where obj.Id == BatchId && obj.IsDeleted == false  select obj).FirstOrDefault();
            }
        }
        public static List<Batch> GetByCourseId( int courseid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Batches where obj.CourseId == courseid select obj).ToList();
            }
        }

        public static List<Batch> GetAll()
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Batches where obj.IsDeleted == false select obj).ToList();
            }
        }

        public static List<AllBatchesByInstituteId_Result> GetAllBatchesByInstituteId(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllBatchesByInstituteId(instituteid)).ToList();
            }
        }

        public static BatchesDetails_Batchid_InstituteId_Result GetBatchbyName(int instituteid,string batchname)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetBatchesDetails_Batchid_InstituteId(instituteid, batchname)).FirstOrDefault();
            }
        }

        public static List<BatchByCourseId_InstituteId_Result> GetBatchesofcourseid_instituteid(int courseid,int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetBatchByCourseId_InstituteId(courseid, instituteid)).ToList();
            }
        }
    }
}
