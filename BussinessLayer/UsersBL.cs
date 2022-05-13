
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnosoftModel;

namespace BussinessLayer
{
    public class UsersBL
    {
        public static bool Add(User obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Users.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception excp)
            {
                return false;
            }
        }
        public static bool Edit(User obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Users.Attach(obj);
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

        public static List<User> GetAll(int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Users where obj.InstituteId == instituteid && obj.IsDeleted == false select obj).ToList();
            }
        }
        public static User GetById(int userId, int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Users where obj.Id == userId && obj.InstituteId == instituteid && obj.IsActive == true && obj.IsDeleted == false select obj).FirstOrDefault();
            }
        }
        public static User GetByUserName_instituteid(string userName, int instituteid)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Users where obj.Username == userName && obj.InstituteId == instituteid select obj).FirstOrDefault();
            }
        }

        public static User GetByUserNamePassword(string userName, string password)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                try
                {
                    User user = (from obj in context.Users where obj.Username == userName && obj.Password == password && obj.IsActive == true select obj).FirstOrDefault();
                    return user;
                }
                catch (Exception excp)
                {
                    Console.WriteLine(excp.Message);
                    throw excp;
                }
            }
        }

        public static User GetByUserName(string userName, int instituteId)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Users where obj.Username == userName && obj.InstituteId == instituteId select obj).FirstOrDefault();
            }
        }

        public static bool Delete(User obj)
        {
            try
            {
                using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
                {
                    context.Users.Attach(obj);
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


        public static User GetByEmail(string userName)
        {
            using (TECHNOSOFTSERVICESEntities context = new TECHNOSOFTSERVICESEntities())
            {
                return (from obj in context.Users where obj.Username == userName select obj).FirstOrDefault();
            }
        }
    }
}
