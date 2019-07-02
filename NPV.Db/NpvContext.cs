using NPV.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPV.Db
{
    public class NpvContext : DbContext
    {
        public NpvContext() : base("name=DefaultConnection")
        {

        }

        public NpvContext(string connectionString) : base(connectionString)
        {

        }


        public DbSet<NPVRecord> NPVRecords { get; set; }
    }
}
