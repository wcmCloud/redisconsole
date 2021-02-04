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
                if (redisClient.Id == 0)
                    db.Instances.Add(redisClient);
                else
                    db.Instances.Update(redisClient);
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

        public static RedisClient Get(int id)
        {
            using (var db = new SQLiteDBContext())
            {
                var instance = db.Instances
                    .Where(p => p.Id == id)
                  .Single();
                return instance;
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

        public static List<Tuple<int, string>> GetInstanceKeys(string filter = null)
        {
            using (var db = new SQLiteDBContext())
            {
                if (filter == null || filter == "")
                    return db.Instances.OrderBy(x => x.Name).Select(k => new Tuple<int, string>(k.Id, k.Name)).ToList();
                else
                    return db.Instances.Where(p => p.Name.Contains(filter)).OrderBy(x => x.Name).Select(k => new Tuple<int, string>(k.Id, k.Name)).ToList();
            }

        }

    }
}
