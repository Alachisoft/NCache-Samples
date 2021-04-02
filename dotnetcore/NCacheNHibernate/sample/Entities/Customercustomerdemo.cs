using System;

namespace Sample.CustomerService.Domain
{
    public class Customercustomerdemo
    {
        public virtual Customers Customers { get; set; }
        public virtual Customerdemographics Customerdemographics { get; set; }

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

            var other = obj as Customercustomerdemo;

            if (other == null)
            {
                return false;
            }

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Customers.Equals(other.Customers) &&
                Customerdemographics.Equals(other.Customerdemographics))
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

            hash = hash * 23 + (Customers == null ?
                new Customers().GetHashCode() :
                Customers.GetHashCode());

            hash = hash * 23 + (Customerdemographics == null ?
                new Customerdemographics().GetHashCode() :
                Customerdemographics.GetHashCode());

            return hash;
        }

        private static bool IsTransient(Customercustomerdemo obj)
        {
            return obj != null &&
                obj.Customerdemographics == null &&
                obj.Customers == null;
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
                $"{{Customers}}[{Customers.Id}]:" +
                $"{{Customerdemographics}}[{Customerdemographics.Id}]";
        }
    }
}
