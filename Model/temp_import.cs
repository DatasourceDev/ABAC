using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC.Models
{
    public class temp_import
    {
        [Key]
        public int ID { get; set; }

        public string username { get; set; }
        public string password { get; set; }


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
        public bool ImportVerify { get; set; }
        public int ImportRow { get; set; }
        public string ImportRemark { get; set; }

        public DateTime? valid_date { get; set; }
        public DateTime? expire_date { get; set; }


    }
}
