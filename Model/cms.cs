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
        [Display(Name = "html content at student home page")]
        public string HOME_Student { get; set; }

        [Display(Name = "html content at staff home page")]
        public string HOME_Staff { get; set; }

        [Display(Name = "html content at vip home page")]
        public string HOME_VIP { get; set; }

        [Display(Name = "html content at office home page")]
        public string HOME_Office { get; set; }

        [Display(Name = "html content at guest home page")]
        public string HOME_Guest { get; set; }
        #endregion

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
