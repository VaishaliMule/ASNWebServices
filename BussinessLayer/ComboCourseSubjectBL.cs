using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class ComboCourseSubjectBL
    {

        public static bool Add(ComboCourseSubject obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.ComboCourseSubjects.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(ComboCourseSubject obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.ComboCourseSubjects.Attach(obj);
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
        public static bool Delete(ComboCourseSubject obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.ComboCourseSubjects.Attach(obj);
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


        public static List<AllComboCourses_Result> GetAllComboCoursesByInstitutes(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllComboCourses(instituteid)).ToList();
            }
        }

        public static ComboCourseSubject GetSelectedComboCourse(int instituteid, int combocourseid,int subcourseid)
        {
            using(TECHNOSOFTSERVICESEntities context=new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.ComboCourseSubjects where obj.InstituteId == instituteid && obj.ComboCourseId == combocourseid && obj.SubCourseId==subcourseid && obj.IsDeleted==false select obj).FirstOrDefault();
            }
        }



        public static List<ComboCourseSubject> GetListofComboCourse(int instituteid, int combocourseid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.ComboCourseSubjects where obj.InstituteId == instituteid && obj.ComboCourseId == combocourseid && obj.IsDeleted == false select obj).ToList();
            }
        }




        public static List<SubcoursesOfCombo_Result> GetAllSubCourses(int instituteid, int combocourseId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetSubcoursesOfCombo(instituteid, combocourseId)).ToList();
            }
        }

        public static List<AllActiveBatchesOfSubCourses_Result> GetAllActiveBatchesOfComboCourses(int instituteid, int combocourseId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllActiveBatchesOfSubCourses(instituteid, combocourseId)).ToList();
            }
        }


    }
}
