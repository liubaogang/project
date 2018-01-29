namespace Net.Attri
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ComponentAttribute : Attribute
    {
        public ComponentAttribute() : this(null, null, Net.Base.LifeTime.PerResolve)
        {
        }

        public ComponentAttribute(string name) : this(name, null, Net.Base.LifeTime.PerResolve)
        {
        }

        public ComponentAttribute(Type from) : this(null, from, Net.Base.LifeTime.PerResolve)
        {
        }

        public ComponentAttribute(string name, Type from) : this(name, from, Net.Base.LifeTime.PerResolve)
        {
        }

        public ComponentAttribute(string name, Type from, Net.Base.LifeTime lifeTime)
        {
            this.Name = name;
            this.From = from;
            this.Lifetime = lifeTime;
        }

        public Type From { get; set; }

        public Net.Base.LifeTime Lifetime { get; set; }

        public string Name { get; set; }
    }
}
