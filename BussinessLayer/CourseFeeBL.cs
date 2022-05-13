using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class CourseFeeBL
    {
        public static bool Add(CourseFee obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.CourseFees.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(CourseFee obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.CourseFees.Attach(obj);
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

        public static List<CourseFee> GetInstallmentByCourseId(int courseid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.CourseFees where obj.CourseId == courseid select obj).ToList();
            }
        }
        public static CourseFee GetById(int CourseFeeId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.CourseFees where obj.Id == CourseFeeId select obj).FirstOrDefault();
            }
        }
        public static List<CourseFee> GetAllFeeByCoursei()
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.CourseFees select obj).ToList();
            }
        }

        public static CourseFee GetByCourse_Installment(int CourseId, int installmentmode)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.CourseFees where obj.CourseId == CourseId && obj.NoOfInstallments==installmentmode select obj).FirstOrDefault();
            }
        }

        public static List<CourseFee> GetAll()
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.CourseFees select obj).ToList();
            }
        }

        public static List<AllCourseFeesByInstituteId_Result> GetAllCourseFessByInstitutes(int instituteid,int courseid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllCourseFeesByInstituteId(instituteid,courseid)).ToList();
            }
        }

        public static List<CourseFeeBy_Courseid_InstallmentMode_Result> GetCourseFeeBy_CourseId_InstallmentMode(int InstituteId,int CourseId, int installmentmode)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetCourseFeeBy_Courseid_InstallmentMode(InstituteId, CourseId,installmentmode)).ToList();
            }
        }
    }
}
