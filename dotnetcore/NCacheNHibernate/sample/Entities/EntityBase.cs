using System;

namespace Sample.CustomerService.Domain
{
    public abstract class Entity<TId>
    {
        public virtual TId Id { get; set; }

        public override bool Equals (object obj)
        {
            return Equals(obj as Entity<TId>);
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(TId)))
            {
                return base.GetHashCode();
            }

            return Id.GetHashCode();
        }

        private static bool IsTransient(Entity<TId> obj)
        {
            return obj != null &&
                Equals(obj.Id, default(TId));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(Entity<TId> other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();

                return thisType.IsAssignableFrom(otherType) ||
                    otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

    }
}
