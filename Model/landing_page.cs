using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC.Models
{
    public class landing_page
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "landing page image url")]
        public string Url { get; set; }

        [Display(Name = "landing page image file name")]
        public string File_Name { get; set; }

        #region Audit

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

        #endregion      
    }
}
