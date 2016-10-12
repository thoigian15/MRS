using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertDB.MySQLModel
{
    [Table("wv_hs_patient")]
    public class wv_hs_patient
    {
        [Key]
        public string id { get; set; }
        public string pass { get; set; }
        public string name { get; set; }
        public DateTime dob { get; set; }
        public bool gender { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string h_phone { get; set; }
        public string m_phone { get; set; }
        public DateTime last_updated { get; set; }
    }
}
