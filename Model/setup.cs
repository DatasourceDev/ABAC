using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC.Models
{
    public class setup
    {
        [Key]
        public int ID { get; set; }

        #region AD
        [Display(Name = "ad host")]
        [MaxLength(150)]
        public string Host { get; set; }

        [Display(Name = "ad port")]
        [MaxLength(150)]
        public string Port { get; set; }

        [Display(Name = "ad base")]
        [MaxLength(150)]
        public string Base { get; set; }

        [Display(Name = "ad username")]
        [MaxLength(150)]
        public string Username { get; set; }

        [Display(Name = "ad password")]
        [MaxLength(150)]
        public string Password { get; set; }
        #endregion        

        #region SMTP

        [Display(Name = "SMTP server")]
        [MaxLength(150)]
        public string SMTP_Server { get; set; }

        [Display(Name = "SMTP port")]
        public int SMTP_Port { get; set; }

        [Display(Name = "SMTP from")]
        [MaxLength(150)]
        public string SMTP_From { get; set; }

        [Display(Name = "SMTP username")]
        [MaxLength(150)]
        public string SMTP_Username { get; set; }

        [Display(Name = "SMTP password")]
        [MaxLength(150)]
        public string SMTP_Password { get; set; }

        [Display(Name = "SMTP SSL")]
        public bool? SMTP_SSL { get; set; }
        #endregion

        #region setup
        [Display(Name = "guest row number")]
        public int GuestRowNumber { get; set; }

        [Display(Name = "landing page text")]
        public string first_page_description { get; set; }


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
