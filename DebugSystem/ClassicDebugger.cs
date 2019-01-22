using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderingFramework.Debug
{
    /// <summary>
    /// A Debugger for the regular console. It will output the state of variables at a set rate
    /// </summary>
    public class ClassicDebugger
    {
        public List<HolderT> Watcher = new List<HolderT>();

        /// <summary>
        /// When the debugger gets updated in MiliSec (1000 = 1 Sec)
        /// </summary>
        /// <remarks>
        /// too low number may make problems 
        /// </remarks>
        public int UpdateTime = 1000;

        private bool keepRunning = false;

        public void PrintData()
        {
            foreach (var item in Watcher)
            {
                item.Print();
            }
        }

        private void Run()
        {
            keepRunning = true;
            Console.Clear();
            PrintData();
            while (keepRunning)
            {
                System.Threading.Thread.Sleep(UpdateTime);
                Console.Clear();
                PrintData();
            }
        }

        public async void Activate()
        {
            await Task.Run(()=>Run());
        }

        public void Abort()
        {
            keepRunning = false;
        }


    }
}
