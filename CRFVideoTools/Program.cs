using FFMediaToolkit.Decoding;
using FFMediaToolkit.Common;
using FFMediaToolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using ConsoleRenderingFramework.ConsoleSpeedUp;
using ConsoleRenderingFramework.BasicScreenManagerPackage;
using System.IO;
using ConsoleRenderingFramework;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using OpenCvSharp;
using System.IO.Compression;

namespace CRFVideoTools
{
    class Program
    {

        public static int width = 510;//+2
        public static int height = 135;//+2

        const string LOGFILE = "Logs\\log4.csv";
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.ReadLine();
            if (false)
            {
                Console.WriteLine("Waiting");

                Console.ReadLine();

                Console.WriteLine("Doing");

                //var res = ConvertBitmaps(@"C:\Users\Marc\Downloads\Video", @"D:\GitHub\ConsoleRenderingFramework\CRFVideoTools\bin\Debug\ChikaVideo");

                //SaveVideoInConsoleFormat("rickroll.mp4", "ChikaVideo/OpenCvVideoTest2.crf");
                SaveVideoInConsoleFormat("ChikaScaled.mp4", "ChikaVideo/chickaNewConverted.crf");
                /*
                SaveVideoFromBitmaps(
                    @"ImageVideo25fps",
                    @"ChikaVideo");
                */
                /*
                SaveVideoFromBitmaps(
                                    @"D:\GitHub\ConsoleRenderingFramework\CRFVideoTools\bin\Debug\ScaledFrames",
                                    @"D:\GitHub\ConsoleRenderingFramework\CRFVideoTools\bin\Debug\ChikaVideo2");
                */
                Console.WriteLine("Saving Done");
                Console.ReadLine();

            }
            else
            {
                Console.WriteLine("Waiting");
                Console.ReadLine();
                ShowVideo(@"Videos\OpenCvVideoTest2.crf");
                //ShowVideo(@"Videos\chickaNewConverted.crf");

            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        static void SaveVideoFromBitmaps(string folderPath, string destinationPath)
        {
            string[] res = Directory.GetFiles(folderPath);

            res = res.Where(l => int.TryParse(l.Split('\\').Last().Split('.')[0], out int o)).OrderBy((st) => int.Parse(st.Split('\\').Last().Split('.')[0])).ToArray();

            Bitmap first = new Bitmap(res[0]);

            ColorUtil.Initialize();


            

            byte[][] frames = new byte[res.Length][];

            Task[] t = new Task[res.Length];

            for (int i = 0; i < res.Length; i++)
            {
                int ci = i;
                
                
                frames[ci] = new byte[height * width*2];
                t[ci] = Task.Run(() =>
                {



                    Color c;
                    Bitmap map = new Bitmap(res[ci]);

                    //map = map.ImageSmooth();

                    //FastBitmap fastMap = new FastBitmap(map);

                    //fastMap.Lock();


                    int imageWidth = map.Width - 1;
                    int imageHeight = map.Height - 1;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int valueCount = 1;

                            double pX = (x / (double)width) * imageWidth;
                            double pY = (y / (double)height) * imageHeight;

                            double xFloor = Math.Floor(pX);
                            double xCeil = Math.Ceiling(pX);
                            double xDelta = Math.Abs(pX - Math.Round(pY));
                            double xPFloorDelta = Math.Abs(pX - xFloor);

                            double yFloor = Math.Floor(pY);
                            double yCeil = Math.Ceiling(pY);
                            double yDelta = Math.Abs(pY - Math.Round(pY));
                            double yPFloorDelta = Math.Abs(pY - yFloor);


                            if (xDelta < 0.01 && yDelta < 0.01)
                            {
                                c = map.GetPixel((int)Math.Round(pX), (int)Math.Round(pY));
                            }
                            else
                            {
                                if (xDelta < 0.01)
                                {
                                    Color c1 = map.GetPixel((int)Math.Round(pX), (int)yFloor);
                                    Color c2 = map.GetPixel((int)Math.Round(pX), (int)yCeil);

                                    c = Color.FromArgb(
                                            (int)(c1.R * (1 - yPFloorDelta) + c2.R * (yPFloorDelta)),
                                            (int)(c1.G * (1 - yPFloorDelta) + c2.G * (yPFloorDelta)),
                                            (int)(c1.B * (1 - yPFloorDelta) + c2.B * (yPFloorDelta))
                                        );
                                }
                                else
                                {
                                    if (yDelta < 0.01)
                                    {
                                        Color c3 = map.GetPixel((int)xFloor, (int)Math.Round(pY));
                                        Color c4 = map.GetPixel((int)xCeil, (int)Math.Round(pY));

                                        c = Color.FromArgb(
                                                (int)(c3.R * (1 - xPFloorDelta) + c4.R * (xPFloorDelta)),
                                                (int)(c3.G * (1 - xPFloorDelta) + c4.G * (xPFloorDelta)),
                                                (int)(c3.B * (1 - xPFloorDelta) + c4.B * (xPFloorDelta))
                                            );
                                    }
                                    else
                                    {
                                        Color c1 = map.GetPixel((int)xFloor, (int)yFloor);
                                        Color c2 = map.GetPixel((int)xFloor, (int)yCeil);
                                        Color c3 = map.GetPixel((int)xCeil, (int)yFloor);
                                        Color c4 = map.GetPixel((int)xCeil, (int)yCeil);

                                        double xf = (1 - xPFloorDelta);
                                        double xc = xPFloorDelta;
                                        double yf = (1 - yPFloorDelta);
                                        double yc = yPFloorDelta;

                                        int r = (int)(c1.R * xf * yf + c2.R * xf * yc + c3.R * xc * yf + c4.R * xc * yc);
                                        int g = (int)(c1.G * xf * yf + c2.G * xf * yc + c3.G * xc * yf + c4.G * xc * yc);
                                        int b = (int)(c1.B * xf * yf + c2.B * xf * yc + c3.B * xc * yf + c4.B * xc * yc);

                                        c = Color.FromArgb(r, g, b);
                                    }
                                }
                            }


                            //c = map.GetPixel((int)Math.Round(pX), (int)Math.Round(pY));


                            PInfo info = ColorUtil.GetRepresentation(c.R, c.G, c.B);
                            DirectConsoleAccess.CharInfo sinfo = DirectConsoleAccess.ConvertToInfo(info);
                            int ch = 0b1_0000_0000;
                            if (info.Character == ' ')
                                ch = 0b1000_1000;
                            if (info.Character == '░')
                                ch = 0b0100_0100;
                            if (info.Character == '▒')
                                ch = 0b0010_0010;
                            if (info.Character == '▓')
                                ch = 0b0001_0001;
                            if (ch == 0b1_0000_0000)
                            {
                                Console.WriteLine("There was a pixel with wrong things going on");
                                throw new Exception("Character was not correct");
                            }
                            frames[ci][2 * (x + y * width)] = (byte)(sinfo.Attributes);
                            frames[ci][2 * (x + y * width) + 1] = (byte)(ch);
                        }
                    }
                    //fastMap.Unlock();

                    Console.WriteLine(ci + " Done");

                });
            }
            Task.WaitAll(t);

            Console.WriteLine("Now Saving to file");

            using(FileStream fs = new FileStream(destinationPath + "\\video.crf",FileMode.OpenOrCreate,FileAccess.Write))
            {
                fs.Write(BitConverter.GetBytes(width), 0, 4);
                fs.Write(BitConverter.GetBytes(height), 0, 4);
                fs.Write(BitConverter.GetBytes(25), 0, 8);
                for (int i = 0; i < frames.Length; i++)
                {
                    fs.Write(frames[i], 0, frames[i].Length);
                }
                fs.Flush();
            }
            Console.WriteLine("Done Saving");

        }

        static DirectConsoleAccess.CharInfo[][] ConvertBitmaps(string folderPath, string destinationPath, int digits = 3)
        {
            string[] res = Directory.GetFiles(folderPath);

            res = res.Where(l => int.TryParse(l.Split('\\').Last().Split('.')[0], out int o)).OrderBy((st) => int.Parse(st.Split('\\').Last().Split('.')[0])).ToArray();

            Bitmap first = new Bitmap(res[0]);

            ColorUtil.Initialize();


            int imageWidth = first.Width - 1;
            int imageHeight = first.Height - 1;
            Color c;

            DirectConsoleAccess.CharInfo[][] frames = new DirectConsoleAccess.CharInfo[res.Length][];

            Task[] t = new Task[res.Length];

            for (int i = 0; i < res.Length; i++)
            {
                int ci = i;
                Bitmap map = new Bitmap(res[i]);
                frames[ci] = new DirectConsoleAccess.CharInfo[height * width];
                t[ci] = Task.Run(() =>
                {
                    
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            c = map.GetPixel((int)((x / (double)width) * imageWidth), (int)((y / (double)height) * imageHeight));

                            var info = ColorUtil.GetRepresentation(c.R, c.G, c.B);
                            var sinfo = DirectConsoleAccess.ConvertToInfo(info);
                            frames[ci][x + y * width] = sinfo;
                        }
                    }
                    Console.WriteLine(ci + " Done");
                });
            }
            Task.WaitAll(t);

            return frames;
        }
        
        
        static void SaveVideoInConsoleFormat(string sourceVideo, string destinationFile)
        {
            Console.WriteLine("Start Read");
            ColorUtil.Initialize(); 

            using (VideoCapture cap = new VideoCapture(sourceVideo))
            {
                using(FileStream fs = new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using(var o = new DeflateStream(fs, CompressionMode.Compress))
                    {
                        o.Write(BitConverter.GetBytes(width), 0, 4);
                        o.Write(BitConverter.GetBytes(height), 0, 4);
                        o.Write(BitConverter.GetBytes(cap.Fps), 0, 8);


                        Mat image = new Mat();

                        int w = cap.FrameWidth;
                        int h = cap.FrameHeight;

                        int frameCount = 0;

                        while (true)
                        {
                            cap.Read(image);
                            if (image.Empty())
                                break;
                            frameCount++;
                            Console.WriteLine("Frame: " + frameCount);

                            using (var mat = new Mat<Vec3b>(image))
                            {
                                MatIndexer<Vec3b> indexer = mat.GetIndexer();

                                Vec3b c;
                                for (int y = 0; y < height; y++)
                                {
                                    for (int x = 0; x < width;x++)
                                    {

                                        double pX = (x / (double)width) * w;
                                        double pY = (y / (double)height) * h;

                                        double xFloor = Math.Floor(pX);
                                        double xCeil = Math.Ceiling(pX);
                                        double xDelta = Math.Abs(pX - Math.Round(pY));
                                        double xPFloorDelta = Math.Abs(pX - xFloor);

                                        double yFloor = Math.Floor(pY);
                                        double yCeil = Math.Ceiling(pY);
                                        double yDelta = Math.Abs(pY - Math.Round(pY));
                                        double yPFloorDelta = Math.Abs(pY - yFloor);


                                        if (xDelta < 0.01 && yDelta < 0.01)
                                        {
                                            c = indexer[(int)Math.Round(pY),(int)Math.Round(pX)];
                                        }
                                        else
                                        {
                                            if (xDelta < 0.01)
                                            {
                                                Vec3b c1 = indexer[(int)yFloor, (int)Math.Round(pX)];
                                                Vec3b c2 = indexer[(int)yCeil, (int)Math.Round(pX)];


                                                c = new Vec3b(
                                                        (byte)(c1.Item0 * (1 - yPFloorDelta) + c2.Item0 * (yPFloorDelta)),
                                                        (byte)(c1.Item1 * (1 - yPFloorDelta) + c2.Item1 * (yPFloorDelta)),
                                                        (byte)(c1.Item2 * (1 - yPFloorDelta) + c2.Item2 * (yPFloorDelta))
                                                    );
                                            }
                                            else
                                            {
                                                if (yDelta < 0.01)
                                                {
                                                    Vec3b c3 = indexer[(int)Math.Round(pY), (int)xFloor];
                                                    Vec3b c4 = indexer[(int)Math.Round(pY), (int)xCeil];

                                                    c = new Vec3b(
                                                            (byte)(c3.Item0 * (1 - xPFloorDelta) + c4.Item0 * (xPFloorDelta)),
                                                            (byte)(c3.Item1 * (1 - xPFloorDelta) + c4.Item1 * (xPFloorDelta)),
                                                            (byte)(c3.Item2 * (1 - xPFloorDelta) + c4.Item2 * (xPFloorDelta))
                                                        );
                                                }
                                                else
                                                {
                                                    Vec3b c1 = indexer[(int)yFloor, (int)xFloor];
                                                    Vec3b c2 = indexer[(int)yCeil, (int)xFloor];
                                                    Vec3b c3 = indexer[(int)yFloor, (int)xCeil];
                                                    Vec3b c4 = indexer[(int)yCeil,(int)xCeil];

                                                    double xf = (1 - xPFloorDelta);
                                                    double xc = xPFloorDelta;
                                                    double yf = (1 - yPFloorDelta);
                                                    double yc = yPFloorDelta;

                                                    byte b = (byte)(c1.Item0 * xf * yf + c2.Item0 * xf * yc + c3.Item0 * xc * yf + c4.Item0 * xc * yc);
                                                    byte g = (byte)(c1.Item1 * xf * yf + c2.Item1 * xf * yc + c3.Item1 * xc * yf + c4.Item1 * xc * yc);
                                                    byte r = (byte)(c1.Item2 * xf * yf + c2.Item2 * xf * yc + c3.Item2 * xc * yf + c4.Item2 * xc * yc);

                                                    c = new Vec3b(b, g, r);
                                                }
                                            }
                                        }


                                        //c = map.GetPixel((int)Math.Round(pX), (int)Math.Round(pY));


                                        PInfo info = ColorUtil.GetRepresentation(c.Item2, c.Item1, c.Item0);
                                        DirectConsoleAccess.CharInfo sinfo = DirectConsoleAccess.ConvertToInfo(info);
                                        int ch = 0b1_0000_0000;
                                        if (info.Character == ' ')
                                            ch = 0b1000_1000;
                                        if (info.Character == '░')
                                            ch = 0b0100_0100;
                                        if (info.Character == '▒')
                                            ch = 0b0010_0010;
                                        if (info.Character == '▓')
                                            ch = 0b0001_0001;

                                        if (ch == 0b1_0000_0000)
                                        {
                                            Console.WriteLine("There was a pixel with wrong things going on");
                                            throw new Exception("Character was not correct");
                                        }
                                        o.Write(new byte[] { (byte)(sinfo.Attributes), (byte)(ch) }, 0, 2);
                                    }
                                }
                            }
                        }
                        image.Dispose();
                        o.Close();
                    }
                }         
            }
        }
        
        
        static async void ShowVideo(DirectConsoleAccess.CharInfo[][] frames)
        {

            Console.WriteLine("Init color");
            ColorUtil.Initialize();
            Console.WriteLine("Color Done");
            Console.WriteLine("Enter to start showing stuff");

                Console.ReadLine();

            FastGMU gmu = new FastGMU(width + 2, height + 2,1,1);

            

            MultiSplitScreenManager mssm = new MultiSplitScreenManager(gmu.PlacePixels, width, height);
            FullScreenManager screen = new FullScreenManager(width - 2, height - 2, null);

            mssm.AddScreen(screen, new System.Drawing.Rectangle(1, 1, width - 2, height - 2));
            //gmu.PrintFrame();   

            for (int i = 0; i < 2589; i++)
            {
                var back = await gmu.PrintBuffer(frames[i], width, height);
                if (!back)
                {
                    Console.WriteLine("PAnic");
                }
                //System.Threading.Thread.Sleep(25);
            }
        }

        static void ShowVideo(string dataFile)
        {

            if (!File.Exists(dataFile))
            {
                Console.WriteLine("File Not found");
                return;
            }

            using(FileStream s = File.OpenRead(dataFile))
            {
                using(DeflateStream stream = new DeflateStream(s, CompressionMode.Decompress))
                {
                    byte[] wP = new byte[4];
                    byte[] hP = new byte[4];
                    byte[] fP = new byte[8];
                    int n = stream.Read(wP, 0, 4);
                    if (n != 4)
                    {
                        throw new InvalidDataException("Not enough data in file");
                    }
                    n = stream.Read(hP, 0, 4);
                    if (n != 4)
                    {
                        throw new InvalidDataException("Not enough data in file");
                    }
                    n = stream.Read(fP, 0, 8);
                    if (n != 8)
                    {
                        throw new InvalidDataException("Not enough data in file");
                    }
                    int videoWidth = BitConverter.ToInt32(wP, 0);
                    int videoHeight = BitConverter.ToInt32(hP, 0);
                    double fps = BitConverter.ToDouble(fP, 0);


                    VTSGMU gmu = new VTSGMU(videoWidth + 2, videoHeight + 2);

                    //gmu.access.EnableDoubleRegion(width + 2 + 1, width + 2 + 2, 1, width + 2 + width + 1, height + 1);

                    MultiSplitScreenManager mssm = new MultiSplitScreenManager(gmu.PlacePixels, videoWidth, videoHeight);
                    FullScreenManager screen = new FullScreenManager(videoWidth - 2, videoHeight - 2, null);

                    mssm.AddScreen(screen, new System.Drawing.Rectangle(1, 1, videoWidth - 2, videoHeight - 2));
                    gmu.PrintFrame();

                    Console.WriteLine("Init color");
                    ColorUtil.Initialize();
                    Console.WriteLine("Color Done");

                    // colors are: a short 2Byte (onle second byte used)
                    // char is stored in first byte using the right most 2 bit
                    // char is one of 4, " ░▒▓"

                    int numberOfElements = (videoHeight * videoWidth) * 2;
                    int numberOfPixels = (videoHeight * videoWidth);
                    int bufferSize = (numberOfElements) * 10;

                    byte[] frameData = new byte[(numberOfElements)];

                    DirectConsoleAccess.CharInfo[] screenBuffer = new DirectConsoleAccess.CharInfo[numberOfPixels];

                    for (int i = 0; i < numberOfPixels; i++)
                    {
                        screenBuffer[i] = new DirectConsoleAccess.CharInfo();
                    }
                    Console.WriteLine("Buffers initialized");

                    using (var log = new StreamWriter(LOGFILE))
                    {
                        using (System.Media.SoundPlayer player = new System.Media.SoundPlayer("rick roll total.wav"))
                        {

                            log.WriteLine("FrameStartTime;LoadTime;PrintTime;FrameTime;FrameEndTime");

                            using (var buffer = new BufferedStream(stream, bufferSize))
                            {
                                Stopwatch watch = new Stopwatch();
                                long frameStart = 0;
                                long loadEnd = 0;
                                watch.Start();

                                int frameNumber = -1;
                                long expectedFrameTime = (long)((1/fps)*1000);
                                long lastFrameTime = 40;
                                long lastlastFrameTime = 40;

                                while (buffer.Read(frameData, 0, numberOfElements) == numberOfElements)
                                {
                                    frameNumber++;
                                    frameStart = watch.ElapsedMilliseconds;
                                    log.Write(frameStart + ";");

                                    //screenBuffer = new DirectConsoleAccess.CharInfo[numberOfPixels];

                                    for (int i = 0; i < numberOfPixels; i++)
                                    {

                                        screenBuffer[i] = new DirectConsoleAccess.CharInfo();
                                        if ((frameData[i * 2 + 1] & 0b1000_0000) != 0)
                                        {
                                            screenBuffer[i].Char.UnicodeChar = ' ';
                                        }
                                        if ((frameData[i * 2 + 1] & 0b0100_0000) != 0)
                                        {
                                            screenBuffer[i].Char.UnicodeChar = '░';
                                        }
                                        if ((frameData[i * 2 + 1] & 0b0010_0000) != 0)
                                        {
                                            screenBuffer[i].Char.UnicodeChar = '▒';
                                        }
                                        if ((frameData[i * 2 + 1] & 0b0001_0000) != 0)
                                        {
                                            screenBuffer[i].Char.UnicodeChar = '▓';
                                        }
                                        screenBuffer[i].Attributes = (short)(frameData[i * 2]);
                                    }
                                    loadEnd = watch.ElapsedMilliseconds;
                                    if (
                                        frameNumber * expectedFrameTime +
                                            (lastFrameTime + lastlastFrameTime) / 2
                                                > watch.ElapsedMilliseconds)
                                    {
                                        gmu.PrintBuffer(screenBuffer, videoWidth, videoHeight);
                                        lastlastFrameTime = lastFrameTime;
                                        lastFrameTime = watch.ElapsedMilliseconds - loadEnd;
                                    }

                                    //Task.Run(() =>
                                    //{
                                    //    gmu.PrintBuffer(screenBuffer, videoWidth, videoHeight);
                                    //});
                                    log.Write(loadEnd - frameStart + ";");

                                    if (frameNumber == 0)
                                    {
                                        //player.Play();
                                    }

                                    log.WriteLine(
                                        (watch.ElapsedMilliseconds - loadEnd) + ";" +
                                        (watch.ElapsedMilliseconds - frameStart) + ";" +
                                        watch.ElapsedMilliseconds);
                                    //System.Threading.Thread.Sleep(140);
                                    //if (Console.KeyAvailable)
                                    //{
                                    //    Console.WriteLine("Pause");
                                    //    Console.ReadLine();
                                    //}
                                }
                                Console.WriteLine("End of Video show");
                                log.WriteLine("-- TOTAL TIME " + watch.Elapsed.ToString() + " --");
                                log.WriteLine("-- END OF LOG --");
                            }
                        }
                    }
                }                
            }            
        }
        
    }


}
