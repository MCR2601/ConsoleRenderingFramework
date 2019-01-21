using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework.Debug;

namespace OtherTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassicDebugger debugger = new ClassicDebugger();

            Holder<int> i = new Holder<int>("Index", 0, 10);
            Holder<int> sum = new Holder<int>("Sum", 0, 10);

            debugger.Watcher.Add(i);
            debugger.Watcher.Add(sum);

            debugger.Activate();

            for (i.SetValue(0); !Console.KeyAvailable; i.value++)
            {
                sum.value = sum.value + i.value;

                System.Threading.Thread.Sleep(10);
            }

            debugger.Abort();

            Console.ReadLine();
        }
    }
}
