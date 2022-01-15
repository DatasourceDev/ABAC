using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC.Models
{
    public class m_step_object_details
    {
        [Key]
        public int row_key { get; set; }

        public string step_history_id { get; set; }
        public string run_result { get; set; }
        public string cs_dn { get; set; }
        public string cs_object_id { get; set; }

    }
}
