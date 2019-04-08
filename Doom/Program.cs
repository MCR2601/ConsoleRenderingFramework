﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;
using System.Diagnostics;
using System.Windows.Forms;
using ConsoleRenderingFramework.BasicScreenManagerPackage;

namespace Doom
{
    class Program
    {
        const double Speed = 0.3;
        static int height = 60;
        static int width = 120;
        const double rotation = 10 * Math.PI / 180;

        const bool inConsole = true;

        

        static void Main(string[] args)
        {
            Console.ReadLine();
            //height = Console.LargestWindowHeight-1;
            //width = Console.LargestWindowWidth-1;

            if (inConsole)
            {
                StartGMU();
            }
            else
            {
                StartGMUF();
            }


            



        }
        public static void MoveFront(DoomScreenManager manager)
        {
            double angle = Math.Atan2(manager.Player.y_2 - manager.Player.y_1, manager.Player.x_2 - manager.Player.x_1);

            double xDif = Math.Cos(angle ) * Speed;
            double yDif = Math.Sin(angle ) * Speed;

            manager.Player.x_1 += xDif;
            manager.Player.x_2 += xDif;

            manager.Player.y_1 += yDif;
            manager.Player.y_2 += yDif;

        }
        public static void MoveRight(DoomScreenManager manager)
        {
            double angle = Math.Atan2(manager.Player.y_2 - manager.Player.y_1, manager.Player.x_2 - manager.Player.x_1);

            double xDif = Math.Cos(angle + (90 * Math.PI / 180)) * Speed;
            double yDif = Math.Sin(angle + (90 * Math.PI / 180)) * Speed;

            manager.Player.x_1 += xDif;
            manager.Player.x_2 += xDif;

            manager.Player.y_1 += yDif;
            manager.Player.y_2 += yDif;
        }
        public static void MoveBack(DoomScreenManager manager)
        {
            double angle = Math.Atan2(manager.Player.y_2 - manager.Player.y_1, manager.Player.x_2 - manager.Player.x_1);

            double xDif = Math.Cos(angle + (180 * Math.PI / 180)) * Speed;
            double yDif = Math.Sin(angle + (180 * Math.PI / 180)) * Speed;

            manager.Player.x_1 += xDif;
            manager.Player.x_2 += xDif;

            manager.Player.y_1 += yDif;
            manager.Player.y_2 += yDif;

        }
        public static void MoveLeft(DoomScreenManager manager)
        {
            double angle = Math.Atan2(manager.Player.y_2 - manager.Player.y_1, manager.Player.x_2 - manager.Player.x_1);

            double xDif = Math.Cos(angle - (90 * Math.PI / 180)) * Speed;
            double yDif = Math.Sin(angle - (90 * Math.PI / 180)) * Speed;

            manager.Player.x_1 += xDif;
            manager.Player.x_2 += xDif;

            manager.Player.y_1 += yDif;
            manager.Player.y_2 += yDif;
        }
        public static void StartGMU()
        {
            GMU gmu = new GMU(width, height);
            MultiSplitScreenManager sm = new MultiSplitScreenManager(gmu.PlacePixels, height, width);
            // use this for rendering
            FullScreenManager rs = new FullScreenManager(width - 2, height - 2, null);

            DoomScreenManager doom = new DoomScreenManager(null, width - 2, height - 2);

            sm.AddScreen(doom, new System.Drawing.Rectangle(1, 1, width - 2, height - 2));

            doom.Render();
            gmu.PrintFrame();

            ConsoleKey input;
            bool sneak = false;

            while ((input = Console.ReadKey(true).Key) != ConsoleKey.Spacebar)
            {

                double currRotation = Math.Atan2(doom.Player.y_2 - doom.Player.y_1, doom.Player.x_2 - doom.Player.x_1);
                switch (input)
                {
                    case ConsoleKey.C:
                        if (sneak)
                        {
                            doom.PlayerHeight = 1;
                            sneak = false;
                        }
                        else
                        {
                            doom.PlayerHeight = 0.5;
                            sneak = true;
                        }
                        break;
                    case ConsoleKey.A:
                        MoveLeft(doom);
                        break;
                    case ConsoleKey.S:
                        MoveBack(doom);
                        break;
                    case ConsoleKey.D:
                        MoveRight(doom);
                        break;
                    case ConsoleKey.W:
                        MoveFront(doom);
                        break;
                    case ConsoleKey.RightArrow:
                        doom.Player.x_2 = doom.Player.x_1 + Math.Cos(currRotation + rotation);
                        doom.Player.y_2 = doom.Player.y_1 + Math.Sin(currRotation + rotation);
                        break;
                    case ConsoleKey.LeftArrow:
                        doom.Player.x_2 = doom.Player.x_1 + Math.Cos(currRotation - rotation);
                        doom.Player.y_2 = doom.Player.y_1 + Math.Sin(currRotation - rotation);
                        break;
                    default:
                        break;
                }
                //Console.Clear();
                doom.Render();
                gmu.PrintFrame();
            }


            //Console.ReadKey(true);
        }
        public static void StartGMUF()
        {
            GMUF gmu = new GMUF(height, width);
            NotAConsoleWindow w = new NotAConsoleWindow();

            Task t = new Task(() =>
            {
                Application.EnableVisualStyles();
                Application.Run(w);
            });
            (gmu as GMUF).SetRederingForm(w);
            
            t.Start();
            


            MultiSplitScreenManager sm = new MultiSplitScreenManager(gmu.PlacePixels, height, width);
            // use this for rendering
            FullScreenManager rs = new FullScreenManager(width - 2, height - 2, null);

            DoomScreenManager doom = new DoomScreenManager(null, width - 2, height - 2);

            sm.AddScreen(doom, new System.Drawing.Rectangle(1, 1, width - 2, height - 2));

            doom.Render();
            gmu.PrintFrame();

            
            ConsoleKey input;
            bool sneak = false;

            while ((input = Console.ReadKey(true).Key) != ConsoleKey.Spacebar)
            {

                double currRotation = Math.Atan2(doom.Player.y_2 - doom.Player.y_1, doom.Player.x_2 - doom.Player.x_1);
                switch (input)
                {
                    case ConsoleKey.C:
                        if (sneak)
                        {
                            doom.PlayerHeight = 1;
                            sneak = false;
                        }
                        else
                        {
                            doom.PlayerHeight = 0.5;
                            sneak = true;
                        }
                        break;
                    case ConsoleKey.A:
                        MoveLeft(doom);
                        break;
                    case ConsoleKey.S:
                        MoveBack(doom);
                        break;
                    case ConsoleKey.D:
                        MoveRight(doom);
                        break;
                    case ConsoleKey.W:
                        MoveFront(doom);
                        break;
                    case ConsoleKey.RightArrow:
                        doom.Player.x_2 = doom.Player.x_1 + Math.Cos(currRotation + rotation);
                        doom.Player.y_2 = doom.Player.y_1 + Math.Sin(currRotation + rotation);
                        break;
                    case ConsoleKey.LeftArrow:
                        doom.Player.x_2 = doom.Player.x_1 + Math.Cos(currRotation - rotation);
                        doom.Player.y_2 = doom.Player.y_1 + Math.Sin(currRotation - rotation);
                        break;
                    default:
                        break;
                }
                //Console.Clear();
                
                doom.Render();
                gmu.PrintFrame();
            }


            //Console.ReadKey(true);
        }


    }
}
