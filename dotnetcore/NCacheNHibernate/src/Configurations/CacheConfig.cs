using Alachisoft.NCache.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHibernate.Caches.NCache
{
    public class CacheConfig
    {
        internal string CacheConfigId
        {
            get
            {
                return $"{RegionPrefix}:{CacheId}";
            }
        }
        public string CacheId { get; set; }
        public bool IsDefault { get; set; } = false;
        public string RegionPrefix { get; set; } = "nhibernate";

        internal readonly CacheConnectionOptions connectionOptions =
            new CacheConnectionOptions 
            {
                DefaultReadThruProvider = "",
                DefaultWriteThruProvider = ""
            };
        public double? ConnectionTimeout
        {
            get
            {
                if (connectionOptions.ConnectionTimeout.HasValue)
                {
                    return connectionOptions.ConnectionTimeout.Value.TotalSeconds;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    connectionOptions.ConnectionTimeout =
                        TimeSpan.FromSeconds(value.Value);
                }
            }
        }
        public double? KeepAliveInterval
        {
            get
            {
                if (connectionOptions.KeepAliveInterval.HasValue)
                {
                    return connectionOptions.KeepAliveInterval.Value.TotalSeconds;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    connectionOptions.KeepAliveInterval =
                        TimeSpan.FromSeconds(value.Value);
                }
            }
        }
        public bool? EnableKeepAlive
        {
            get
            {
                if (connectionOptions.EnableKeepAlive.HasValue)
                {
                    return connectionOptions.EnableKeepAlive.Value;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    connectionOptions.EnableKeepAlive = value.Value;
                }
            }
        }
        public double? CommandRetryInterval
        {
            get
            {
                if (connectionOptions.CommandRetryInterval.HasValue)
                {
                    return connectionOptions.CommandRetryInterval.Value.TotalSeconds;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    connectionOptions.CommandRetryInterval =
                        TimeSpan.FromSeconds(value.Value);
                }
            }
        }
        public int? CommandRetries
        {
            get
            {
                if (connectionOptions.CommandRetries.HasValue)
                {
                    return connectionOptions.CommandRetries.Value;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    connectionOptions.CommandRetries = value.Value;
                }
            }
        }
        public double? RetryConnectionDelay
        {
            get
            {
                if (connectionOptions.RetryConnectionDelay.HasValue)
                {
                    return connectionOptions.RetryConnectionDelay.Value.TotalSeconds;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    connectionOptions.RetryConnectionDelay =
                        TimeSpan.FromSeconds(value.Value);
                }
            }
        }
        public double? RetryInterval
        {
            get
            {
                if (connectionOptions.RetryInterval.HasValue)
                {
                    return connectionOptions.RetryInterval.Value.TotalSeconds;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    connectionOptions.RetryInterval =
                        TimeSpan.FromSeconds(value.Value);
                }
            }
        }
        public int? ConnectionRetries
        {
            get
            {
                if (connectionOptions.ConnectionRetries.HasValue)
                {
                    return connectionOptions.ConnectionRetries.Value;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    connectionOptions.ConnectionRetries = value.Value;
                }
            }
        }
        public bool? EnableClientLogs
        {
            get
            {
                if (connectionOptions.EnableClientLogs.HasValue)
                {
                    return connectionOptions.EnableClientLogs.Value;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    connectionOptions.EnableClientLogs = value.Value;
                }
            }
        }
        public double? ClientRequestTimeout
        {
            get
            {
                if (connectionOptions.ClientRequestTimeOut.HasValue)
                {
                    return connectionOptions.ClientRequestTimeOut.Value.TotalSeconds;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    connectionOptions.ClientRequestTimeOut =
                        TimeSpan.FromSeconds(value.Value);
                }
            }
        }
        public bool? LoadBalance
        {
            get
            {
                if (connectionOptions.LoadBalance.HasValue)
                {
                    return connectionOptions.LoadBalance.Value;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    connectionOptions.LoadBalance = value.Value;
                }
            }
        }
        public string AppName
        {
            get
            {
                return connectionOptions.AppName;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    connectionOptions.AppName = value;
                }
            }
        }
        public string ClientBindIP
        {
            get
            {
                return connectionOptions.ClientBindIP;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    connectionOptions.ClientBindIP = value;
                }
            }
        }
        public NCacheLogLevel? LogLevel
        {
            get
            {
                if (connectionOptions.LogLevel.HasValue)
                {
                    var logLevel = connectionOptions.LogLevel.Value;

                    if (logLevel == Alachisoft.NCache.Client.LogLevel.Debug)
                    {
                        return NCacheLogLevel.Debug;
                    }
                    else if (logLevel == Alachisoft.NCache.Client.LogLevel.Error)
                    {
                        return NCacheLogLevel.Error;
                    }
                    else
                    {
                        return NCacheLogLevel.Info;
                    }
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    var logLevel = value.Value;

                    if (logLevel == NCacheLogLevel.Info)
                    {
                        connectionOptions.LogLevel = Alachisoft.NCache.Client.LogLevel.Info;
                    }
                    else if (logLevel == NCacheLogLevel.Error)
                    {
                        connectionOptions.LogLevel = Alachisoft.NCache.Client.LogLevel.Error;
                    }
                    else
                    {
                        connectionOptions.LogLevel = Alachisoft.NCache.Client.LogLevel.Debug;
                    }
                }
            }
        }
        public NCacheIsolationLevel? Mode
        {
            get
            {
                if (connectionOptions.Mode.HasValue)
                {
                    var mode = connectionOptions.Mode.Value;

                    if (mode == IsolationLevel.Default)
                    {
                        return NCacheIsolationLevel.Default;
                    }
                    else if (mode == IsolationLevel.InProc)
                    {
                        return NCacheIsolationLevel.InProc;
                    }
                    else
                    {
                        return NCacheIsolationLevel.Outproc;
                    }
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    var mode = value.Value;

                    if (mode == NCacheIsolationLevel.Default)
                    {
                        connectionOptions.Mode = IsolationLevel.Default;
                    }
                    else if (mode == NCacheIsolationLevel.InProc)
                    {
                        connectionOptions.Mode = IsolationLevel.InProc;
                    }
                    else
                    {
                        connectionOptions.Mode = IsolationLevel.OutProc;
                    }
                }
            }
        }
        public List<NCacheServerInfo> ServerList
        {
            get
            {

                if (connectionOptions.ServerList.Count > 0)
                {

                    List<NCacheServerInfo> serverList =
                        new List<NCacheServerInfo>();

                    serverList.AddRange(
                        connectionOptions.ServerList
                                            .Select(x => 
                                                        new NCacheServerInfo(x)));
                }

                return null;
            }
            set
            {
                if (value != null &&
                    value.Count > 0)
                {
                    for (int i = 0; i < value.Count; i++)
                    {
                        if (value[i].IP != null)
                        {
                            connectionOptions.ServerList.Add(
                                new ServerInfo(value[i].IP, value[i].Port)
                                {
                                    Priority = value[i].Priority
                                });
                        }
                        else
                        {
                            connectionOptions.ServerList.Add(
                                new ServerInfo(value[i].Name, value[i].Port)
                                {
                                    Priority = value[i].Priority
                                });
                        }
                    }
                }
            }
        }
        public NCacheCredentials UserCredentials
        {
            get
            {
                if (connectionOptions.UserCredentials != null)
                {
                    return new NCacheCredentials(
                                    connectionOptions.UserCredentials.UserID,
                                    connectionOptions.UserCredentials.Password);
                }

                return null;
            }
            set
            {
                if (value != null)
                {
                    connectionOptions.UserCredentials =
                        new Credentials(value.UserID, value.Password);
                }
            }
        }
        public NCacheClientSyncMode? SyncMode
        {
            get
            {
                if (connectionOptions.ClientCacheMode.HasValue)
                {
                    var mode = connectionOptions.ClientCacheMode.Value;

                    if (mode == ClientCacheSyncMode.Optimistic)
                    {
                        return NCacheClientSyncMode.Optimistic;
                    }
                    else
                    {
                        return NCacheClientSyncMode.Pessimistic;
                    }
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    var mode = value.Value;

                    if (mode == NCacheClientSyncMode.Optimistic)
                    {
                        connectionOptions.ClientCacheMode =
                                            ClientCacheSyncMode.Optimistic;
                    }
                    else
                    {
                        connectionOptions.ClientCacheMode =
                                            ClientCacheSyncMode.Pessimistic;
                    }
                }
            }
        }
    }
}
