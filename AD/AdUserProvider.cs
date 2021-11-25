using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ABAC.Identity.Extensions;
using ABAC.DAL;
using ABAC.Models;
using ABAC.DTO;
using System.DirectoryServices;
using System.Text;
using System.Reflection;
using ABAC.Services;
using ABAC.Extensions;

namespace ABAC.Identity
{
    public interface IUserProvider
    {

        Task<AdUser2> GetAdUser2(string samAccountName, SpuContext spucontext);

        Result ValidateCredentials(string samAccountName, string password, SpuContext spucontext);

        Task<List<AdUser4>> FindUser(SearchDTO model, string[] roles, SpuContext spucontext);

        Result ChangePwdGuestUser(User user, SpuContext spucontext);
        Result CreateUser(AdUser2 model, SpuContext spucontext);

        Result UpdateUser(AdUser2 model, SpuContext spucontext);
        //Result MoveOU(AdUser2 model, SpuContext spucontext);
        Result ChangePwd(AdUser2 model, string pwd, SpuContext spucontext);

        Result DeleteUser(AdUser2 model, SpuContext spucontext);

        //Task<Result> RemoveStaffUser(string samAccountName, SpuContext spucontext);

        Result EnableUser(AdUser2 model, SpuContext spucontext);

        Result DisableUser(AdUser2 model, SpuContext spucontext);

        //Task<Result> CreateOU(string name, SpuContext spucontext);
    }
    public class AdUserProvider : IUserProvider
    {

        public Task<AdUser2> GetAdUser2(string samAccountName, SpuContext spucontext)
        {
            return Task.Run(() =>
            {
                try
                {
                    var setup = spucontext.table_setup.FirstOrDefault();

                    PrincipalContext context = new PrincipalContext(ContextType.Domain, setup.Host, setup.Base, setup.Username, setup.Password);
                    UserPrincipal principal = new UserPrincipal(context);

                    if (context != null)
                    {
                        principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, samAccountName);
                    }

                    if (principal != null)
                        return AdUser2.CastToAdUser(principal);
                    else
                        return null;

                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving AD User", ex);
                }
            });
        }
       
        public Result ValidateCredentials(string samAccountName, string password, SpuContext spucontext)
        {
            try
            {
                var setup = spucontext.table_setup.FirstOrDefault();

                PrincipalContext context = new PrincipalContext(ContextType.Domain, setup.Host, setup.Base, setup.Username, setup.Password);
                var result = context.ValidateCredentials(samAccountName, password);
                return new Result() { result = result };
            }
            catch (Exception ex)
            {
                return new Result() { result = false, Message = ex.Message };
            }

        }
        private List<AdUser4> FindUser(string ou, string role, string text_search, setup setup, SpuContext spucontext)
        {
            var adusers = new List<AdUser4>();
            try
            {
                var oufilter = "ou=" + ou + ",";
                if (!string.IsNullOrEmpty(role))
                    oufilter = "ou=" + role + "," + oufilter;


                var context = new PrincipalContext(ContextType.Domain, setup.Host, oufilter + setup.Base, setup.Username, setup.Password);
                var principal = new UserPrincipal(context);

                var searcher = new PrincipalSearcher(principal);
                var nDS = (DirectorySearcher)searcher.GetUnderlyingSearcher();
                nDS.SearchScope = SearchScope.Subtree;
                var filter = new StringBuilder();
                filter.Append("(& (objectClass=user)(objectCategory=person)");
                if (!string.IsNullOrEmpty(text_search))
                {
                    filter.Append("(| (sAMAccountName=" + text_search + "*) (cn=" + text_search + "*) (sn=" + text_search + "*) (givenName=" + text_search + "*) (mail=" + text_search + "*) (aUStudentId=" + text_search + "*) (aUEmpcode=" + text_search + "*) (aUIDCard=" + text_search + "*) )");
                }
                filter.Append(")");
                nDS.Filter = filter.ToString();

                var src = nDS.FindAll();
                foreach (SearchResult sr in src)
                {
                    PropertyCollection propertyCollection = sr.GetDirectoryEntry().Properties;

                    var aduser = new AdUser4();
                    aduser.sAMAccountName = getPropertyValue(propertyCollection, "sAMAccountName");
                    aduser.userPrincipalName = getPropertyValue(propertyCollection, "userPrincipalName");
                    aduser.displayName = getPropertyValue(propertyCollection, "displayName");
                    aduser.givenName = getPropertyValue(propertyCollection, "givenName");
                    aduser.sn = getPropertyValue(propertyCollection, "sn");
                    aduser.cn = getPropertyValue(propertyCollection, "cn");
                    aduser.distinguishedName = getPropertyValue(propertyCollection, "distinguishedName");
                    aduser.userAccountControl = getPropertyValue(propertyCollection, "userAccountControl");
                    aduser.mail = getPropertyValue(propertyCollection, "mail");
                    aduser.aUEmpcode = getPropertyValue(propertyCollection, "aUEmpcode");
                    aduser.aUEmpType = getPropertyValue(propertyCollection, "aUEmpType");
                    aduser.aUIDCard = getPropertyValue(propertyCollection, "aUIDCard");
                    aduser.aUFactCode = getPropertyValue(propertyCollection, "aUFactCode");
                    aduser.aUFactCode = getPropertyValue(propertyCollection, "aUFactCode");
                    aduser.aUFaculty = getPropertyValue(propertyCollection, "aUFaculty");
                    aduser.aUMetier = getPropertyValue(propertyCollection, "aUMetier");
                    aduser.aUOffice365 = getPropertyValue(propertyCollection, "aUOffice365");
                    aduser.aUPosition = getPropertyValue(propertyCollection, "aUPosition");
                    aduser.aUUserType = getPropertyValue(propertyCollection, "aUUserType");
                    aduser.aUStudentId = getPropertyValue(propertyCollection, "aUStudentId");
                    aduser.aUSex = getPropertyValue(propertyCollection, "aUSex");
                    aduser.department = getPropertyValue(propertyCollection, "department");
                    aduser.departmentNumber = getPropertyValue(propertyCollection, "departmentNumber");

                    adusers.Add(aduser);
                }


            }
            catch
            {

            }
            return adusers;
        }
        public Task<List<AdUser4>> FindUser(SearchDTO model, string[] roles, SpuContext spucontext)
        {
            return Task.Run(() =>
            {
                var setup = spucontext.table_setup.FirstOrDefault();
                var adusers = new List<AdUser4>();

                if (!string.IsNullOrEmpty(model.usertype_search))
                {
                    if (adusers.Count < 100)
                        adusers.AddRange(FindUser(model.usertype_search.toOUName().ToLower(), "", model.text_search, setup, spucontext));
                }
                else
                {
                    if(roles != null)
                    {
                        foreach (var role in roles)
                        {
                            if (adusers.Count < 100)
                                adusers.AddRange(FindUser(model.usertype_search.toOUName().ToLower(), role, model.text_search, setup, spucontext));
                        }
                    }
                    
                }

                return adusers.OrderBy(o => o.givenName).ThenBy(o => o.sn).ToList();
            });
        }
        public Result ChangePwdGuestUser(User user, SpuContext spucontext)
        {

            try
            {
                var setup = spucontext.table_setup.FirstOrDefault();
                PrincipalContext context = new PrincipalContext(ContextType.Domain, setup.Host, "ou=guest," + setup.Base, setup.Username, setup.Password);
                UserPrincipal principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, user.UserName);
                if (principal == null)
                {
                    return new Result() { result = false, Message = "Account has not found" };
                }
                principal.SetPassword(DataEncryptor.Decrypt(user.Password));
                principal.Save();

                return new Result() { result = true };
            }
            catch (Exception ex)
            {
                return new Result() { result = false, Message = ex.Message };
            }

        }


        //public Task<Result> CreateOU(string name, SpuContext spucontext)
        //{
        //    return Task.Run(() =>
        //    {
        //        try
        //        {
        //            var setup = spucontext.table_setup.FirstOrDefault();
        //            var ouname = "ou=guest,";

        //            PrincipalContext context = new PrincipalContext(ContextType.Domain, setup.Host, ouname + setup.Base, setup.Username, setup.Password);

        //            DirectoryEntry objAD = new DirectoryEntry(setup.Base, setup.Username, setup.Password);
        //            DirectoryEntry objOU = objAD.Children.Add("OU=" + name, "OrganizationalUnit");
        //            objOU.CommitChanges();
        //            return new Result() { result = true };
        //        }
        //        catch (Exception ex)
        //        {
        //            return new Result() { result = false, Message = ex.Message };
        //        }
        //    });
        //}

        public Result CreateUser(AdUser2 model, SpuContext spucontext)
        {

            try
            {
                var setup = spucontext.table_setup.FirstOrDefault();
                var oufilter = model.DistinguishedName;

                PrincipalContext context = new PrincipalContext(ContextType.Domain, setup.Host, oufilter, setup.Username, setup.Password);
                UserPrincipal old = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, model.SamAccountName);
                if (old != null)
                {
                    return new Result() { result = false, Message = "Account is duplicated" };
                }

                UserPrincipal principal = new UserPrincipal(context, model.SamAccountName, model.Password, true);
                principal.SamAccountName = model.SamAccountName;
                principal.GivenName = model.GivenName;
                principal.Surname = model.Surname;
                principal.DisplayName = model.DisplayName;
                if (!string.IsNullOrEmpty(model.VoiceTelephoneNumber))
                    principal.VoiceTelephoneNumber = model.VoiceTelephoneNumber;
                else
                    principal.VoiceTelephoneNumber = null;
                principal.EmailAddress = model.EmailAddress;
                principal.UserPrincipalName = model.UserPrincipalName;

                principal.Save();

                DirectoryEntry d = principal.GetUnderlyingObject() as DirectoryEntry;
                if (!string.IsNullOrEmpty(model.aUIDCard))
                    d.Properties["aUIDCard"].Value = model.aUIDCard;
                else
                    d.Properties["aUIDCard"].Value = null;

                if (!string.IsNullOrEmpty(model.aUEmpcode))
                    d.Properties["aUEmpcode"].Value = model.aUEmpcode;
                else
                    d.Properties["aUEmpcode"].Value = null;

                if (!string.IsNullOrEmpty(model.aUStudentId))
                    d.Properties["aUStudentId"].Value = model.aUStudentId;
                else
                    d.Properties["aUStudentId"].Value = null;

                if(!string.IsNullOrEmpty(model.Reference))
                    d.Properties["department"].Value = model.Reference;
                else
                    d.Properties["department"].Value = null;

                if (!string.IsNullOrEmpty(model.PassportID))
                    d.Properties["departmentNumber"].Value = model.PassportID;
                else
                    d.Properties["departmentNumber"].Value = null;

                d.Properties["aUUserType"].Value = model.aUUserType;
                d.Properties["userAccountControl"].Value = userAccountControl.EnablePasswordNotRequired;
                principal.Save();
                return new Result() { result = true };
            }
            catch (Exception ex)
            {
                return new Result() { result = false, Message = ex.Message };
            }

        }
        public Result UpdateUser(AdUser2 model, SpuContext spucontext)
        {

            try
            {
                var setup = spucontext.table_setup.FirstOrDefault();
                var oufilter = getOufromDistinguishedName(model.DistinguishedName);


                PrincipalContext context = new PrincipalContext(ContextType.Domain, setup.Host, oufilter, setup.Username, setup.Password);
                UserPrincipal principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, model.SamAccountName);
                if (principal == null)
                {
                    return new Result() { result = false, Message = "Account has not found" };
                }
                principal.GivenName = model.GivenName;
                principal.Surname = model.Surname;
                principal.DisplayName = model.DisplayName;
                principal.EmailAddress = model.EmailAddress;
                principal.UserPrincipalName = model.UserPrincipalName;

                principal.Save();

                DirectoryEntry d = principal.GetUnderlyingObject() as DirectoryEntry;
                if (!string.IsNullOrEmpty(model.aUIDCard))
                    d.Properties["aUIDCard"].Value = model.aUIDCard;
                else
                    d.Properties["aUIDCard"].Value = null;

                if (!string.IsNullOrEmpty(model.aUEmpcode))
                    d.Properties["aUEmpcode"].Value = model.aUEmpcode;
                else
                    d.Properties["aUEmpcode"].Value = null;

                if (!string.IsNullOrEmpty(model.aUStudentId))
                    d.Properties["aUStudentId"].Value = model.aUStudentId;
                else
                    d.Properties["aUStudentId"].Value = null;

                if (!string.IsNullOrEmpty(model.Reference))
                    d.Properties["department"].Value = model.Reference;
                else
                    d.Properties["department"].Value = null;

                if (!string.IsNullOrEmpty(model.PassportID))
                    d.Properties["departmentNumber"].Value = model.PassportID;
                else
                    d.Properties["departmentNumber"].Value = null;

                principal.Save();

                return new Result() { result = true };
            }
            catch (Exception ex)
            {
                return new Result() { result = false, Message = ex.Message };
            }

        }

        //public Result MoveOU(AdUser2 model, SpuContext spucontext)
        //{

        //    try
        //    {
        //        var setup = spucontext.table_setup.FirstOrDefault();
        //        var oufilter = model.system_ou_lvl1.Replace("o=", "ou=") + ",";
        //        if (!string.IsNullOrEmpty(model.system_ou_lvl2))
        //            oufilter = model.system_ou_lvl2.Replace("o=", "ou=") + "," + oufilter;
        //        if (!string.IsNullOrEmpty(model.system_ou_lvl3))
        //            oufilter = model.system_ou_lvl3.Replace("o=", "ou=") + "," + oufilter;


        //        PrincipalContext context = new PrincipalContext(ContextType.Domain, setup.Host,  setup.Base, setup.Username, setup.Password);
        //        UserPrincipal principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, model.SamAccountName);
        //        if (principal == null)
        //        {
        //            return new Result() { result = false, Message = "Account has not found" };
        //        }


        //        DirectoryEntry de = principal.GetUnderlyingObject() as DirectoryEntry;

        //        DirectoryEntry nde = new DirectoryEntry("LDAP://" + setup.Host +"/" + oufilter + setup.Base, setup.Username, setup.Password, AuthenticationTypes.FastBind);
        //        de.CommitChanges();
        //        de.MoveTo(nde);
        //        de.Close();
        //        nde.Close();
        //        return new Result() { result = true };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result() { result = false, Message = ex.Message };
        //    }

        //}
        public Result DeleteUser(AdUser2 model, SpuContext spucontext)
        {
            try
            {
                var setup = spucontext.table_setup.FirstOrDefault();
                var oufilter = getOufromDistinguishedName(model.DistinguishedName);

                PrincipalContext context = new PrincipalContext(ContextType.Domain, setup.Host, oufilter, setup.Username, setup.Password);
                UserPrincipal principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, model.SamAccountName);
                if (principal == null)
                {
                    return new Result() { result = false, Message = "Account has not found" };
                }
                principal.Delete();
                //principal.Save();

                return new Result() { result = true };
            }
            catch (Exception ex)
            {
                return new Result() { result = false, Message = ex.Message };
            }
        }
        private string getOufromDistinguishedName(string distinguishedName)
        {
            var oufilter = "";
            var splits = distinguishedName.Split(",");
            var i = 0;
            foreach (var split in splits)
            {
                if (i == 0)
                {
                    i++;
                    continue;
                }
                if (i > 1)
                    oufilter += ",";
                oufilter += split;
                i++;
            }
            return oufilter;
        }
        public Result ChangePwd(AdUser2 model, string pwd, SpuContext spucontext)
        {

            try
            {
                var setup = spucontext.table_setup.FirstOrDefault();
                var oufilter = getOufromDistinguishedName(model.DistinguishedName);

                PrincipalContext context = new PrincipalContext(ContextType.Domain, setup.Host, oufilter, setup.Username, setup.Password);
                UserPrincipal principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, model.SamAccountName);
                if (principal == null)
                {
                    return new Result() { result = false, Message = "Account has not found" };
                }
                principal.SetPassword(pwd);
                principal.Save();

                return new Result() { result = true };
            }
            catch (Exception ex)
            {
                return new Result() { result = false, Message = ex.Message };
            }

        }

        public Result EnableUser(AdUser2 model, SpuContext spucontext)
        {

            try
            {
                var setup = spucontext.table_setup.FirstOrDefault();

                PrincipalContext context = new PrincipalContext(ContextType.Domain, setup.Host, setup.Base, setup.Username, setup.Password);
                UserPrincipal principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, model.SamAccountName);
                if (principal == null)
                {
                    return new Result() { result = false, Message = "Account has not found" };
                }

                DirectoryEntry d = principal.GetUnderlyingObject() as DirectoryEntry;
                d.Properties["userAccountControl"].Value = userAccountControl.EnablePasswordNotRequired;
                principal.Save();

                return new Result() { result = true };
            }
            catch (Exception ex)
            {
                return new Result() { result = false, Message = ex.Message };
            }

        }
        public Result DisableUser(AdUser2 model, SpuContext spucontext)
        {
            try
            {
                var setup = spucontext.table_setup.FirstOrDefault();

                PrincipalContext context = new PrincipalContext(ContextType.Domain, setup.Host, setup.Base, setup.Username, setup.Password);
                UserPrincipal principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, model.SamAccountName);
                if (principal == null)
                {
                    return new Result() { result = false, Message = "Account has not found" };
                }

                DirectoryEntry d = principal.GetUnderlyingObject() as DirectoryEntry;
                d.Properties["userAccountControl"].Value = userAccountControl.DisablePasswordNotRequired;
                principal.Save();

                return new Result() { result = true };
            }
            catch (Exception ex)
            {
                return new Result() { result = false, Message = ex.Message };
            }
        }

        //public Task<Result> RemoveStaffUser(string samAccountName, SpuContext spucontext)
        //{
        //    return Task.Run(() =>
        //    {
        //        try
        //        {
        //            var setup = spucontext.table_setup.FirstOrDefault();

        //            PrincipalContext context = new PrincipalContext(ContextType.Domain, setup.Host, "ou=staff," + setup.Base, setup.Username, setup.Password);
        //            UserPrincipal principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, samAccountName);
        //            if (principal == null)
        //            {
        //                return new Result() { result = false, Message = "Account has not found" };
        //            }
        //            principal.Delete();
        //            principal.Save();

        //            return new Result() { result = true };
        //        }
        //        catch (Exception ex)
        //        {
        //            return new Result() { result = false, Message = ex.Message };
        //        }
        //    });
        //}
        private string getPropertyValue(PropertyCollection propertyCollection, string propertyName)
        {
            PropertyValueCollection ValueCollection = propertyCollection[propertyName];
            var value = "";
            for (int i = 0; i < ValueCollection.Count; i++)
            {
                if (i == 0)
                    value = ValueCollection[i].ToString();
                else
                    value = value + "|" + ValueCollection[i].ToString();
            }
            return value;
        }

    }


}