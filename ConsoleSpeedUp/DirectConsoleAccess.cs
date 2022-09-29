using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using System.IO;
using ConsoleRenderingFramework;

namespace ConsoleRenderingFramework.ConsoleSpeedUp
{
    // This has been taken from stack overflow (https://stackoverflow.com/questions/2754518/how-can-i-write-fast-colored-output-to-console)

    public class DirectConsoleAccess
    {
        #region Imports
        [DllImport("kernel32.dll",SetLastError =true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true,CharSet = CharSet.Unicode)]
        static extern bool WriteConsoleOutput(
            IntPtr hConsoleOutput,
            CharInfo[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion);

        
        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };
        [StructLayout(LayoutKind.Explicit,CharSet=CharSet.Unicode)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
        #endregion

        private IntPtr OutHandle;

        private int _BufferWidth;
        private int _BufferHeight;

        private CharInfo[] _Buffer;

        SmallRect _WriteRegion;


        bool doubleRegion = false;
        SmallRect _writeRegion2;
        int xPos1 = 0;
        int xPos2 = 0;
        bool currentRegion = true;

        private bool EnableDounbleBuffering = false;
        private IntPtr ShownHandle;
        private IntPtr WriteHandle;

        public DirectConsoleAccess(int width,int height,int offsetX,int offsetY)
        {
            OutHandle = GetSTD_OUT();
            _BufferWidth = width;
            _BufferHeight = height;

            _Buffer = new CharInfo[_BufferWidth * _BufferHeight];
            _WriteRegion = new SmallRect() {Left = (short)offsetX, Top = (short)offsetY, Right = (short)_BufferWidth, Bottom = (short)_BufferHeight };
        }

        public void EnableDoubleRegion(int xPos, int left,int top, int right,int bottom)
        {
            _writeRegion2 = new SmallRect() { Left = (short)left, Top = (short)top, Right = (short)right, Bottom = (short)bottom};
            doubleRegion = true;
            xPos2 = xPos;
        }

        public static CharInfo ConvertToInfo(PInfo data)
        {
            CharInfo info = new CharInfo();
            info.Char.UnicodeChar = data.Character;
            info.Attributes = ColorToAttribute(data);
            return info;
        }

        public bool PrintBuffer(PInfo[,] data)
        {
            if (data.GetLength(0) != _BufferWidth || data.GetLength(1) != _BufferHeight)
            {
                // we dont have the right format
                return false;
            }

            for (int x = 0; x < _BufferWidth; x++)
            {
                for (int y = 0; y < _BufferHeight; y++)
                {
                    _Buffer[x + y * _BufferWidth].Char.UnicodeChar = data[x, y].Character;
                    _Buffer[x + y * _BufferWidth].Attributes = ColorToAttribute(data[x, y]);
                }
            }

            //SmallRect rect = new SmallRect() {Left = 0, Top = 0, Right = (short)_BufferWidth, Bottom = (short)_BufferHeight};

            bool b = WriteConsoleOutput(OutHandle, _Buffer,
                new Coord() {X = (short) _BufferWidth, Y = (short) _BufferHeight},
                new Coord() {X = 0, Y = 0},
                ref _WriteRegion);
            return b;
        }

        public bool PrintBuffer(CharInfo[] buffer,int width,int height)
        {
            if (doubleRegion)
            {
                if (currentRegion)
                {

                    bool b = WriteConsoleOutput(OutHandle, buffer,
                        new Coord() { X = (short)width, Y = (short)height },
                        new Coord() { X = 0, Y = 0 },
                        ref _WriteRegion);
                    Console.SetWindowPosition(xPos1, 0);

                    return b;
                }
                else
                {

                    bool b = WriteConsoleOutput(OutHandle, buffer,
                        new Coord() { X = (short)width, Y = (short)height },
                        new Coord() { X = 0, Y = 0 },
                        ref _writeRegion2);
                    Console.SetWindowPosition(xPos2, 0);

                    return b;
                }
            }
            else
            {
                bool b = WriteConsoleOutput(OutHandle, buffer,
                 new Coord() { X = (short)width, Y = (short)height },
                 new Coord() { X = 0, Y = 0 },
                 ref _WriteRegion);
                return b;
            }   
            
        }

        

        public void TestOutput()
        {
            CharInfo[] buf = new CharInfo[80 * 25];
            SmallRect rect = new SmallRect() { Left = 0, Top = 0, Right = 80, Bottom = 25 };

            for (byte character = 65; character < 65 + 26; ++character)
            {
                
                    for (int i = 0; i < buf.Length; ++i)
                    {
                        buf[i].Attributes = ColorToAttribute(new PInfo().SetBg( ConsoleColor.Red).SetFg( ConsoleColor.Yellow));
                        buf[i].Char.AsciiChar = CharToByte('a');
                    }

                    bool b = WriteConsoleOutput(OutHandle, buf,
                        new Coord() { X = 80, Y = 25 },
                        new Coord() { X = 0, Y = 0 },
                        ref rect);
                

                System.Threading.Thread.Sleep(100);
            }
        }

        public void TestPInfo()
        {
            PInfo[,] data = new PInfo[_BufferWidth, _BufferHeight];
            data.Populate(new PInfo().SetC('c').SetBg(ConsoleColor.Red).SetFg(ConsoleColor.Blue));
            
            PrintBuffer(data);
            
        }

        public static byte CharToByte(char c)
        {
            return (byte)c;
        }

        public static short ColorToAttribute(PInfo info)
        {
            int f = (int)info.Foreground;
            int b = (int)info.Background;
            return (short)(f | (b<<4));
        }



        public static IntPtr GetSTD_OUT()
        {
            return GetStdHandle(-11);
        }
        public static IntPtr GetSTD_INPUT()
        {
            return GetStdHandle(-10);
        }
        public static IntPtr GetSTD_ERROR()
        {
            return GetStdHandle(-12);
        }
    }

}
