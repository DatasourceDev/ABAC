using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC.Models
{
    public class activate_code
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "username")]
        public string UserName { get; set; }

        [Display(Name = "Role Type")]
        public string Code { get; set; }

        [Display(Name = "Expiry_Date")]
        public DateTime? Expiry_Date { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        #region Audit

        [Display(Name = "ผู้สร้าง")]
        [MaxLength(250)]
        public string Create_By { get; set; }
        [Display(Name = "เวลาสร้าง")]
        public Nullable<DateTime> Create_On { get; set; }

        #endregion      
    }
}
