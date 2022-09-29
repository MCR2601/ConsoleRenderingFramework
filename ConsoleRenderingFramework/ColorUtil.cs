using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderingFramework
{
    public static class ColorUtil
    {

        static PInfo[,,] rgbMap;

        const string COLOR_FILE_NAME = "Color.col";


        private struct RGB
        {
            public int Red;
            public int Green;
            public int Blue;

            public RGB(int red, int green, int blue)
            {
                Red = red;
                Green = green;
                Blue = blue;
            }
            public static RGB Between(RGB c1, RGB c2,double amount)
            {
                return new RGB(
                    (int)(c1.Red * amount + c2.Red* (1 - amount)),
                    (int)(c1.Green * amount + c2.Green * (1 - amount)),
                    (int)(c1.Blue * amount + c2.Blue * (1 - amount)));
            }
            public static int Difference(RGB c1, RGB c2)
            {
                //return Math.Abs(c1.Red - c2.Red) + Math.Abs(c1.Green - c2.Green) + Math.Abs(c1.Blue - c2.Blue);
                return (int)Math.Sqrt(Math.Pow(c1.Red-c2.Red,2)+ Math.Pow(c1.Green - c2.Green, 2)+ Math.Pow(c1.Blue - c2.Blue, 2));
            }
        }

        private class Mapping
        {
            public PInfo Info;
            public RGB Color;

            public Mapping(PInfo info, RGB color)
            {
                Info = info;
                Color = color;
            }
        }
        private static Dictionary<ConsoleColor, RGB> BaseColor = new Dictionary<ConsoleColor, RGB>()
        {
            {ConsoleColor.Red,new RGB(255,0,0) },
            {ConsoleColor.DarkRed,new RGB(128,0,0) },
            {ConsoleColor.White,new RGB(255,255,255) },
            {ConsoleColor.Black,new RGB(0,0,0) },
            {ConsoleColor.Gray,new RGB(192,192,192) },
            {ConsoleColor.DarkGray,new RGB(128,128,128) },
            {ConsoleColor.Blue,new RGB(0,0,255 ) },
            {ConsoleColor.DarkBlue,new RGB(0,0,128) },
            {ConsoleColor.Cyan,new RGB(0,255,255) },
            {ConsoleColor.DarkCyan,new RGB(0,128,128) },
            {ConsoleColor.Magenta,new RGB(255,0,255) },
            {ConsoleColor.DarkMagenta,new RGB(128,0,128 ) },
            {ConsoleColor.Green,new RGB(0,255,0 ) },
            {ConsoleColor.DarkGreen,new RGB(0,128,0 ) },
            {ConsoleColor.Yellow,new RGB(255,255,0) },
            {ConsoleColor.DarkYellow,new RGB(128,128,0 ) }
        };
        private static List<Mapping> baseValues;
        private static List<ColorPair> baseCalculations = new List<ColorPair>()
        {
            new ColorPair(){c1 = ConsoleColor.Black,c2 = ConsoleColor.DarkRed,q1 = false,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Black,c2 = ConsoleColor.DarkGreen,q1 = false,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Black,c2 = ConsoleColor.DarkBlue,q1 = false,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Black,c2 = ConsoleColor.DarkCyan,q1 = false,q2 = true,q3 = true, q4 = true, q5 = true },
            new ColorPair(){c1 = ConsoleColor.Black,c2 = ConsoleColor.DarkYellow,q1 = false,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Black,c2 = ConsoleColor.DarkMagenta,q1 = false,q2 = true,q3 = true, q4 = true, q5 = true},
            
            new ColorPair(){c1 = ConsoleColor.Black,c2 = ConsoleColor.DarkGray,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},


            new ColorPair(){c1 = ConsoleColor.DarkBlue,c2 = ConsoleColor.Blue,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkBlue,c2 = ConsoleColor.DarkMagenta,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkBlue,c2 = ConsoleColor.DarkCyan,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},

            new ColorPair(){c1 = ConsoleColor.DarkRed,c2 = ConsoleColor.Red,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkRed,c2 = ConsoleColor.DarkYellow,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkRed,c2 = ConsoleColor.DarkMagenta,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},

            new ColorPair(){c1 = ConsoleColor.DarkGreen,c2 = ConsoleColor.Green,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkGreen,c2 = ConsoleColor.DarkCyan,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkGreen,c2 = ConsoleColor.DarkYellow,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},

            new ColorPair(){c1 = ConsoleColor.DarkMagenta,c2 = ConsoleColor.Magenta,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkYellow, c2 = ConsoleColor.Yellow,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkCyan,c2 = ConsoleColor.Cyan,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},

            new ColorPair(){c1 = ConsoleColor.DarkGray,c2 = ConsoleColor.Gray,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},

            new ColorPair(){c1 = ConsoleColor.DarkGray,c2 = ConsoleColor.DarkYellow,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkGray,c2 = ConsoleColor.DarkCyan,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkGray,c2 = ConsoleColor.DarkMagenta,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},

            new ColorPair(){c1 = ConsoleColor.DarkGray,c2 = ConsoleColor.Yellow,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkGray,c2 = ConsoleColor.Magenta,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkGray,c2 = ConsoleColor.Cyan,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},

            new ColorPair(){c1 = ConsoleColor.DarkGray,c2 = ConsoleColor.DarkBlue,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkGray,c2 = ConsoleColor.DarkGreen,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkGray,c2 = ConsoleColor.DarkRed,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},

            new ColorPair(){c1 = ConsoleColor.DarkGray,c2 = ConsoleColor.Blue,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkGray,c2 = ConsoleColor.Red,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkGray,c2 = ConsoleColor.Green,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},


            new ColorPair(){c1 = ConsoleColor.Gray,c2 = ConsoleColor.White,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Gray,c2 = ConsoleColor.Yellow,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Gray,c2 = ConsoleColor.Red,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Gray,c2 = ConsoleColor.Magenta,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Gray,c2 = ConsoleColor.Blue,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Gray,c2 = ConsoleColor.Green,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Gray,c2 = ConsoleColor.Cyan,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},


            new ColorPair(){c1 = ConsoleColor.Red,c2 = ConsoleColor.Yellow,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Red,c2 = ConsoleColor.White,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Red,c2 = ConsoleColor.Magenta,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},

            new ColorPair(){c1 = ConsoleColor.Blue,c2 = ConsoleColor.White,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Blue,c2 = ConsoleColor.Magenta,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Blue,c2 = ConsoleColor.Cyan,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},

            new ColorPair(){c1 = ConsoleColor.Green,c2 = ConsoleColor.White,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Green,c2 = ConsoleColor.Yellow,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Green,c2 = ConsoleColor.Cyan,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},


            new ColorPair(){c1 = ConsoleColor.DarkBlue,c2 = ConsoleColor.DarkGreen,q1 = true,q2 = true,q3 = false, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkCyan,c2 = ConsoleColor.DarkMagenta,q1 = true,q2 = true,q3 = false, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkYellow,c2 = ConsoleColor.DarkMagenta,q1 = true,q2 = true,q3 = false, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkRed,c2 = ConsoleColor.DarkGreen,q1 = true,q2 = true,q3 = false, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkRed,c2 = ConsoleColor.DarkBlue,q1 = true,q2 = true,q3 = false, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkYellow,c2 = ConsoleColor.DarkCyan,q1 = true,q2 = true,q3 = false, q4 = true, q5 = true},

            new ColorPair(){c1 = ConsoleColor.DarkRed,c2 = ConsoleColor.DarkCyan,q1 = true,q2 = true,q3 = false, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.DarkYellow,c2 = ConsoleColor.DarkBlue,q1 = true,q2 = true,q3 = false, q4 = true, q5 = true},
            
            
            new ColorPair(){c1 = ConsoleColor.Gray,c2 = ConsoleColor.Green,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Gray,c2 = ConsoleColor.Blue,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Gray,c2 = ConsoleColor.Red,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Gray,c2 = ConsoleColor.Yellow,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Gray,c2 = ConsoleColor.Cyan,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Gray,c2 = ConsoleColor.Magenta,q1 = true,q2 = true,q3 = true, q4 = true, q5 = true},


            new ColorPair(){c1 = ConsoleColor.Yellow,c2 = ConsoleColor.Magenta,q1 = true,q2 = true,q3 = false, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Yellow,c2 = ConsoleColor.Cyan,q1 = true,q2 = true,q3 = false, q4 = true, q5 = true},
            new ColorPair(){c1 = ConsoleColor.Magenta,c2 = ConsoleColor.Cyan,q1 = true,q2 = true,q3 = false, q4 = true, q5 = true},


        
        };
        struct ColorPair
        {
            public ConsoleColor c1;
            public ConsoleColor c2;
            public bool q1;
            public bool q2;
            public bool q3;
            public bool q4;
            public bool q5;
        }
        public static void Initialize()
        {
            if (rgbMap == null)
            {
                if (TryReadColorFile())
                {
                    Console.WriteLine("Colors read from file");
                }
                else
                {
                    rgbMap = new PInfo[256, 256, 256];

                    baseValues = new List<Mapping>();
                    for (int i = 0; i < 16; i++)
                    {
                        ConsoleColor col1 = (ConsoleColor)i;
                        baseValues.Add(new Mapping(new PInfo(' ', ConsoleColor.White, col1), BaseColor[col1]));
                        for (int x = i + 1; x < 16; x++)
                        {
                            ConsoleColor col2 = (ConsoleColor)x;
                            baseValues.Add(new Mapping(new PInfo('░', col1, col2), RGB.Between(BaseColor[col1], BaseColor[col2], 0.14)));
                            baseValues.Add(new Mapping(new PInfo('▒', col1, col2), RGB.Between(BaseColor[col1], BaseColor[col2], 0.25)));
                            baseValues.Add(new Mapping(new PInfo('▓', col1, col2), RGB.Between(BaseColor[col1], BaseColor[col2], 0.5)));
                            baseValues.Add(new Mapping(new PInfo('▒', col2, col1), RGB.Between(BaseColor[col1], BaseColor[col2], 0.75)));
                            baseValues.Add(new Mapping(new PInfo('░', col2, col1), RGB.Between(BaseColor[col1], BaseColor[col2], 0.86)));
                        }
                    }
                    /*
                    foreach (var item in baseCalculations)
                    {
                        if(item.q1)
                            baseValues.Add(new Mapping(new PInfo('░', item.c1, item.c2), RGB.Between(BaseColor[item.c1], BaseColor[item.c2], 0.14)));
                        if (item.q2)
                            baseValues.Add(new Mapping(new PInfo('▒', item.c1, item.c2), RGB.Between(BaseColor[item.c1], BaseColor[item.c2], 0.25)));
                        if (item.q3)
                            baseValues.Add(new Mapping(new PInfo('▓', item.c1, item.c2), RGB.Between(BaseColor[item.c1], BaseColor[item.c2], 0.5)));
                        if (item.q4)
                            baseValues.Add(new Mapping(new PInfo('▒', item.c2, item.c1), RGB.Between(BaseColor[item.c1], BaseColor[item.c2], 0.75)));
                        if (item.q5)
                            baseValues.Add(new Mapping(new PInfo('░', item.c2, item.c1), RGB.Between(BaseColor[item.c1], BaseColor[item.c2], 0.86)));
                    }
                    */

                    for (int i = 0; i < baseValues.Count; i++)
                    {
                        Mapping m = baseValues[i];
                        m.Info.PrintPixel();
                        rgbMap[m.Color.Red, m.Color.Green, m.Color.Blue] = m.Info;
                    }

                    List<Task> tasks = new List<Task>();
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    for (int r = 0; r < 256; r++)
                    {
                        int cr = r;

                        tasks.Add(Task.Run(() =>
                        {
                            int ccr = cr;
                            for (int g = 0; g < 256; g++)
                            {
                                for (int b = 0; b < 256; b++)
                                {
                                    GetRepresentation(ccr, g, b);
                                }
                            }
                            Console.WriteLine("R: " + ccr + " Done");
                        }));
                    }
                    Task.WaitAll(tasks.ToArray());
                    Console.WriteLine("Calculations complete for color");
                    WriteColors();
                    Console.WriteLine("Colors written to file");
                }    
            }
        }

        private static bool TryReadColorFile()
        {
            if (File.Exists(COLOR_FILE_NAME))
            {
                rgbMap = new PInfo[256, 256, 256];

                // 
                using (FileStream stream = File.Open(COLOR_FILE_NAME, FileMode.Open, FileAccess.Read))
                {
                    using (var buf = new BufferedStream(stream))
                    {
                        byte[] data = new byte[10];
                        for (int r = 0; r < 256; r++)
                        {
                            for (int g = 0; g < 256; g++)
                            {
                                for (int b = 0; b < 256; b++)
                                {
                                    if(buf.Read(data,0,10) < 10)
                                    {
                                        throw new InvalidDataException("There was an error loading colors from file");
                                    }
                                    PInfo info = new PInfo();
                                    // bg fg char
                                    info.SetBg((ConsoleColor)BitConverter.ToInt32(data, 0));
                                    info.SetFg((ConsoleColor)BitConverter.ToInt32(data, 4));
                                    info.SetC(BitConverter.ToChar(data, 8));
                                    rgbMap[r, g, b] = info;
                                }
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }
        private static void WriteColors()
        {
            if(rgbMap == null)
            {
                return;
            }

            using (FileStream stream = File.Open(COLOR_FILE_NAME, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var buf = new BufferedStream(stream))
                {
                    PInfo info;
                    for (int r = 0; r < 256; r++)
                    {
                        for (int g = 0; g < 256; g++)
                        {
                            for (int b = 0; b < 256; b++)
                            {
                                info = rgbMap[r,g,b];
                                // bg fg char
                                byte[] bg = BitConverter.GetBytes((int)info.Background);
                                byte[] fg = BitConverter.GetBytes((int)info.Foreground);
                                byte[] c = BitConverter.GetBytes(info.Character);
                                buf.Write(bg, 0, bg.Length);
                                buf.Write(fg, 0, fg.Length);
                                buf.Write(c, 0, c.Length);
                            }
                        }
                    }
                    buf.Flush();
                }
            }
        }


        public static PInfo GetRepresentation(int r,int g,int b)
        {
            if(rgbMap[r,g,b] == null)
            {
                RGB c = new RGB(r, g, b);
                //Mapping min = baseValues.Aggregate((currMin, m) => (currMin == null || (RGB.Difference(currMin.Color, c) > RGB.Difference(m.Color, c))) ? m : currMin);

                Mapping currBest = baseValues[0];
                int colDiff = RGB.Difference(currBest.Color, c);
                for (int i = 1; i < baseValues.Count; i++)
                {
                    int nextDiff = RGB.Difference(baseValues[i].Color, c);
                    if (nextDiff < colDiff)
                    {
                        currBest = baseValues[i];
                        colDiff = nextDiff;
                    }
                }
                
                rgbMap[r, g, b] = currBest.Info.Copy();
                return currBest.Info.Copy();
            }
            return rgbMap[r, g, b].Copy();
        }

    }
}
