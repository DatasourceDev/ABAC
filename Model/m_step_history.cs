using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC.Models
{
    public class m_step_history
    {
        [Key]
        public string step_history_id { get; set; }

        public string run_history_id { get; set; }

        public int step_number { get; set; }

        public string step_result { get; set; }

        public string start_date { get; set; }
        public string end_date { get; set; }
        public int export_add { get; set; }
        public int export_update { get; set; }
        public int export_rename { get; set; }
        public int export_delete { get; set; }
        public int export_deleteadd { get; set; }
        public int export_failure { get; set; }

        public string run_profile_name { get; set; }
        public string ma_name { get; set; }


    }
}
