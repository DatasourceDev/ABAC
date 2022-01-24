using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.DirectoryServices;
using ABAC.DTO;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ABAC.Identity
{
    public class AdUser
    {
        public object accountExpires { get; set; }
        public object badPasswordTime { get; set; }
        public int? badPwdCount { get; set; }
        public string cn { get; set; }
        public int? codePage { get; set; }
        public int? countryCode { get; set; }
        public string displayName { get; set; }
        public string distinguishedName { get; set; }
        public Object[] dSCorePropagationData { get; set; }
        public string employeeID { get; set; }
        public string givenName { get; set; }
        public string homeMDB { get; set; }
        public int? instanceType { get; set; }
        public int? internetaccess { get; set; }
        public object lastLogoff { get; set; }
        public object lastLogon { get; set; }
        public object lastLogonTimestamp { get; set; }
        public string legacyExchangeDN { get; set; }
        public object lockoutTime { get; set; }
        public int? logonCount { get; set; }
        public string mail { get; set; }
        public string mailNickname { get; set; }
        public int? mDBOverHardQuotaLimit { get; set; }
        public int? mDBOverQuotaLimit { get; set; }
        public int? mDBStorageQuota { get; set; }
        public bool mDBUseDefaults { get; set; }
        public Object[] memberOf { get; set; }
        public byte[] mS_DS_ConsistencyGuid { get; set; }
        public object msExchArchiveQuota { get; set; }
        public object msExchArchiveWarnQuota { get; set; }
        public int? msExchCalendarLoggingQuota { get; set; }
        public int? msExchDumpsterQuota { get; set; }
        public int? msExchDumpsterWarningQuota { get; set; }
        public string msExchHomeServerName { get; set; }
        public byte[] msExchMailboxGuid { get; set; }
        public object msExchMailboxSecurityDescriptor { get; set; }
        public int? msExchOmaAdminWirelessEnable { get; set; }
        public Object[] msExchPoliciesIncluded { get; set; }
        public object msExchPreviousRecipientTypeDetails { get; set; }
        public string msExchRBACPolicyLink { get; set; }
        public int? msExchRecipientDisplayType { get; set; }
        public int? msExchRecipientSoftDeletedStatus { get; set; }
        public object msExchRecipientTypeDetails { get; set; }
        public Object[] msExchTextMessagingState { get; set; }
        public Object[] msExchUMDtmfMap { get; set; }
        public int? msExchUserAccountControl { get; set; }
        public string msExchUserCulture { get; set; }
        public object msExchVersion { get; set; }
        public DateTime? msExchWhenMailboxCreated { get; set; }
        public string name { get; set; }
        public int? netcastaccess { get; set; }
        public object nTSecurityDescriptor { get; set; }
        public string objectCategory { get; set; }
        public Object[] objectClass { get; set; }
        public byte[] objectGUID { get; set; }
        public byte[] objectSid { get; set; }
        public string pplid { get; set; }
        public int? primaryGroupID { get; set; }
        public Object[] protocolSettings { get; set; }
        public Object[] proxyAddresses { get; set; }
        public object pwdLastSet { get; set; }
        public string sAMAccountName { get; set; }
        public int? sAMAccountType { get; set; }
        public Object[] showInAddressBook { get; set; }
        public string sn { get; set; }
        public string telephoneNumber { get; set; }
        public int? userAccountControl { get; set; }
        public string userPrincipalName { get; set; }
        public object uSNChanged { get; set; }
        public object uSNCreated { get; set; }
        public DateTime? whenChanged { get; set; }
        public DateTime? whenCreated { get; set; }

        private static string getpropertyvalue(PropertyCollection Properties, string PropertyName)
        {
            if (Properties.Contains(PropertyName))
            {
                if (Properties[PropertyName].Value != null)
                    return Properties[PropertyName].Value.ToString();
            }
            return "";
        }

        public static AdUser CastToAdUser(UserPrincipal user)
        {
            DirectoryEntry d = user.GetUnderlyingObject() as DirectoryEntry;

            return new AdUser
            {
                //description = user.Description,
                displayName = user.DisplayName,
                distinguishedName = user.DistinguishedName,
                mail = user.EmailAddress,
                givenName = user.GivenName,
                lastLogon = user.LastLogon,
                name = user.Name,
                sAMAccountName = user.SamAccountName,
                sn = user.Surname,
                userPrincipalName = user.UserPrincipalName,
                telephoneNumber = user.VoiceTelephoneNumber,
                //department = getpropertyvalue(d.Properties, "department"),
            };
        }

    }

    public class AdUser4 : BaseDTO
    {
        public string objectClass { get; set; }
        public string uid { get; set; }
        public string cn { get; set; }
        public string sn { get; set; }
        public string description { get; set; }
        public string givenName { get; set; }
        public string distinguishedName { get; set; }
        public string displayName { get; set; }
        public string department { get; set; }
        public string departmentNumber { get; set; }
        public string name { get; set; }
        public string userAccountControl { get; set; }
        public string codePage { get; set; }
        public string countryCode { get; set; }
        public DateTime? pwdLastSet { get; set; }
        public string primaryGroupID { get; set; }
        public string accountExpires { get; set; }
        public string sAMAccountName { get; set; }
        public string division { get; set; }
        public string sAMAccountType { get; set; }
        public string userPrincipalName { get; set; }
        public string objectCategory { get; set; }
        public string mail { get; set; }

        public string title { get; set; }
        public string postalCode { get; set; }
        public string physicalDeliveryOfficeName { get; set; }
        public string telephoneNumber { get; set; }
        public string company { get; set; }
        public string postOfficeBox { get; set; }
               
        public string comment { get; set; }
        public string lastLogonTimestamp { get; set; }

        public DateTime? lastLogon { get; set; }
        public string logonCount { get; set; }
        
        public string aUEmpcode { get; set; }
        public string aUEmpType { get; set; }
        public string aUIDCard { get; set; }
        public string aUFactCode { get; set; }
        public string aUFaculty { get; set; }
        public string aUMetier { get; set; }
        public string aUOffice365 { get; set; }
        public string aUPosition { get; set; }
        public string aUUserType { get; set; }
        public string aUStudentId { get; set; }
        public string aUSex { get; set; }
        public string aUOtherMail { get; set; }

        private static string getpropertyvalue(PropertyCollection Properties, string PropertyName)
        {
            if (Properties.Contains(PropertyName))
            {
                if (Properties[PropertyName].Value != null)
                    return Properties[PropertyName].Value.ToString();
            }
            return "";
        }

        public static AdUser4 CastToAdUser(UserPrincipal user)
        {
            DirectoryEntry d = user.GetUnderlyingObject() as DirectoryEntry;

            return new AdUser4
            {
                description = user.Description,
                displayName = user.DisplayName,
                distinguishedName = user.DistinguishedName,
                mail = user.EmailAddress,
                givenName = user.GivenName,
                lastLogon = user.LastLogon,
                name = user.Name,
                sAMAccountName = user.SamAccountName,
                sn = user.Surname,
                userPrincipalName = user.UserPrincipalName,
                telephoneNumber = user.VoiceTelephoneNumber,
                department = getpropertyvalue(d.Properties, "department"),
                title = getpropertyvalue(d.Properties, "title"),
                uid = getpropertyvalue(d.Properties, "uid"),
                //jobcode = user.EmployeeId,
            };
        }
    }
    public class AdUser2
    {
        public bool isnew { get; set; }
        public DateTime? AccountExpirationDate { get; set; }
        public DateTime? AccountLockoutTime { get; set; }
        public int BadLogonCount { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string DistinguishedName { get; set; }
        public string Domain { get; set; }

        public string EmailAddress { get; set; }
        public string EmployeeId { get; set; }
        public bool? Enabled { get; set; }

        [Required]
        public string GivenName { get; set; }
        public Guid? Guid { get; set; }
        public string HomeDirectory { get; set; }
        public string HomeDrive { get; set; }
        public DateTime? LastBadPasswordAttempt { get; set; }
        public DateTime? LastLogon { get; set; }
        public DateTime? LastPasswordSet { get; set; }
        public string MiddleName { get; set; }
        public string Name { get; set; }
        public bool PasswordNeverExpires { get; set; }
        public bool PasswordNotRequired { get; set; }
        [Required]
        public string SamAccountName { get; set; }
        public string ScriptPath { get; set; }
        public SecurityIdentifier Sid { get; set; }

        [Required]
        public string Surname { get; set; }
        public bool UserCannotChangePassword { get; set; }
        public string UserPrincipalName { get; set; }
        public string VoiceTelephoneNumber { get; set; }
        public string aUEmpcode { get; set; }
        public string aUEmpType { get; set; }
        public string aUIDCard { get; set; }
        public string aUUserType { get; set; }
        public string aUStudentId { get; set; }
        public string aUOtherMail { get; set; }
        public string comment { get; set; }
        
        public string Reference { get; set; }
        public string ValidDate { get; set; }

        public string ExpireDate{ get; set; }
        public string PassportID { get; set; }
        public string userAccountControl { get; set; }
        public DateTime? accountExpires { get; set; }

        [DataType(DataType.Password)]
        [StringLength(16, ErrorMessage = "รหัสผ่านต้องไม่น้อยกว่า {2} ตัวและไม่เกิน {1} ตัว", MinimumLength = 8)]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "รหัสผ่านจะต้องประกอบด้วยตัวพิมพ์ใหญ่อย่างน้อย 1 ตัว ตัวพิมพ์เล็กอย่างน้อย 1 ตัวและตัวเลขอย่างน้อย 1 ตัว")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [StringLength(16, ErrorMessage = "รหัสผ่านต้องไม่น้อยกว่า {2} ตัวและไม่เกิน {1} ตัว", MinimumLength = 8)]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "รหัสผ่านจะต้องประกอบด้วยตัวพิมพ์ใหญ่อย่างน้อย 1 ตัว ตัวพิมพ์เล็กอย่างน้อย 1 ตัวและตัวเลขอย่างน้อย 1 ตัว")]
        [Compare("Password", ErrorMessage = "รหัสผ่านไม่ตรงกัน")]
        public string ConfirmPassword { get; set; }


        public static AdUser2 CastToAdUser(UserPrincipal user)
        {
            DirectoryEntry d = user.GetUnderlyingObject() as DirectoryEntry;

            return new AdUser2
            {
                AccountExpirationDate = user.AccountExpirationDate,
                AccountLockoutTime = user.AccountLockoutTime,
                BadLogonCount = user.BadLogonCount,
                Description = user.Description,
                DisplayName = user.DisplayName,
                DistinguishedName = user.DistinguishedName,
                EmailAddress = user.EmailAddress,
                EmployeeId = user.EmployeeId,
                Enabled = user.Enabled,
                GivenName = user.GivenName,
                Guid = user.Guid,
                HomeDirectory = user.HomeDirectory,
                HomeDrive = user.HomeDrive,
                LastBadPasswordAttempt = user.LastBadPasswordAttempt,
                LastLogon = user.LastLogon,
                LastPasswordSet = user.LastPasswordSet,
                MiddleName = user.MiddleName,
                Name = user.Name,
                PasswordNeverExpires = user.PasswordNeverExpires,
                PasswordNotRequired = user.PasswordNotRequired,
                SamAccountName = user.SamAccountName,
                ScriptPath = user.ScriptPath,
                Sid = user.Sid,
                Surname = user.Surname,
                UserCannotChangePassword = user.UserCannotChangePassword,
                UserPrincipalName = user.UserPrincipalName,
                VoiceTelephoneNumber = user.VoiceTelephoneNumber,
                accountExpires = user.AccountExpirationDate,
                aUIDCard = getpropertyvalue(d.Properties, "aUIDCard"),
                aUEmpcode = getpropertyvalue(d.Properties, "aUEmpcode"),
                aUEmpType = getpropertyvalue(d.Properties, "aUEmpType"),
                aUStudentId = getpropertyvalue(d.Properties, "aUStudentId"),
                aUUserType = getpropertyvalue(d.Properties, "aUUserType"),
                aUOtherMail = getpropertyvalue(d.Properties, "aUOtherMail"),
                PassportID = getpropertyvalue(d.Properties, "departmentNumber"),
                Reference = getpropertyvalue(d.Properties, "department"),
                userAccountControl = getpropertyvalue(d.Properties, "userAccountControl"),
                comment = getpropertyvalue(d.Properties, "comment"),
                //accountExpires = getpropertydatevalue(d.Properties, "accountExpires"),

            };
        }

        private static string getpropertyvalue(PropertyCollection Properties, string PropertyName)
        {
            if (Properties.Contains(PropertyName))
            {
                if (Properties[PropertyName].Value != null)
                    return Properties[PropertyName].Value.ToString();
            }
            return "";
        }
        private static DateTime? getpropertydatevalue(PropertyCollection Properties, string PropertyName)
        {
            if (Properties.Contains(PropertyName))
            {
                if (Properties[PropertyName].Value != null)
                {
                    try
                    {
                        //var date = Convert.ToDateTime(Properties[PropertyName].Value);
                        var datestr = Properties[PropertyName].Value.ToString();
                        //var value = DateTime.ParseExact(Properties[PropertyName].Value, "ddd MMM dd yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        return null;
                    }
                    catch(Exception ex) 
                    {

                    }
                    
                }
                    //return Properties[PropertyName].Value;
            }
            return null;
        }
        public string GetDomainPrefix() => DistinguishedName
            .Split(',')
            .FirstOrDefault(x => x.ToLower().Contains("dc"))
            .Split('=')
            .LastOrDefault()
            .ToUpper();
    }

}