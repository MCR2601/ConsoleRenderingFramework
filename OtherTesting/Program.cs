using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework.Debug;
using ConsoleRenderingFramework.Input;

namespace OtherTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassicDebugger debugger = new ClassicDebugger();

            Holder<int> x = new Holder<int>("X", 0, 10);
            Holder<int> y = new Holder<int>("Y", 0, 10);
            Holder<int> index = new Holder<int>("i", 0, 10);
            Holder<int> left = new Holder<int>("left", 0, 10);
            Holder<int> top = new Holder<int>("top", 0, 10);
            Holder<int> w = new Holder<int>("with", 0, 10);
            Holder<int> h = new Holder<int>("height", 0, 10);

            

            debugger.Watcher.Add(x);
            debugger.Watcher.Add(y);
            debugger.Watcher.Add(index);
            debugger.Watcher.Add(left);
            debugger.Watcher.Add(top);
            debugger.Watcher.Add(w);
            debugger.Watcher.Add(h);

            debugger.UpdateTime = 200;
            debugger.Activate();

            //Console.SetWindowPosition(10, 10);

            

            while (!Console.KeyAvailable)
            {
                //index.value++;
                var i = MouseInput.GetMousePosition();
                x.value = i.X;
                y.value = i.Y;

                var pos = MouseInput.GetMouseConsolePosition();

                left.value = pos.X;
                top.value = pos.Y;
                w.value = pos.W;
                h.value = pos.H;

                System.Threading.Thread.Sleep(50);
            }

            debugger.Abort();

            Console.ReadLine();
        }
    }
}
