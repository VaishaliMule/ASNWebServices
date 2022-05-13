using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class FeeReceiptBL
    {
        public static bool Add(FeeReceipt obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.FeeReceipts.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(FeeReceipt obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.FeeReceipts.Attach(obj);
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

        public static List<FeeReceipt> GetAllFeeReceiptByStudentid(int studentid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.FeeReceipts where obj.StudentId == studentid select obj).ToList();
            }
        }
        public static FeeReceipt GetById(int FeeReceiptId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.FeeReceipts where obj.Id == FeeReceiptId  select obj).FirstOrDefault();
            }
        }

    }
}
