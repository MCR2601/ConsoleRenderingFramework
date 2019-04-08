using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;
using System.Diagnostics;

namespace Doom
{
    class DoomScreenManager : IScreenManager, IRenderingApplication
    {
        public int width { get; set ; }
        public int height { get; set; }

        public event DrawEvent DrawScreen;

        double distPerHeigth;
        const double MAXDRAWDIST = 15;
        static int FOV = 90;
        public Line Player = new Line(0,0,0,1);

        public double PlayerHeight = 1;
        public double RoomHeight = 2;

        private ObjectSprite enemy = new ObjectSprite()
        {
            Name = "Enemy",
            isTransparent = true,
            Sprite = new PInfo[,]{
                {
                    new PInfo(){ },
                    new PInfo(){ },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ },
                    new PInfo(){ }
                },
                {
                    new PInfo(){ },
                    new PInfo(){},
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ },
                    new PInfo(){  }
                },
                {
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true }
                },
                {
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true }
                },
                {
                    new PInfo(){ },
                    new PInfo(){ },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ },
                    new PInfo(){}
                },
                {
                    new PInfo(){ },
                    new PInfo(){},
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ Background = ConsoleColor.DarkGreen, HasBackground = true },
                    new PInfo(){ },
                    new PInfo(){ }
                }
            }
        };
        /// <summary>
        /// The line of a wall, and the sprite of that wall
        /// </summary>
        public Dictionary<Line, Object3D> DObjectects = new Dictionary<Line, Object3D>();  
        /// <summary>
        /// Entity object, position
        /// </summary>
        public Dictionary<ObjectSprite,Tuple<double,double>> SpriteObjects = new Dictionary<ObjectSprite, Tuple<double, double>>();



        List<Tuple<Line, PInfo>> allLines = new List<Tuple<Line, PInfo>>()
        {
            new Tuple<Line,PInfo>(new Line(-2,-2,2,-2),new PInfo(){ HasBackground = true, Background = ConsoleColor.Yellow }),
            new Tuple<Line, PInfo>(new Line(2,4,1,4),new PInfo(){HasBackground = true, Background = ConsoleColor.Cyan}),
            new Tuple<Line, PInfo>(new Line(1,5,2,5),new PInfo(){HasBackground = true, Background = ConsoleColor.Cyan}),
            new Tuple<Line, PInfo>(new Line(1,4,1,5),new PInfo(){HasBackground = true, Background = ConsoleColor.DarkCyan}),
            new Tuple<Line,PInfo>(new Line(-2,-2,-2,10),new PInfo(){ HasBackground = true, Background = ConsoleColor.Blue }),
            new Tuple<Line,PInfo>(new Line(2,-2,2,10),new PInfo(){ HasBackground = true, Background = ConsoleColor.Blue}),
            new Tuple<Line,PInfo>(new Line(-2,10,2,10),new PInfo(){ HasBackground = true, Background = ConsoleColor.Yellow}),
        };

        public DoomScreenManager(DrawEvent DrawHirachy, int w, int h)
        {
            DrawScreen += DrawHirachy;
            width = w;
            height = h;
            distPerHeigth = MAXDRAWDIST / height / 2;
            //allLines = Labyrinth;

            #region information
            Object3D Wall = new Object3D()
            {
                patternheight = 2,
                patternwidth = 1,
                isTransparent = false,
                pattern = new PInfo[,]
                {
                    {
                        new PInfo(){HasBackground = true, Background = ConsoleColor.DarkGreen},
                        new PInfo(){HasBackground = true, Background = ConsoleColor.Gray},
                        //new PInfo(){HasBackground = true, Background = ConsoleColor.Black}
                    },
                    {
                        new PInfo(){HasBackground = true, Background = ConsoleColor.White},
                        new PInfo(){HasBackground = true, Background = ConsoleColor.Black},
                        //new PInfo(){HasBackground = true, Background = ConsoleColor.White}
                    }/*,
                    {
                        new PInfo(){HasBackground = true, Background = ConsoleColor.Yellow},
                        new PInfo(){HasBackground = true, Background = ConsoleColor.Red},
                        //new PInfo(){hasBackground = true, background = ConsoleColor.Black}
                    }/*,
                    {
                        new PInfo(){hasBackground = true, background = ConsoleColor.White},
                        new PInfo(){hasBackground = true, background = ConsoleColor.Black},
                        new PInfo(){hasBackground = true, background = ConsoleColor.White}
                    }  */                  
                }
            };
            foreach (var item in allLines)
            {
                DObjectects.Add(item.Item1, Wall);
            }
            //SpriteObjects.Add(enemy, new Tuple<double, double>(0, 5));
            #endregion
        }

                          
        public void App_DrawScreen(PInfo[,] information, int x, int y, IRenderingApplication sender)
        {
            DrawScreen?.Invoke(information, x, y, this);
        }
        /// <summary>
        /// TODO: Render this Frame
        /// </summary>
        public void Render()
        {
            Line[] strips = getScreenInfo(Player, width, FOV);

            //for (int i = 0; i < strips.Length; i++)
            //{
            //    if (i%20==0)
            //    {
            //        Console.ReadKey(true);
            //    }
            //    Console.WriteLine("Strip Nr {0}: {1}", i, Math.Atan2(strips[i].y_2 - strips[i].y_1, strips[i].x_2 - strips[i].x_1)*180/Math.PI);
            //}
            //Console.ReadKey();
            for (int x = 0; x < width; x++)
            {
                App_DrawScreen(renderLine(strips[x]), x, 0, this);
                //App_DrawScreen(SplitRenderer.getColoredStrip(height, getClosestWall(strips[x]), new PInfo() { hasBackground = true, background = ConsoleColor.DarkGray }, new PInfo() { hasBackground = true,background = ConsoleColor.Gray}),x,0,this);
            }

        }
        private PInfo[,] renderLine(Line look)
        {
            PInfo[,] strip = new PInfo[1, height];
            strip.Populate();

            // Renderable, dist from origin, dist from player
            Stack<Tuple<IRenderable, double, double>> renderstack = new Stack<Tuple<IRenderable, double, double>>();


            // create a list of all things
            List<Tuple<Line, IRenderable>> visibles = new List<Tuple<Line, IRenderable>>();


            foreach (var item in SpriteObjects)
            {
                Line l = new Line();
                // angle to player
                double changeX = Player.x_2 - Player.x_1;
                double changeY = Player.y_2 - Player.y_1;
                // get angle to player
                double angle = Math.Atan2(changeY, changeX);
                // now rotate and get cords of start and end
                // origin, left of player    
                l.x_1 = item.Value.Item1 + (Math.Cos(angle - (90 / Math.PI * 2)) * item.Key.Width / 2);
                l.y_1 = item.Value.Item2 + (Math.Sin(angle - (90 / Math.PI * 2)) * item.Key.Width / 2);
                // other side right of player
                l.x_2 = item.Value.Item1 + (Math.Cos(angle + (90 / Math.PI * 2)) * item.Key.Width / 2);
                l.y_2 = item.Value.Item2 + (Math.Sin(angle + (90 / Math.PI * 2)) * item.Key.Width / 2);
                visibles.Add(new Tuple<Line, IRenderable>(l, item.Key));
            }

            foreach (var item in DObjectects)
            {
                visibles.Add(new Tuple<Line, IRenderable>(item.Key, item.Value));
            }

            // create shorter representation of line
            double dirX = look.x_2 - look.x_1;
            double dirY = look.y_2 - look.y_1;

            double dist = Math.Sqrt(dirX * dirX + dirY * dirY);
            // normalize vector
            dirX /= dist;
            dirY /= dist;

            // now calculate the number of pixels until first contact with a wall
            int inst = 0;
            //PInfo wallLook = new PInfo() { hasBackground = true, background = ConsoleColor.Black };
            bool reachedWall = false;


            while ((!reachedWall) && inst * 0.01 < MAXDRAWDIST)
            {
                inst++;
                look.x_2 = look.x_1 + dirX * MAXDRAWDIST;
                look.y_2 = look.y_1 + dirY * MAXDRAWDIST;
                
                double xInt;
                double yInt;

                if (look.y_2 == 15.3)
                {
                    Debug.WriteLine("");
                }

                var found = visibles
                    .Where(v => doLinesIntersect(look, v.Item1, out xInt, out yInt))
                    .Select(i =>
                    {
                        double atX;
                        double atY;
                        doLinesIntersect(look, i.Item1, out atX, out atY);
                        return new { Object = i, Distance = Math.Sqrt(Math.Pow(look.x_1 - atX, 2) + Math.Pow(look.y_1 - atY, 2)), XInt = atX, YInt = atY };
                    })
                    .ToList();
                if (found.Count == 0)
                {
                    Debug.WriteLine("empty");
                }
                var ToRender = found
                    .Where(f => f.Distance <= found.Where(wall => !wall.Object.Item2.isTransparent).Min(closest => closest.Distance))
                    .OrderBy(all => -all.Distance)
                    .ToList();

                if (ToRender.Count==0)
                {
                    Debug.WriteLine("empty");
                }

                reachedWall = true;
                foreach (var some in ToRender)
                {
                    double distFromOrigin = Math.Sqrt(Math.Pow(some.XInt - some.Object.Item1.x_1, 2) + Math.Pow(some.YInt - some.Object.Item1.y_1, 2));
                    renderstack.Push(new Tuple<IRenderable, double, double>(some.Object.Item2, distFromOrigin, some.Distance));
                }
                //Debug.WriteLine(ToRender.Count);
                /*
                foreach (var item in visibles)
                {
                    double xIntersect;
                    double yIntersect;
                    if (doLinesIntersect(look, item.Item1, out xIntersect, out yIntersect))
                    {

                        if (!renderstack.Select((x) => { return x.Item1; }).Contains(item.Item2))
                        {
                            if (!item.Item2.isTransparent)
                            {
                                reachedWall = true;
                            }
                            // distance from object origin
                            double distFromOrigin = Math.Sqrt(Math.Pow(xIntersect - item.Item1.x_1, 2) + Math.Pow(yIntersect - item.Item1.y_1, 2));
                            // Object, distance from object origin, distance from camera
                            renderstack.Push(new Tuple<IRenderable, double, double>(item.Item2, distFromOrigin, inst * 0.01));
                        }
                        
                    }
                }*/

            }

            renderstack.Push(new Tuple<IRenderable, double, double>(
            new Object3D()
            {
                isTransparent = false,
                pattern = new PInfo[,]
                {
                        {
                            new PInfo(){ HasBackground = true,Background = ConsoleColor.Black}
                        }
                }
                
                
            }
            , 1
            , MAXDRAWDIST
            ));

            while (renderstack.Count > 0)
            {
                Tuple<IRenderable, double, double> data = renderstack.Pop();

                strip.Merge(data.Item1.Render(data.Item3, height, PlayerHeight, RoomHeight, data.Item2));

            }

            return strip;
        }

        private Tuple<Tuple<int, int, int>, PInfo> getClosestWall(Line look)
        {
            double updown = 30 * Math.PI / 180;


            // create shorter representation of line
            double dirX = look.x_2 - look.x_1;
            double dirY = look.y_2 - look.y_1;

            double dist = Math.Sqrt(dirX * dirX + dirY * dirY);
            // normalize vector
            dirX /= dist;
            dirY /= dist;

            // now calculate the number of pixels until first contact with a wall
            bool wallReached = false;
            int inst = 0;
            PInfo wallLook = new PInfo() { HasBackground = true, Background = ConsoleColor.Black };

            while (!wallReached && inst * 0.01 < MAXDRAWDIST)
            {
                inst++;
                look.x_2 = look.x_1 + dirX * inst * 0.01;
                look.y_2 = look.y_1 + dirY * inst * 0.01;

                foreach (var item in allLines)
                {
                    double xIntersect;
                    double yIntersect;
                    if (doLinesIntersect(look, item.Item1, out xIntersect, out yIntersect))
                    {
                        wallReached = true;
                        wallLook = item.Item2;
                    }
                }
            }
            // calculate the parts of top and bottom
            double upHigh = RoomHeight - PlayerHeight;

            


            double unseenTop = Math.Tan(updown) * upHigh;
            double unseenBot = Math.Tan(updown)*PlayerHeight;
            
            double angleMidTop = Math.Atan(upHigh / inst * 100);
            double angleMidBot = Math.Atan(PlayerHeight / inst * 100);
            double angleTop = (90 * Math.PI/ 180) - unseenTop - angleMidTop;
            double angleBot = (90 * Math.PI / 180) - unseenBot - angleMidBot;

            double halfheight = height / 2;

            int Top = (int)Math.Round((angleTop / (angleTop + angleMidTop))* halfheight);
            int Mid = (int)Math.Round(((angleMidTop / (angleTop + angleMidTop)) * halfheight) + ((angleMidBot / (angleBot + angleMidBot)) * halfheight));
            int Bottom = (int)Math.Round((angleBot /(angleBot + angleMidBot)) * halfheight);

            while (Top+Mid+Bottom<height)
            {
                Mid++;
            }

            return new Tuple<Tuple<int, int, int>, PInfo>(new Tuple<int, int, int>(Top,Mid,Bottom), wallLook);
        }
        private bool doLinesIntersect(Line li1, Line li2, out double posX, out double posY)
        {
            Line l1 = new Line(Math.Round(li1.x_1, 10), Math.Round(li1.y_1, 10), Math.Round(li1.x_2, 10), Math.Round(li1.y_2, 10));
            Line l2 = new Line(Math.Round(li2.x_1, 10), Math.Round(li2.y_1, 10), Math.Round(li2.x_2, 10), Math.Round(li2.y_2, 10));

            double A1 = l1.y_2 - l1.y_1;
            double B1 = l1.x_1 - l1.x_2;
            double C1 = A1 * l1.x_1 + B1 * l1.y_1;

            double A2 = l2.y_2 - l2.y_1;
            double B2 = l2.x_1 - l2.x_2;
            double C2 = A2 * l2.x_1 + B2 * l2.y_1;
            posX = 0;
            posY = 0;
            double det = A1 * B2 - A2 * B1;
            if (det == 0)
            {
                
                return false;
            }
            else
            {
                double x = (B2 * C1 - B1 * C2) / det;
                double y = (A1 * C2 - A2 * C1) / det;

                x = Math.Round(x, 10);
                y = Math.Round(y, 10);

                if (Math.Min(l1.x_1,l1.x_2)<= x && x <= Math.Max(l1.x_1,l1.x_2)&& Math.Min(l1.y_1, l1.y_2) <= y && y <= Math.Max(l1.y_1, l1.y_2) && Math.Min(l2.x_1, l2.x_2) <= x && x <= Math.Max(l2.x_1, l2.x_2) && Math.Min(l2.y_1, l2.y_2) <= y && y <= Math.Max(l2.y_1, l2.y_2))
                {
                    posX = x;
                    posY = y;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gives all Lines required for the screen
        /// </summary>
        /// <param name="source">the center direction</param>
        /// <param name="numberOfLines">How many lines should be calculated?</param>
        /// <param name="FOV">total angle from left to right</param>
        /// <returns></returns>
        private Line[] getScreenInfo(Line source,int numberOfLines, double FOV)
        {
            double baseAngleLeft = ((Math.Atan2(source.y_2 - source.y_1, source.x_2 - source.x_1) * 180 / Math.PI) - (FOV / 2) + 360) % 360;
            double angleStep = FOV / numberOfLines;

            Line[] lines = new Line[numberOfLines];
            for (int a = 0 ; a < numberOfLines; a++)
            {
                Line tmp = new Line(source.x_1, source.y_1, source.x_1 + Math.Cos((baseAngleLeft + ((a) * angleStep))*Math.PI/180), source.y_1 + Math.Sin((baseAngleLeft + ((a) * angleStep)) * Math.PI / 180));
                lines[a] = tmp;
                //Debug.WriteLine(((a) * angleStep)+ "   -   " + ((a) * angleStep));
            }
            return lines;
        }
        static ConsoleColor labyrinthVertical = ConsoleColor.Blue;
        static ConsoleColor labyrinthHorizontal = ConsoleColor.DarkBlue;
        static ConsoleColor labyrinthSpecial1 = ConsoleColor.Yellow;
        static ConsoleColor labyrinthSpecial2 = ConsoleColor.Blue;
        public static List<Tuple<Line, PInfo>> Labyrinth = new List<Tuple<Line, PInfo>>()
        {
            // This Labyrinth has a specific colorsceme
            // entrance: 4x4 area
            new Tuple<Line, PInfo>(new Line(-2,-2,-2,2),new PInfo(){HasBackground = true, Background = labyrinthVertical }),
            new Tuple<Line, PInfo>(new Line(-2,-2,2,-2),new PInfo(){HasBackground = true, Background =  labyrinthHorizontal}),
            new Tuple<Line, PInfo>(new Line(-2,2,-1,2),new PInfo(){HasBackground = true, Background =  labyrinthHorizontal}),
            new Tuple<Line, PInfo>(new Line(1,2,2,2),new PInfo(){HasBackground = true, Background =  labyrinthHorizontal}),
            new Tuple<Line, PInfo>(new Line(2,-2,2,2),new PInfo(){HasBackground = true, Background =  labyrinthVertical}),
            
            // first fork 
            new Tuple<Line, PInfo>(new Line(-4,4,-1,4),new PInfo(){HasBackground = true, Background = labyrinthHorizontal }),
            new Tuple<Line, PInfo>(new Line(-1,2,-1,4),new PInfo(){HasBackground = true, Background = labyrinthVertical }),
            new Tuple<Line, PInfo>(new Line(1,2,1,4),new PInfo(){HasBackground = true, Background =  labyrinthVertical}),
            new Tuple<Line, PInfo>(new Line(1,4,5,4),new PInfo(){HasBackground = true, Background = labyrinthHorizontal }),
            new Tuple<Line, PInfo>(new Line(-2,6,3,6),new PInfo(){HasBackground = true, Background = labyrinthHorizontal }),


            // Inside deadend
            new Tuple<Line, PInfo>(new Line(-4,4,-4,13),new PInfo(){HasBackground = true, Background =  labyrinthVertical}),
            new Tuple<Line, PInfo>(new Line(-2,6,-2,8),new PInfo(){HasBackground = true, Background =  labyrinthVertical}),
            new Tuple<Line, PInfo>(new Line(-2,8,2,8),new PInfo(){HasBackground = true, Background = labyrinthHorizontal }),
            new Tuple<Line, PInfo>(new Line(2,8,2,10),new PInfo(){HasBackground = true, Background = labyrinthVertical }),
            new Tuple<Line, PInfo>(new Line(-2,10,2,10),new PInfo(){HasBackground = true, Background =  labyrinthHorizontal}),
            new Tuple<Line, PInfo>(new Line(-2,10,-2,11),new PInfo(){HasBackground = true, Background = labyrinthVertical }),
            
            // Outside deadend
            new Tuple<Line, PInfo>(new Line(3,6,3,11),new PInfo(){HasBackground = true, Background = labyrinthVertical }),
            new Tuple<Line, PInfo>(new Line(5,4,5,8),new PInfo(){HasBackground = true, Background =  labyrinthVertical}),
            new Tuple<Line, PInfo>(new Line(5,8,8,8),new PInfo(){HasBackground = true, Background = labyrinthHorizontal }),
            new Tuple<Line, PInfo>(new Line(8,8,8,10),new PInfo(){HasBackground = true, Background = labyrinthVertical }),
            new Tuple<Line, PInfo>(new Line(5,10,8,10),new PInfo(){HasBackground = true, Background = labyrinthHorizontal}),
            new Tuple<Line, PInfo>(new Line(5,10,5,13),new PInfo(){HasBackground = true, Background =  labyrinthVertical}),

            // last room
            new Tuple<Line, PInfo>(new Line(-2,11,3,11),new PInfo(){HasBackground = true, Background = labyrinthHorizontal }),
            new Tuple<Line, PInfo>(new Line(-4,13,-2,13),new PInfo(){HasBackground = true, Background = labyrinthHorizontal }),
            new Tuple<Line, PInfo>(new Line(0,13,5,13),new PInfo(){HasBackground = true, Background = labyrinthHorizontal }),
            new Tuple<Line, PInfo>(new Line(-3,15,-3,17),new PInfo(){HasBackground = true, Background =  labyrinthVertical}),
            new Tuple<Line, PInfo>(new Line(-3,17,1,17),new PInfo(){HasBackground = true, Background =  labyrinthHorizontal}),
            new Tuple<Line, PInfo>(new Line(1,15,1,17),new PInfo(){HasBackground = true, Background = labyrinthVertical }),
            // angled
            new Tuple<Line, PInfo>(new Line(-2,13,-3,15),new PInfo(){HasBackground = true, Background = labyrinthSpecial2 }),
            new Tuple<Line, PInfo>(new Line(0,13,1,15),new PInfo(){HasBackground = true, Background = labyrinthSpecial2 }),

            // goal
            new Tuple<Line, PInfo>(new Line(-1.5,15,-1.5,16),new PInfo(){HasBackground = true, Background =  labyrinthSpecial1}),
            new Tuple<Line, PInfo>(new Line(-1.5,16,-0.5,16),new PInfo(){HasBackground = true, Background =  labyrinthSpecial1}),
            new Tuple<Line, PInfo>(new Line(-1.5,15,-0.5,15),new PInfo(){HasBackground = true, Background =  labyrinthSpecial1}),
            new Tuple<Line, PInfo>(new Line(-0.5,15,-0.5,16),new PInfo(){HasBackground = true, Background =  labyrinthSpecial1})


        };
    }
}
