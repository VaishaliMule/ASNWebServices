using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class CourseFeeStructureBL
    {
        public static bool Add(CourseFeeStructure obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.CourseFeeStructures.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(CourseFeeStructure obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.CourseFeeStructures.Attach(obj);
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

        public static List<CourseFeeStructure> GetAll(int coursefeeid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.CourseFeeStructures where obj.CourseFeeId == coursefeeid select obj).ToList();
            }
        }
        public static CourseFeeStructure GetById(int CourseFeeStructureId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.CourseFeeStructures where obj.Id == CourseFeeStructureId select obj).FirstOrDefault();
            }
        }
        public static CourseFeeStructure GetByCourseFeesId_installment(int coursefeeid,int installment)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.CourseFeeStructures where obj.CourseFeeId == coursefeeid && obj.Installment==installment select obj).FirstOrDefault();
            }
        }

    }
}
