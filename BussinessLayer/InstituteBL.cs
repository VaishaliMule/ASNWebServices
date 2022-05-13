using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnosoftModel;

namespace BussinessLayer
{
    public class InstituteBL
    {
        public static bool Add(Institute obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Institutes.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                Console.WriteLine(excp.Message);
                return false;
               // throw excp;
            }
        }
        public static bool Edit(Institute obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Institutes.Attach(obj);
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

        public static List<Institute> GetAll()
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Institutes where obj.IsDeleted == false && obj.IsActive==true && obj.IsApproved==true select obj).ToList();
            }
        }
        public static List<Institute> GetAllActiveInstitute()
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Institutes where obj.IsActive==true && obj.IsApproved==true && obj.IsDeleted==false && obj.IsPayment==true select obj).ToList();
            }
        }
        public static List<Institute> GetAllInActiveInstitute()
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Institutes where obj.IsActive == true && obj.IsApproved == true && obj.IsDeleted == false && obj.IsPayment == false select obj).ToList();
            }
        }
        public static Institute GetById(int InstituteId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Institutes where obj.Id == InstituteId select obj).FirstOrDefault();
            }
        }
        public static Institute GetByEmailId(string Email)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Institutes where obj.Email == Email && obj.IsActive==true && obj.IsApproved==true select obj).FirstOrDefault();
            }
        }

        public static Institute GetByMobileNumber(string Mobile)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Institutes where obj.Mobile == Mobile && obj.IsActive == true && obj.IsApproved == true select obj).FirstOrDefault();
            }
        }

        public static List<Institute> GetInastituteByStatus(string status)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Institutes where obj.IsActive == true && obj.IsApproved == true && obj.IsDeleted == false && obj.Status == status select obj).ToList();
            }
        }

        public static List<InstituteDetails_All_Result> GetAllInstitutes()
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (context.GET_InstituteDetails_All()).ToList();
            }
        }

    }
}
