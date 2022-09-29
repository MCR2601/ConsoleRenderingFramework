using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderingFramework.ConsoleSpeedUp
{
    /// <summary>
    /// Virtual Terminal Sequence Graphical Management Unit
    /// </summary>
    public class VTSGMU : GMU
    {
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        const char ESC = '\u001b';

        public VTSGMU(int w, int h)
        {
            height = h;
            width = w;

            CreateScreen();
            ScreenBuffer.Populate(new PInfo().SetC(' ').SetBg(ConsoleColor.Blue));

            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
            if(!GetConsoleMode(iStdOut,out uint outConsoleMode))
            {
                Console.WriteLine("failed to get output console mode");
                throw new Exception("failed to get output console mode");
            }

            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
            if (!SetConsoleMode(iStdOut, outConsoleMode))
            {
                Console.WriteLine($"failed to set output console mode, error code: {GetLastError()}");
                throw new Exception($"failed to set output console mode, error code: {GetLastError()}");
            }
            Console.WriteLine("\u001b[31mGMU was Setup correctly!\u001b[0m");
        }

        public override void PrintFrame()
        {
            base.PrintFrame();
        }
        public bool PrintBuffer(DirectConsoleAccess.CharInfo[] buffer,int width, int height)
        {
            StringBuilder sb = new StringBuilder();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            int cf = 37;
            int cb = 40;
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);

            int nextF = 37;
            int nextB = 40;


            for (int i = 0; i < buffer.Length; i++)
            {
                if (i % width == 0)
                {
                    sb.Append(ESC + "[" + 1 + "E");
                }

                nextF = GetCodeForeground(buffer[i].Attributes);
                nextB = GetCodeBackground(buffer[i].Attributes);

                if(nextF!= cf)
                {
                    sb.Append(ESC + "[" + nextF + "m");
                }
                if(nextB != cb)
                {
                    sb.Append(ESC + "[" + nextB + "m");
                }

                sb.Append(buffer[i].Char.UnicodeChar);

                cf = nextF;
                cb = nextB;                
            }
            Console.Write(sb.ToString());
            return true;
        }

        private int GetCodeForeground(short attribute)
        {
            int v = attribute & 0xf;
            switch (v)
            {
                case 0x0:
                    return 30;
                case 0x1:
                    return 34;
                case 0x2:
                    return 32;
                case 0x3:
                    return 36;
                case 0x4:
                    return 31;
                case 0x5:
                    return 35;
                case 0x6:
                    return 33;
                case 0x7:
                    return 37;
                case 0x8:
                    return 90;
                case 0x9:
                    return 94;
                case 0xa:
                    return 92;
                case 0xb:
                    return 96;
                case 0xc:
                    return 91;
                case 0xd:
                    return 95;
                case 0xe:
                    return 93;
                case 0xf:
                    return 97;
                default: return 0;
            }
        }
        private int GetCodeBackground(short attribute)
        {
            int v = (attribute & 0xf0)>>4;
            switch (v)
            {
                case 0x0:
                    return 40;
                case 0x1:
                    return 44;
                case 0x2:
                    return 42;
                case 0x3:
                    return 46;
                case 0x4:
                    return 41;
                case 0x5:
                    return 45;
                case 0x6:
                    return 43;
                case 0x7:
                    return 47;
                case 0x8:
                    return 100;
                case 0x9:
                    return 104;
                case 0xa:
                    return 102;
                case 0xb:
                    return 106;
                case 0xc:
                    return 101;
                case 0xd:
                    return 105;
                case 0xe:
                    return 103;
                case 0xf:
                    return 107;
                default: return 0;
            }
        }
    }
}
