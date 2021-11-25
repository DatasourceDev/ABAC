using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC.Models
{
    public class cms
    {
        [Key]
        public int ID { get; set; }

        #region CMS
        [Display(Name = "CMS Student หน้า Home")]
        public string HOME_Student { get; set; }
        [Display(Name = "CMS Staff หน้า Home")]
        public string HOME_Staff { get; set; }
        [Display(Name = "CMS VIP หน้า Home")]
        public string HOME_VIP { get; set; }
        [Display(Name = "CMS Office หน้า Home")]
        public string HOME_Office { get; set; }
        [Display(Name = "CMS Guest หน้า Home")]
        public string HOME_Guest { get; set; }
        #endregion
        #region Audit

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

        #endregion      
    }
}
