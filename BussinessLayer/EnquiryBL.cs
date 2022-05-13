using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class EnquiryBL
    {
        public static bool Add(Enquiry obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Enquiries.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(Enquiry obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Enquiries.Attach(obj);
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

        public static List<Enquiry> GetAll(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Enquiries where obj.InstituteId == instituteid select obj).ToList();
            }
        }
        public static Enquiry GetById(int EnquiryId, int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Enquiries where obj.Id == EnquiryId && obj.InstituteId == instituteid select obj).FirstOrDefault();
            }
        }
    
        public static List<AllEnquiriesOfStudentByInstituteId_Result> GetAllEnquiryOfStudentByInstituteId(int instituteid, int studentid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllEnquiriesOfStudentByInstituteId(instituteid, studentid)).ToList();
            }
        }

        public static List<StudentListForEnquiryDetailsByInstituteId_Result> GetStudentEnquiryByInstituteId(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetStudentListForEnquiryDetailsByInstituteId(instituteid)).ToList();
            }
        }
        public static Enquiry GetEnquiriesByCourseId_StudentId(int Courseid,int Studentid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Enquiries where obj.CourseId == Courseid && obj.StudentId==Studentid  select obj).FirstOrDefault();
            }
        }

        public static List<AllPendingEnquiresByInstituteid_Result> GetAllPendingEnquiries(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllPendingEnquiresByInstituteid(instituteid)).ToList();
            }
        }
        public static List<AllOverdueEnquiriesForFollowup_Result> GetAllOverdueEnquiries(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllOverdueEnquiriesForFollowup(instituteid)).ToList();
            }
        }

        public static List<TodaysEnquiryFollowupList_Result> GetAllTodaysFollowupEnquiries(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetTodaysEnquiryFollowupList(instituteid)).ToList();
            }
        }

        public static List<AllEnquiryFollowupReportByInstituteId_Result> GetStudentEnquiryFollowupListByInstituteId(DateTime startdate,DateTime todate,int? courseid, int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetAllEnquiryFollowupReportByInstituteId(startdate,todate,courseid, instituteid)).ToList();
            }
        }

        public static List<FollowupofStudent_enquiryid_instituteid_Result> GetEnquiryFollowupListByInstituteId_enquiryid(int enquiryid, int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GetFollowupofStudent_enquiryid_instituteid(enquiryid, instituteid)).ToList();
            }
        }
    }
}
