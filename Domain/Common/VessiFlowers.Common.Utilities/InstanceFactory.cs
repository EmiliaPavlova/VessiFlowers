namespace VessiFlowers.Common.Utilities
{
    using System;

    public class InstanceFactory<T>
    {
        private readonly Type implementationType;

        public InstanceFactory(T implementation)
        {
            if (implementation == null)
            {
                throw new ArgumentNullException(nameof(implementation));
            }

            this.implementationType = implementation.GetType();
        }

        public virtual T Create()
        {
            return (T)Activator.CreateInstance(this.implementationType);
        }
    }
}
