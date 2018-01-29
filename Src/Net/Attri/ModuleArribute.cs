using System;
namespace Net.Attri
{
    using System;

    public class ModuleArribute : Attribute
    {
        public bool Disable { get; set; }

        public int Order { get; set; }
    }
}
