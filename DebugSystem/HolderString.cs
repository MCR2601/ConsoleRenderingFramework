using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderingFramework.Debug
{
    public class HolderString : HolderT
    {
        public string Value;

        public HolderString(string value)
        {
            Value = value;
        }

        public override void Print()
        {
            Console.WriteLine(Value);
        }

        public override void SetValue(object o)
        {
            Value = o.ToString();
        }
    }
}
