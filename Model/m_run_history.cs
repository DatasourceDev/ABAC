using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABAC.Models
{
    public class m_run_history
    {
        [Key]
        public string run_history_id { get; set; }

        public string ma_id { get; set; }

        public int run_profile_id { get; set; }
        public string run_result { get; set; }

        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string run_profile_name { get; set; }

    }
}
