using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderingFramework.Debug
{
    public abstract class HolderT
    {

        public virtual object v { get; set; }

        public abstract void SetValue(object o);

        public abstract void Print();
    }
}
