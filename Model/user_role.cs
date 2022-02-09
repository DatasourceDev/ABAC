using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC.Models
{
    public class user_role
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "username")]
        public string username { get; set; }

        [Display(Name = "role type (admin, helpdesk, password-operator, web master)")]
        public string roleType { get; set; }
        #region Audit

         [Display(Name = "create by")]
        [MaxLength(250)]
        public string Create_By { get; set; }
        [Display(Name = "create datetime")]
        public Nullable<DateTime> Create_On { get; set; }

        #endregion      
    }
}
