using ConvertDB.MySQLModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertDB
{
    public partial class MySQLEntities : DbContext
    {
        public MySQLEntities() : base(nameOrConnectionString: "MySQLPortal") { }
        public DbSet<wv_hs_patient> Patients { get; set; }
        public DbSet<wv_hs_patient_data> Datas { get; set; }
    }
}
