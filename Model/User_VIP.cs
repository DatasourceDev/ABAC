using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC.Models
{
    public class User_VIP
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "รหัสผู้ใช้")]
        public string username { get; set; }

        [Display(Name = "รหัสผ่าน")]
        public string password { get; set; }

        [Display(Name = "adminname")]
        public string adminname { get; set; }

        [Display(Name = "ชื่อ")]
        public string firstname { get; set; }

        [Display(Name = "นามสกุล")]
        public string lastname { get; set; }

        [Display(Name = "รหัสบัตรประชาชน")]
        public string CitizenID { get; set; }

        [Display(Name = "Passport")]
        public string PassportID { get; set; }

        [Display(Name = "Reference")]
        public string Reference { get; set; }

        //[NotMapped]
        //[Display(Name = "วันที่เปิดใช้งาน")]
        //public DateTime? OpenDate { get; set; }

        //[NotMapped]
        //[Display(Name = "วันที่หมดอายุ")]
        //public DateTime? ExpiryDate { get; set; }


        [Display(Name = "ผู้สร้าง")]
        [MaxLength(250)]
        public string Create_By { get; set; }
        [Display(Name = "เวลาสร้าง")]
        public Nullable<DateTime> Create_On { get; set; }
        [Display(Name = "ผู้แก้ไข")]
        [MaxLength(250)]
        public string Update_By { get; set; }
        [Display(Name = "เวลาแก้ไข")]
        public Nullable<DateTime> Update_On { get; set; }

        public bool ad_created { get; set; }




    }
}
