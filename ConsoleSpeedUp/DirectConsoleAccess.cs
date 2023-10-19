using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using System.IO;
using ConsoleRenderingFramework;
using System.Diagnostics;
using System.ComponentModel;

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

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateConsoleScreenBuffer(
            UInt32 dwDesiredAccess,
            UInt32 dwShareMode,
            IntPtr secutiryAttributes,
            UInt32 flags,
            IntPtr screenBufferData
            );

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool SetConsoleActiveScreenBuffer(
            IntPtr Handle
            );


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

        SmallRect _WriteRegion;


        private int bufferCount = 1; // number of total buffers used 
        private List<IntPtr> bufferHandles; // all buffers currently in use
        private List<CharInfo[]> bufferBuffers; // all the character info per used buffer
        private int nextBuffer; // the index of the next buffer used (next: (nextBuffer + 1) % bufferCount
        private int latestFrame; // latest Frame which was dispatched (Frame number)
        private int latestPrintFrame; // latest Frame which was set Active (Frame number)


        public DirectConsoleAccess(int width,int height,int offsetX,int offsetY)
        {
            OutHandle = GetSTD_OUT();
            _BufferWidth = width;
            _BufferHeight = height;

            bufferHandles = new List<IntPtr>();
            bufferHandles.Add(OutHandle);
            bufferBuffers = new List<CharInfo[]>();

            nextBuffer = 0;
            latestFrame = 0;
            latestPrintFrame = 0;

            bufferBuffers.Add(new CharInfo[_BufferWidth * _BufferHeight]);
            _WriteRegion = new SmallRect() { Left = (short)offsetX, Top = (short)offsetY, Right = (short)_BufferWidth, Bottom = (short)_BufferHeight };
        }

        public void AddAdditonalBuffer()
        {
            IntPtr handle = CreateConsoleScreenBuffer(0x80000000 | 0x40000000, 1 | 2, IntPtr.Zero, 1, IntPtr.Zero);
            if(handle.ToInt64() == -1)
            {
                throw new Exception("Was not able to create new console screen buffer");
            }

            bufferHandles.Add(handle);
            bufferBuffers.Add(new CharInfo[_BufferWidth * _BufferHeight]);
            bufferCount++;
        }

        public static CharInfo ConvertToInfo(PInfo data)
        {
            CharInfo info = new CharInfo();
            info.Char.UnicodeChar = data.Character;
            info.Attributes = ColorToAttribute(data);
            return info;
        }

        public async Task<bool> PrintBuffer(PInfo[,] data)
        {
            if (data.GetLength(0) != _BufferWidth || data.GetLength(1) != _BufferHeight)
            {
                // we dont have the right format
                return false;
            }


            if (bufferCount == 1)
            {
                // do not bother with async

                for (int x = 0; x < _BufferWidth; x++)
                {
                    for (int y = 0; y < _BufferHeight; y++)
                    {
                        bufferBuffers[0][x + y * _BufferWidth].Char.UnicodeChar = data[x, y].Character;
                        bufferBuffers[0][x + y * _BufferWidth].Attributes = ColorToAttribute(data[x, y]);
                    }
                }

                bool b = WriteConsoleOutput(bufferHandles[0], bufferBuffers[0],
                new Coord() { X = (short)_BufferWidth, Y = (short)_BufferHeight },
                new Coord() { X = 0, Y = 0 },
                ref _WriteRegion);
                return b;

            }
            else
            {
                // do async work

                int myFrame = ++latestFrame;
                IntPtr handle = bufferHandles[nextBuffer];
                CharInfo[] bufferContent = bufferBuffers[nextBuffer];
                nextBuffer = (nextBuffer + 1) % bufferCount;

                for (int x = 0; x < _BufferWidth; x++)
                {
                    for (int y = 0; y < _BufferHeight; y++)
                    {
                        bufferContent[x + y * _BufferWidth].Char.UnicodeChar = data[x, y].Character;
                        bufferContent[x + y * _BufferWidth].Attributes = ColorToAttribute(data[x, y]);
                    }
                }

                bool b = false;


                await Task.Run(() =>
                {
                    lock (bufferContent)
                    {
                        b = WriteConsoleOutput(handle, bufferContent,
                            new Coord() { X = (short)_BufferWidth, Y = (short)_BufferHeight },
                            new Coord() { X = 0, Y = 0 },
                            ref _WriteRegion);
                    }                    
                });

                if (latestPrintFrame < myFrame && b)
                {
                    SetConsoleActiveScreenBuffer(handle);
                    latestPrintFrame = myFrame;
                }
                else
                {
                }

                if (!b)
                {
                    Win32Exception ex = new Win32Exception();
                    string errMsg = ex.Message;
                    b = false;
                }

                return b;
            }
        }

        public async Task<bool> PrintBuffer(CharInfo[] buffer,int width,int height)
        {
            if (bufferCount == 1)
            {
                // do not bother with async

                bool b = WriteConsoleOutput(bufferHandles[0], buffer,
                new Coord() { X = (short)width, Y = (short)height },
                new Coord() { X = 0, Y = 0 },
                ref _WriteRegion);
                return b;

            }
            else
            {
                // do async work

                int myFrame = ++latestFrame;
                IntPtr handle = bufferHandles[nextBuffer];
                nextBuffer = (nextBuffer + 1) % bufferCount;

                bool b = false;

                await Task.Run(() =>
                {
                    b = WriteConsoleOutput(handle, buffer,
                    new Coord() { X = (short)width, Y = (short)height },
                    new Coord() { X = 0, Y = 0 },
                    ref _WriteRegion);
                });

                if(latestPrintFrame < myFrame && b)
                {
                    SetConsoleActiveScreenBuffer(handle);
                    latestPrintFrame = myFrame;
                }
                
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
