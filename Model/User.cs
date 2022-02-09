using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace ABAC.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "เฉพาะตัวเลขและตัวอักษรภาษาอังกฤษ")]
        [MaxLength(250)]
        public string UserName { get; set; }
        [MaxLength(250)]
        public string Password { get; set; }


        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "รหัสผ่านไม่ตรงกัน")]
        [MaxLength(250)]
        public string ConfirmPassword { get; set; }

         [Display(Name = "create by")]
        [MaxLength(250)]
        public string Create_By { get; set; }
        [Display(Name = "create datetime")]
        public Nullable<DateTime> Create_On { get; set; }
        [Display(Name = "update by")]
        [MaxLength(250)]
        public string Update_By { get; set; }
        [Display(Name = "update datetime")]
        public Nullable<DateTime> Update_On { get; set; }
    }
}
