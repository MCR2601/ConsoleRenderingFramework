using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderingFramework.Debug
{
    public class Holder<T> : HolderT
    {
        public string Name;
        public T value;

        public int PadName;

        private bool hasChanged;

        public Holder(string name, T value, int padName = 1 )
        {
            Name = name;
            this.value = value;
            PadName = padName;
        }

        public bool HasChanged { get => hasChanged; private set => hasChanged = value; }

        public void SetValue(T newV)
        {
            value = newV;
            HasChanged = true;
        }

        public override void SetValue(object o)
        {
            if (o is T)
            {
                SetValue((T)o);
            }
        }

        public override void Print()
        {
            Console.WriteLine((Name + ":").PadRight(PadName) + value);
        }

        public static implicit operator T(Holder<T> h)
        {
            return h.value;
        }
    }
}
