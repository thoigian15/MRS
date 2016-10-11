using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertDB.MySQLModel
{
    [Table("wv_hs_patient_data")]
    public class wv_hs_patient_data
    {
        [Key, Column(Order = 0)]
        public string type { get; set; }
        [Key, Column(Order = 1)]
        public string id_patient { get; set; }
        [Key, Column(Order = 2)]
        public string visit_number { get; set; }
        public string data { get; set; }
    }
}
