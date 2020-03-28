using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;
using ConsoleRenderingFramework.Debug;
using ConsoleRenderingFramework.Input;
using ConsoleRenderingFramework.ConsoleSpeedUp;

namespace OtherTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            Stopwatch stop = new Stopwatch();

            DirectConsoleAccess d = new DirectConsoleAccess(90, 90, 1, 1);
            FastGMU gmu = new FastGMU(90,90);

            Console.ReadLine();
            stop.Start();

            //d.TestOutput();
            //d.TestPInfo();

            PInfo[,] data = new PInfo[90,90];
            data.Populate(new PInfo().SetC('A').SetBg(ConsoleColor.Black).SetFg(ConsoleColor.White));
            d.PrintBuffer(data);


            stop.Stop();
            Console.WriteLine((double)stop.ElapsedMilliseconds / 1000);
            Console.WriteLine("-------------");

            Console.ReadLine();
            ClassicDebugger debugger = new ClassicDebugger();

            Holder<int> x = new Holder<int>("X", 0, 10);
            Holder<int> y = new Holder<int>("Y", 0, 10);
            Holder<int> index = new Holder<int>("i", 0, 10);
            Holder<int> left = new Holder<int>("left", 0, 10);
            Holder<int> top = new Holder<int>("top", 0, 10);
            Holder<int> w = new Holder<int>("with", 0, 10);
            Holder<int> h = new Holder<int>("height", 0, 10);

            HolderString s = new HolderString("---");

            debugger.Watcher.Add(x);
            debugger.Watcher.Add(y);
            debugger.Watcher.Add(index);
            debugger.Watcher.Add(left);
            debugger.Watcher.Add(top);
            debugger.Watcher.Add(w);
            debugger.Watcher.Add(h);
            debugger.Watcher.Add(s);

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

                s.Value = "".PadRight(w.value-1, '-');

                System.Threading.Thread.Sleep(50);
            }

            debugger.Abort();

            Console.ReadLine();
        }
    }
}
