using Alachisoft.NCache.Client;
using System;
using System.Net;

namespace NHibernate.Caches.NCache
{
    public class NCacheServerInfo : IComparable
    {
        internal readonly ServerInfo serverInfo;

        public NCacheServerInfo() : this("", 9800)
        { }

        public NCacheServerInfo(
            string name,
            int port = 9800)
        {
            serverInfo = new ServerInfo(name, port);
        }
        public NCacheServerInfo(
            IPAddress ip,
            int port = 9800)
        {
            serverInfo = new ServerInfo(ip, port);
        }

        internal NCacheServerInfo(
            ServerInfo serverInfo)
        {
            this.serverInfo = serverInfo;
        }

        public int Port
        {
            get
            {
                return serverInfo.Port;
            }
            set
            {
                serverInfo.Port = value;
            }
        }
        public string Name
        {
            get
            {
                return serverInfo.Name;
            }
            set
            {
                serverInfo.Name = value;
            }
        }
        public IPAddress IP
        {
            get
            {
                return serverInfo.IP;
            }
            set
            {
                serverInfo.IP = value;
            }
        }
        public short Priority
        {
            get
            {
                return serverInfo.Priority;
            }
            set
            {
                serverInfo.Priority = value;
            }
        }

        public int CompareTo(object obj)
        {
            return serverInfo.CompareTo(obj);
        }

        public override bool Equals(object obj)
        {
            return serverInfo.Equals(obj);
        }

        public override string ToString()
        {
            return serverInfo.ToString();
        }

        public override int GetHashCode()
        {
            return serverInfo.GetHashCode();
        }
    }
}
