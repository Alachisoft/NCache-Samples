using System;

namespace Sample.CustomerService.Domain
{
    public class NCachePollingTable
    {
        public virtual string cache_key { get; set; }
        public virtual string cache_id { get; set; }
        public virtual bool modified { get; set; } = false;
        public virtual bool work_in_progress { get; set; } = false;


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

            var other = obj as NCachePollingTable;

            if (other == null)
            {
                return false;
            }

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                cache_id == other.cache_id &&
                cache_key == other.cache_key)
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();

                return thisType.IsAssignableFrom(otherType) ||
                    otherType.IsAssignableFrom(thisType);
            }


            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 23 + (string.IsNullOrEmpty(cache_key) ?
                string.Empty.GetHashCode() :
                cache_key.GetHashCode());

            hash = hash * 23 + (string.IsNullOrEmpty(cache_id) ?
                string.Empty.GetHashCode() :
                cache_id.GetHashCode());

            return hash;
        }

        private static bool IsTransient(NCachePollingTable obj)
        {
            return obj != null &&
                string.IsNullOrEmpty(obj.cache_id) &&
                string.IsNullOrEmpty(obj.cache_key);
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public override string ToString()
        {
            if (this == null || IsTransient(this))
            {
                return string.Empty;
            }

            return $"{GetType().AssemblyQualifiedName}:" +
                $"{{cache_key}}[{cache_key}]:{{cache_id}}[{cache_id}]";
        }
    }
}
