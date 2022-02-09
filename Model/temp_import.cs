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

        [Display(Name = "username")]
        public string username { get; set; }

        [Display(Name = "password")]
        public string password { get; set; }


        [Display(Name = "firstname")]
        public string firstname { get; set; }

        [Display(Name = "lastname")]
        public string lastname { get; set; }

        [Display(Name = "citizen id")]
        public string CitizenID { get; set; }

        [Display(Name = "passport")]
        public string PassportID { get; set; }

        [Display(Name = "reference")]
        public string Reference { get; set; }

        [Display(Name = "import verify row status (0 is fail, 1 is success)")]
        public bool ImportVerify { get; set; }

        [Display(Name = "import row number")]
        public int ImportRow { get; set; }

        [Display(Name = "import row remark")]
        public string ImportRemark { get; set; }

        [Display(Name = "valid date")]
        public DateTime? valid_date { get; set; }

        [Display(Name = "expire date")]
        public DateTime? expire_date { get; set; }


    }
}
