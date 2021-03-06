using ABAC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC.DTO
{
    public class SSODTO
    {
        public string responseXml { get; set; }
        public string actionUrl { get; set; }
        public string relayState { get; set; }
    }
    public class Result
    {
        public bool result { get; set; }
        public string Message { get; set; }

    }
    public class Organization
    {
        public string ouname { get; set; }
        public string ou { get; set; }
        public string schemaname { get; set; }
        public string path { get; set; }

    }
    public class SearchDTO
    {
        public string text_search { get; set; }
        public string logstatus_search { get; set; }

        public string usertype_search { get; set; }
        public string log_type_search { get; set; }

        public Status? status_search { get; set; }
        public string id { get; set; }
        public string dfrom { get; set; }
        public string dto { get; set; }
        public string import_option { get; set; }
        public string create_by { get; set; }

        public string option { get; set; }


        private int _pageno; // field
        public int pageno {
            get
            {
                if (_pageno == 0)
                    return 1;
                return _pageno;
            }
            set { _pageno = value; }
        }
        private int _pagelen; // field
        public int pagelen
        {
            get
            {
                if (_pagelen == 0)
                    return 1;
                return _pagelen;
            }
            set { _pagelen = value; }
        }
        
        public int itemcnt { get; set; }

        public ReturnCode? code { get; set; }
        public string msg { get; set; }
        public string temp { get; set; }

        private IQueryable<object> _lists;
        
        public IQueryable<object> lists {
            get
            {                
                return _lists;
            }
            set
            {
                _lists = value;
            }
        }
    }
    public class RenameDTO
    {
        [Required]
        public string SamAccountName { get; set; }

        [Required]
        public string newSamAccountName { get; set; }

    }
    public class UserRoleDTO
    {
        public string text_search { get; set; }
        public string userrole_search { get; set; }

        public ReturnCode? code { get; set; }
        public string msg { get; set; }

        private IQueryable<object> _lists;
        private IQueryable<object> _lists2;
        public IQueryable<object> lists
        {
            get
            {
                return _lists;
            }
            set
            {
                _lists = value;
            }
        }

        public IQueryable<object> lists2
        {
            get
            {
                return _lists2;
            }
            set
            {
                _lists2 = value;
            }
        }
    }
    public class ImportDTO
    {
        public string cu_pplid { get; set; }
        public string basic_uid { get; set; }

    }
    public class GetPasswordDTO : BaseDTO
    {
        [MaxLength(10)]
        [Required]
        public string cu_jobcode { get; set; }


        [MaxLength(13)]
        public string cu_pplid { get; set; }

        [MaxLength(500)]
        public string basic_sn { get; set; }

        [MaxLength(500)]
        public string basic_givenname { get; set; }
        
    }
    public class LoginDTO : BaseDTO
    {
        public string SAMLRequest { get; set; }
        public string RelayState { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(100)]
        public string Password { get; set; }

        public bool isSSO { get; set; }

    }
    public class ChangePasswordDTO : BaseDTO
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(16, ErrorMessage = "Your password must be between {2} and {1} characters.", MinimumLength = 8)]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "Your password must contain at least one uppercase, lowercase letter and number digit.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(16, ErrorMessage = "Your password must be between {2} and {1} characters.", MinimumLength = 8)]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "Your password must contain at least one uppercase, lowercase letter and number digit.")]
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
    public class ChangePassword2DTO : BaseDTO
    {
        [Required]
        public string UserName { get; set; }

        public string Code { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(16, ErrorMessage = "Your password must be between {2} and {1} characters.", MinimumLength = 8)]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "Your password must contain at least one uppercase, lowercase letter and number digit.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        [StringLength(16, ErrorMessage = "Your password must be between {2} and {1} characters.", MinimumLength = 8)]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "Your password must contain at least one uppercase, lowercase letter and number digit.")]
        public string ConfirmPassword { get; set; }
    }
    public class ChangePassword3DTO
    {
        [Required]
        public string id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(16, ErrorMessage = "Your password must be between {2} and {1} characters.", MinimumLength = 8)]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "Your password must contain at least one uppercase, lowercase letter and number digit.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        [StringLength(16, ErrorMessage = "Your password must be between {2} and {1} characters.", MinimumLength = 8)]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,16}$", ErrorMessage = "Your password must contain at least one uppercase, lowercase letter and number digit.")]

        public string ConfirmPassword { get; set; }
    }
    public class Bulk : BaseDTO
    {
        public int NumberOfPeople { get; set; }
        public string ValidDate { get; set; }
        public string ExpireDate { get; set; }

    }

    public class BaseDTO
    {

    }

    public class OTPDTO : BaseDTO
    {
        [Required]
        [MaxLength(6)]
        public string OTP { get; set; }
        public string RefNo { get; set; }
        public bool Renew { get; set; }

    }
    public class ForgotPasswordDTO : BaseDTO
    {
        [Required]
        public string UserName { get; set; }
        public string aCode { get; set; }

        public SendMessageType SendMessageType { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class SMSModel
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Msnlist { get; set; }
        public string Msg { get; set; }
        public string Sender { get; set; }

    }

    public class PasswordGenerateDTO 
    {
        public int PasswordCnt { get; set; }
        public int Length { get; set; }
        public bool Number { get; set; }
        public bool Lower { get; set; }
        public bool Upper { get; set; }

        public List<string> Passwords { get; set; }
    }

    public class ShowPasswordDTO
    {
        public string Password { get; set; }
    }
   
}
