using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class StudentBL
    {
        public static bool Add(Student obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Students.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(Student obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Students.Attach(obj);
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

        public static List<Student> GetAll(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Students where obj.InstituteId == instituteid select obj).ToList();
            }
        }
        public static Student GetById(int studentId, int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Students where obj.Id == studentId && obj.InstituteId == instituteid select obj).FirstOrDefault();
            }
        }

        public static List<Student> SearchBy_firstname_lastname(string firstname, string lastname,int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Students where obj.FirstName == firstname && obj.LastName==lastname && obj.InstituteId==instituteid select obj).ToList();
            }
        }

        public static List<TodaysBirthdaylist_Result> GetAllTodaysBirthday(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetTodaysBirthdaylist(instituteid)).ToList();
            }
        }
        public static List<AllStudentByInstituteId_Result> GetAllStudentByInstituteId(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllStudentByInstituteId(instituteid)).ToList();
            }
        }

        public static Student GetBy_firstname_lastname_dob(string firstname, string lastname,DateTime dob, int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Students where obj.FirstName == firstname && obj.LastName == lastname && obj.BirthDate==dob && obj.InstituteId == instituteid select obj).FirstOrDefault();
            }
        }

        public static StudentForEditById_Result GetStudentForEditByStudentId(int instituteId, int studentId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetStudentForEditById(instituteId,studentId)).FirstOrDefault();
            }
        }
    }
}
