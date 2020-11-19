using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis.Core
{
    public class SQLiteDBContext : DbContext
    {
        public DbSet<RedisClient> Instances { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=sqliteRedisInstances.db");
    }
}
