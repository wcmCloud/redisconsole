using System;
using System.Collections.Generic;
using System.Text;

namespace Redis.Core
{
    public static class ObjectsSafeExtensions
    {

        public static void AddToDictionary<T,X>(this Dictionary<T,X> dic, T key, X value)
        {
            try
            {
                dic.Add(key, value);
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }
        }


    }
}
