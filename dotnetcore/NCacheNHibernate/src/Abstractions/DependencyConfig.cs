using Alachisoft.NCache.Runtime.Dependencies;
using System;

namespace NHibernate.Caches.NCache
{
    public abstract class DependencyConfig
    {
        public virtual int DependencyID { get; set; }
        public virtual string RegionPrefix { get; set; } = "nhibernate";
        public virtual string EntityClassFullName { get; set; }
        public virtual DatabaseDependencyType DatabaseDependencyType { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = obj as DependencyConfig;

            if (other == null)
            {
                return false;
            }


            return DependencyID == other.DependencyID &&
                   RegionPrefix.Equals(
                                    other.RegionPrefix,
                                    StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            if (this == null)
            {
                return base.GetHashCode();
            }

            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            if (this == null)
            {
                return null;
            }

            return DependencyID + "|" +
                   RegionPrefix + "|" +
                   DatabaseDependencyType.ToString() + "|" +
                   EntityClassFullName;
        }

        public abstract CacheDependency GetCacheDependency(
            string connectionString,
            object key);

        internal static bool ValidateKey(
            object key)
        {
            if (key == null)
            {
                return false;
            }

            var type = key.GetType();

            return type.Equals(typeof(string)) ||
                type.Equals(typeof(bool)) ||
                type.Equals(typeof(byte)) ||
                type.Equals(typeof(sbyte)) ||
                type.Equals(typeof(char)) ||
                type.Equals(typeof(decimal)) ||
                type.Equals(typeof(double)) ||
                type.Equals(typeof(float)) ||
                type.Equals(typeof(int)) ||
                type.Equals(typeof(uint)) ||
                type.Equals(typeof(long)) ||
                type.Equals(typeof(ulong)) ||
                type.Equals(typeof(short)) ||
                type.Equals(typeof(ushort));
        }

    }
}
