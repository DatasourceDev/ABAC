using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC.Models
{
    public class OU
    {
        [Key]
        public int OUID { get; set; }

        [Required]
        [Display(Name = "ชื่อ OU")]
        [MaxLength(250)]
        public string OUName { get; set; }

        [Display(Name = "รายละเอียด OU")]
        [MaxLength(1000)]
        public string OUDescription { get; set; }

        public bool Editable { get; set; }

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
