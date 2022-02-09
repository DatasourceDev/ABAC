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

        [Display(Name = "A code that is used to verify a account")]
        public string Code { get; set; }

        [Display(Name = "code expiry date")]
        public DateTime? Expiry_Date { get; set; }

        [Display(Name = "code status (0 is inactive, 1 is active)")]
        public bool Active { get; set; }

        #region Audit

        [Display(Name = "create by")]
        [MaxLength(250)]
        public string Create_By { get; set; }
        [Display(Name = "create datetime")]
        public Nullable<DateTime> Create_On { get; set; }

        #endregion      
    }
}
