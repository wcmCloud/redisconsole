using System;
using System.Collections.Generic;
using System.Text;

namespace Redis.Core
{
    public class RedisClient
    {
        const char DEFAULT_NAMESPACE_SEPARATOR = ':';
        const char DEFAULT_KEYS_GLOB_PATTERN = '*';
        const bool DEFAULT_LUA_KEYS_LOADING = false;
        const uint DEFAULT_DB_SCAN_LIMIT = 20;

        #region properties

        //* Basic settings */
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Auth { get; set; }

        //* SSL settings */
        public bool SslEnabled { get; set; }
        public string SslLocalCertPath { get; set; }
        public string SslPrivateKeyPath { get; set; }
        public string SslCaCertPath { get; set; }

        //* SSH Settings */
        public string SshPassword { get; set; }
        public string SshUser { get; set; }
        public string SshHost { get; set; }
        public int SshPort { get; set; }
        public string SshPrivateKey { get; set; }


        //* Advanced settings */
        public string KeysPattern { get; set; }
        public string NamespaceSeparator { get; set; } = "";
        public int ExecuteTimeout { get; set; }
        public int ConnectionTimeout { get; set; }
        public bool LuaKeysLoading { get; set; }
        public bool OverrideClusterHost { get; set; }
        public bool IgnoreSSLErrors { get; set; }
        public int DatabaseScanLimit { get; set; }

        #endregion

        public RedisClient() { }


    }
}
