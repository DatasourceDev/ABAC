using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC.Models
{
    public class User_Bulk_Import
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "username")]
        public string username { get; set; }

        [Display(Name = "password")]
        public string password { get; set; }

        [Display(Name = "adminname")]
        public string adminname { get; set; }

        [Display(Name = "firstname")]
        public string firstname { get; set; }

        [Display(Name = "lastname")]
        public string lastname { get; set; }

        [Display(Name = "transaction datetime")]
        public DateTime? today { get; set; }

        [Display(Name = "expire date")]
        public DateTime? valid_date { get; set; }

        [Display(Name = "expire date")]
        public DateTime? expire_date { get; set; }

        [Display(Name = "citizen id")]
        public string CitizenID { get; set; }

        [Display(Name = "passport")]
        public string PassportID { get; set; }

        [Display(Name = "reference")]
        public string Reference { get; set; }

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

        [Display(Name = "ad creation status (0 is not create on ad, 1 is created)")]
        public bool ad_created { get; set; }
    }
}
