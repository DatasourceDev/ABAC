using ABAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC
{
    public class AppConst
    {

    }
    public class UserRole
    {
        public static string admin = "admin";
        public static string helpdesk = "HelpDesk";
        public static string approve = "Approve";
        public static string view = "View";
    }
    public enum IDMSource
    {
        Database,
        AD,
        LDAP
    }
    public class aUUserType
    {
        public static string staff = "I";
        public static string student = "s";
        public static string vip = "vip";
        public static string bulk = "bulk";
        public static string office = "office";
        public static string admin = "admin";
    }
    public class roleType
    {
        public static string Admin = "Admin";
        public static string Helpdesk = "Helpdesk";
        public static string WebMaster = "Web Master";
        public static string PasswordOperator = "Password-Operator";
    }
    public enum Status
    {
        Enable = 0,
        Disable = 1,
    }
    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public enum ReturnCode
    {
        Success = 1,
        Error = -1,
    }
    public enum userAccountControl
    {
        Enable = 512,
        Disable = 514,
        EnablePasswordNotRequired = 544,
        DisablePasswordNotRequired = 546,
    }

    public enum RegisterType
    {
        Manual,
        Import
    }
    public enum SendMessageType
    {
        SMS,
        Email
    }
    public enum ImportCreateOption
    {
        student,
        student_sasin,
        student_ppc,
        student_other,
        staff_hr,
        staff_other,
        fixlogin
    }
    public enum ImportLockOption
    {
        pplid,
        loginname
    }
    public enum ImportDeleteOption
    {
        pplid,
        loginname
    }

    public enum ImportType
    {
        lockunlock,
        delete,
        create
    }
    public static class ReturnMessage
    {
        public static string Success = "Your data has been saved successfully.";
        public static string ChangePasswordSuccess = "Password has been changed successfully.";
        public static string Error = "Error! Your data could not be saved.";
        public static string ChangePasswordFail = "Error! Your password could not be changed.";
        public static string SuccessOTP = "OTP has been sent successfully.";
        public static string SuccessEmail = "Email has been sent successfully.";
        public static string DataInUse = "Error! Can not be deleted because data is in use.";
        public static string ImportFail = "Your data has been imported successfully.";
        public static string ImportSuccess = "Error! Your data could not be imported.";
    }
    public static class  Portal
    {
        public static string admin = "admin";
        public static string user = "user";
    }
    public static class LogStatus
    {
        public static string successfully = "successfully";
        public static string failed = "failed";
    }
    public enum LogType
    {
        log_login = 15,
        log_logout,
        log_forgot_password,
        log_change_password,
        log_reset_password,
        log_create_account,
        log_create_account_with_file,
        log_create_account_bulk,
        log_edit_group,
        log_edit_account_for_admin,
        log_reset_password_admin,
        log_delete_account,
        log_delete_user_role,
        log_add_user_role,
        log_edit_account_for_helpdesk,
        log_create_account_temporary,
        log_edit_account,
        log_lock_account,
        log_unlock_account,
    }
    public static class LogActivity
    {
        public static string ResetPassword = "Reset Password";
        public static string ForgotPassword = "Forgot Password";
        public static string ChangePassword = "Change Password";
        public static string RegisterGuest = "Register Guest";
        public static string ChangeExpiryDate = "Change Expiry Date";
        public static string ApproveGuest = "Approve Guest";
        public static string RejectGuest = "Reject Guest";
        public static string EditGuest = "Edit Guest";
        public static string DeleteGuest = "Delete Guest";
        public static string ImportGuest = "Import Guest";
        public static string OTPRequest = "OTP Requested";
        public static string OTPVerified = "OTP Verified";
        public static string DisableGuest = "Disable Guest";
        public static string EnableGuest = "Enable Guest";
        public static string ResetPasswordAPI = "Reset Password by API";
    }
    public static class LockStaus
    {
        public static string Lock = "TRUE";
        public static string Unlock = "FALSE";
    }

    public enum ScriptFormat
    {
        UNIX1,
        Print,
        GW1,
        GW2,
        pigeon,
        cano,
        BB,
        EDMS,
        Info,
        pommo,
        Other,

    }
    public static class ScriptFormatParam
    {
        public static string UNIX1 = "[basic_uid]:[unix_uidNumber]:[unix_gidNumber]::::[basic_displayname]:[unix_homeDirectory]:[unix_loginShell]:[password_initial]";
        public static string Print = "[cu_CUexpire]:[cu_thcn]:[cu_thsn]:[system_org]:[basic_uid]:[password_initial]:[basic_mail]";
        public static string GW1 = "[email_address] ACCEPT";
        public static string GW2 = "[email_address]:[email_address]";
        public static string pigeon = "[basic_uid],[basic_givenname],[basic_sn],[unix_homeDirectory],2097152";
        public static string cano = "[basic_uid]@student.chula.ac.th,[basic_mail]";
        public static string BB = "[basic_uid]|[basic_uid]|[cu_jobcode]|[basic_givenname]|[basic_sn]|[email_address]|none|Student";
        public static string EDMS = "[basic_givenname]:[basic_sn]:[unix_homeDirectory]:[basic_uid]:[email_address]:[cu_jobcode]:[cu_pplid]";
        public static string Info = "[basic_uid]:[password_initial]";
        public static string pommo = "[basic_mail]::[basic_uid]:[basic_givenname]:[cu_gecos]:STAFF:[basic_sn]::";
        public static string Other = "[basic_uid]:[unix_uidNumber]:[unix_gidNumber]::::[basic_displayname]:[unix_homeDirectory]:[unix_loginShell]";
    }
    public static class EnumStatus
    {
        public static string ToUserType(this string text)
        {
            var status = aUUserType.staff;
            switch (text)
            {
                case "staff":
                    status = aUUserType.staff;
                    break;
                case "student":
                    status = aUUserType.student;
                    break;
                default:
                    break;
            }
            return status;
        }

        public static string toUserTypeName(this string statusType)
        {
            string status = "";
            if(statusType == aUUserType.staff)
                status = "Staff";
            else if (statusType == aUUserType.student)
                status = "Student";
            else if (statusType == aUUserType.vip)
                status = "VIP";
            else if (statusType == aUUserType.office)
                status = "Office";
            else if (statusType == aUUserType.bulk)
                status = "Guest";
            return status;
        }
        public static string toOUName(this string statusType)
        {
            string status = "";
            if (statusType == aUUserType.staff)
                status = "Staff";
            else if (statusType == aUUserType.student)
                status = "Student";
            else if (statusType == aUUserType.vip)
                status = "userVIP";
            else if (statusType == aUUserType.office)
                status = "userOffice";
            else if (statusType == aUUserType.bulk)
                status = "userTemp";
            else if (statusType == aUUserType.admin)
                status = "Service-user";
            return status;
        }
        public static string touserAccountControlName(this userAccountControl statusType)
        {
            string status = "";
            switch (statusType)
            {
                case userAccountControl.Disable:
                    status = "Disable";
                    break;
                case userAccountControl.DisablePasswordNotRequired:
                    status = "Disable";
                    break;
                case userAccountControl.Enable:
                    status = "Enable";
                    break;
                case userAccountControl.EnablePasswordNotRequired:
                    status = "Enable";
                    break;
                default:
                    break;
            }
            
            return status;
        }
        public static Status toStatus(this string text)
        {
            var status = Status.Enable;
            switch (text)
            {
                case "Disable":
                    status = Status.Disable;
                    break;
                case "Enable":
                    status = Status.Enable;
                    break;
                default:
                    break;
            }
            return status;
        }

        public static string toStatusName(this Status statusType)
        {
            string status = "";
            switch (statusType)
            {
                case Status.Disable:
                    status = "Disable";
                    break;
                case Status.Enable:
                    status = "Enable";
                    break;
                default:
                    break;
            }
            return status;
        }

        public static ApprovalStatus toApprovalStatus(this string text)
        {
            var status = ApprovalStatus.Pending;
            switch (text)
            {
                case "รอการอนุมัติ":
                    status = ApprovalStatus.Pending;
                    break;
                case "อนุมัติแล้ว":
                    status = ApprovalStatus.Approved;
                    break;
                case "ไม่อนุมัติ":
                    status = ApprovalStatus.Rejected;
                    break;
                default:
                    break;
            }
            return status;
        }

        public static string toApprovalStatusName(this ApprovalStatus statusType)
        {
            string status = "";
            switch (statusType)
            {
                case ApprovalStatus.Pending:
                    status = "รอการอนุมัติ";
                    break;
                case ApprovalStatus.Approved:
                    status = "อนุมัติแล้ว";
                    break;
                case ApprovalStatus.Rejected:
                    status = "ไม่อนุมัติ";
                    break;
                default:
                    break;
            }
            return status;
        }

        public static userAccountControl toUserAccountControl(this string text)
        {
            if (text == ((int)userAccountControl.Disable).ToString())
            {
                return userAccountControl.Disable;
            }
            else if (text == ((int)userAccountControl.DisablePasswordNotRequired).ToString())
            {
                return userAccountControl.Disable;
            }
            else if (text == ((int)userAccountControl.Enable).ToString())
            {
                return userAccountControl.Enable;
            }
            else if (text == ((int)userAccountControl.EnablePasswordNotRequired).ToString())
            {
                return userAccountControl.Enable;
            }
            return userAccountControl.Enable;
        }

        public static string toUserAccountControl(this userAccountControl statusType)
        {
            string status = "";
            switch (statusType)
            {
                case userAccountControl.Disable:
                    status = ((int)userAccountControl.Disable).ToString();
                    break;
                case userAccountControl.DisablePasswordNotRequired:
                    status = ((int)userAccountControl.DisablePasswordNotRequired).ToString();
                    break;
                case userAccountControl.Enable:
                    status = ((int)userAccountControl.Enable).ToString();
                    break;
                case userAccountControl.EnablePasswordNotRequired:
                    status = ((int)userAccountControl.EnablePasswordNotRequired).ToString();
                    break;
                default:
                    break;
            }
            return status;
        }

        public static ImportCreateOption toImportCreateOption(this string text)
        {
            if (text == ((int)ImportCreateOption.student).ToString())
            {
                return ImportCreateOption.student;
            }
            else if (text == ((int)ImportCreateOption.student_sasin).ToString())
            {
                return ImportCreateOption.student_sasin;
            }
            else if (text == ((int)ImportCreateOption.student_ppc).ToString())
            {
                return ImportCreateOption.student_ppc;
            }
            else if (text == ((int)ImportCreateOption.student_other).ToString())
            {
                return ImportCreateOption.student_other;
            }
            else if (text == ((int)ImportCreateOption.staff_hr).ToString())
            {
                return ImportCreateOption.staff_hr;
            }
            else if (text == ((int)ImportCreateOption.staff_other).ToString())
            {
                return ImportCreateOption.staff_other;
            }
            else if (text == ((int)ImportCreateOption.fixlogin).ToString())
            {
                return ImportCreateOption.fixlogin;
            }
            return ImportCreateOption.student;
        }
    }

}
