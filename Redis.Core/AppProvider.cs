using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Redis.Core
{
    public static class AppProvider
    {
        public static AppConfiguration Configuration { get; set; }


        public static void Store(RedisClient redisClient)
        {
            using (var db = new SQLiteDBContext())
            {
                db.Instances.Add(redisClient);
                db.SaveChanges();
            }
        }

        public static RedisClient Get(string redisClientKey)
        {
            using (var db = new SQLiteDBContext())
            {
                var isntance = db.Instances
                    .Where(p => p.Name == redisClientKey)
                  .Single();
                return isntance;
            }
        }

        public static void Delete(string redisClientKey)
        {
            using (var db = new SQLiteDBContext())
            {
                var redisClient = db.Instances.Where(p => p.Name == redisClientKey).Single();
                db.Instances.Remove(redisClient);
                db.SaveChanges();
            }
        }

        public static int GetCount()
        {
            using (var db = new SQLiteDBContext())
            {
                return db.Instances.Count();
            }
        }

        public static List<string> GetKeys(string filter = null)
        {
            using (var db = new SQLiteDBContext())
            {
                if (filter == null || filter == "")
                    return db.Instances.OrderBy(x => x.Name).Select(k => k.Name).ToList();
                else
                    return db.Instances.Where(p => p.Name.Contains(filter)).OrderBy(x => x.Name).Select(k => k.Name).ToList();
            }

        }

    }
}
