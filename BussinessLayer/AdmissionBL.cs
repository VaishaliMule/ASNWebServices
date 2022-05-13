using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class AdmissionBL
    {
        public static bool Add(Admission obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Admissions.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(Admission obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Admissions.Attach(obj);
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

        public static List<Admission> GetAll(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Admissions where obj.InstituteId == instituteid select obj).ToList();
            }
        }
        public static Admission GetById(int AdmissionId, int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Admissions where obj.Id == AdmissionId && obj.InstituteId == instituteid select obj).FirstOrDefault();
            }
        }
        public static Admission GetByName(string Name, int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Admissions where obj.NameOnCertificate == Name && obj.InstituteId == instituteid select obj).FirstOrDefault();
            }
        }

        public static List<StudentListForAdmissionDetailsByInstituteId_Result> GetStudentAdmissionByInstituteId(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetStudentListForAdmissionDetailsByInstituteId(instituteid)).ToList();
            }
        }

        public static List<AllAdmissionOfStudentByInstituteId_Result> GetAllAdmissionOfStudentByInstituteId(int instituteid, int studentid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllAdmissionOfStudentByInstituteId(instituteid,studentid)).ToList();
            }
        }

        public static List<DataForPrintReceipt_studentId_batchId_Result> PrintFeesReceiptOfStudent(int instituteid, int studentid, int batchid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetDataForPrintReceipt_studentId_batchId(instituteid, studentid,batchid)).ToList();
            }
        }

        public static List<StudentFeesReceiptBy_Courseid_batchid_Result> GetFeesReceiptOfStudentByCourseidandBatchid(int courseid, int batchid,int instituteid )
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetStudentFeesReceiptBy_Courseid_batchid(courseid, batchid, instituteid)).ToList();
            }
        }

        public static List<AdmissionCountCoursewisebyInstituteid_Result> GetAllCoursewiseAdmissionCount( int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAdmissionCountCoursewisebyInstituteid(instituteid)).ToList();
            }
        }

        public static Admission GetAdmissionByCourseId_batchId_StudentId(int courseid,int batchid, int studentid, int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Admissions where obj.CourseId == courseid && obj.BatchId==batchid && obj.StudentId==studentid && obj.InstituteId == instituteid select obj).FirstOrDefault();
            }
        }

        public static List<AdmissionReport_FD_TD_Result> GetAdmissionList_FD_TD(DateTime startdate, DateTime todate, int instituteid, int? courseid, int? batchid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAdmissionReport_FD_TD(startdate, todate, instituteid, courseid, batchid)).ToList();
            }
        }
    }
}
