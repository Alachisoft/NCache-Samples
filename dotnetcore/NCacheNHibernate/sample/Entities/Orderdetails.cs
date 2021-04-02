using System;

namespace Sample.CustomerService.Domain 
{
    
    public class Orderdetails 
    {
        public virtual Orders Orders { get; set; }
        public virtual Products Products { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual short Quantity { get; set; }
        public virtual float Discount { get; set; }

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

            var other = obj as Orderdetails;

            if (other == null)
            {
                return false;
            }

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Orders.Equals(other.Orders) &&
                Products.Equals(other.Products))
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

            hash = hash * 23 + (Orders == null ?
                new Orders().GetHashCode() :
                Orders.GetHashCode());

            hash = hash * 23 + (Products == null ?
                new Products().GetHashCode() :
                Products.GetHashCode());

            return hash;
        }

        private static bool IsTransient(Orderdetails obj)
        {
            return obj != null &&
                obj.Orders == null &&
                obj.Products == null;
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
                $"{{Orders}}[{Orders.Id}]:{{Products}}[{Products.Id}]";
        }
    }
}
