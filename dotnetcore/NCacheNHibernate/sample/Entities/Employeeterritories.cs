using System;


namespace Sample.CustomerService.Domain 
{
    
    public class Employeeterritories 
    {
        public virtual Employees Employees { get; set; }
        public virtual Territories Territories { get; set; }

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

            var other = obj as Employeeterritories;

            if (other == null)
            {
                return false;
            }

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Employees.Equals(other.Employees) &&
                Territories.Equals(other.Territories))
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

            hash = hash * 23 + (Employees == null ?
                new Employees().GetHashCode() :
                Employees.GetHashCode());

            hash = hash * 23 + (Territories == null ?
                new Territories().GetHashCode() :
                Territories.GetHashCode());

            return hash;
        }

        private static bool IsTransient(Employeeterritories obj)
        {
            return obj != null &&
                obj.Employees == null &&
                obj.Territories == null;
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
                $"{{Employees}}[{Employees.Id}]:{{Territories}}[{Territories.Id}]";
        }
    }
}
