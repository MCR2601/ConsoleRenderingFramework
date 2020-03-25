using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;
using System.Drawing;
using ConsoleRenderingFramework.RenderProviders;

namespace ConsoleRenderingFramework.BasicScreenManagerPackage
{
    public class MultiSplitScreenManager : IRenderingApplication, IScreenManager
    {

        /// <summary>
        /// this stores all the screens by Application plus the layer
        /// </summary>
        Dictionary<IRenderingApplication, ScreenInfo> SplitScreens = new Dictionary<IRenderingApplication, ScreenInfo>();

        private class ScreenInfo
        {
            public Rectangle Dimensions;
            public int SortingLayer;
            public bool Shown;

            public ScreenInfo(Rectangle dimensions, int sortingLayer, bool shown)
            {
                Dimensions = dimensions;
                SortingLayer = sortingLayer;
                Shown = shown;
            }
        }

        /// <summary>
        /// the highes layer that exists
        /// </summary>
        int highestLayer = 0;


        public int width { get; set; }
        public int height { get; set; }

        public event DrawEvent DrawScreen;

        public PInfo Background = new PInfo();

        // Borders will be placed around every Screen if enabled
        // YOU have to make sure they dont overlap too much
        // They are Drawn BEFORE everything
        public PInfo Border = new PInfo();
        public bool EnablingBordering = false;

        public MultiSplitScreenManager(DrawEvent DrawHirachy, int w, int h)
        {
            DrawScreen += DrawHirachy;
            width = w;
            height = h;
        }

        public MultiSplitScreenManager(DrawEvent DrawHirachy, int w, int h, PInfo BackgroundStyle) : this(DrawHirachy,w,h)
        {
            Background = BackgroundStyle;
        }

        public MultiSplitScreenManager(DrawEvent DrawHirachy, int w, int h, PInfo BackgroundStyle, PInfo BorderStyle) : this(DrawHirachy, w, h, BackgroundStyle)
        {
            Border = BorderStyle;
            EnablingBordering = true;
        }

        public void AddScreen(IRenderingApplication app, Rectangle rec)
        {
            SplitScreens.Add(app, new ScreenInfo(rec,highestLayer,true));
            highestLayer++;
            RegisterApp(app);
        }

        public void AddScreen(IRenderingApplication app, Rectangle rec, int layer)
        {
            SplitScreens.Add(app, new ScreenInfo(rec,layer,true));
            RegisterApp(app);
        }
        
        void RegisterApp(IRenderingApplication app)
        {
            app.DrawScreen += App_DrawScreen;
        }

        public void App_DrawScreen(PInfo[,] information, int x, int y, IRenderingApplication sender)
        {
            
            if (DrawScreen!=null)
            {
                if (sender == this)
                {
                    DrawScreen(information, x, y, this);
                }
                else
                {
                    //Console.WriteLine(sender);
                    ScreenInfo info = SplitScreens[sender];
                    DrawScreen(information, x + info.Dimensions.X, y + info.Dimensions.Y, this);
                }                
            }
        }

        public void Render()
        {
            App_DrawScreen(BasicProvider.getInked(width, height, Background), 0, 0, this);

            

            foreach (var item in SplitScreens.OrderBy(x=>x.Value.SortingLayer))
            {
                ScreenInfo info = item.Value;
                if (EnablingBordering)
                {
                    App_DrawScreen(BasicProvider.getInked(info.Dimensions.Width + 2, info.Dimensions.Height + 2, Border), info.Dimensions.X - 1, info.Dimensions.Y - 1, this);
                    App_DrawScreen(BasicProvider.getInked(info.Dimensions.Width, info.Dimensions.Height, Background), info.Dimensions.X, info.Dimensions.Y, this);
                }
                item.Key.Render();
            }
        }

        public void ChangeLayerOf(IRenderingApplication app, int newLayer)
        {
            if (SplitScreens.ContainsKey(app))
            {
                SplitScreens[app].SortingLayer = newLayer;
            }
        }

        public Rectangle GetPositionOf(IRenderingApplication app)
        {
            if (SplitScreens.ContainsKey(app))
            {
                return SplitScreens[app].Dimensions;
            }
            return new Rectangle();
        }
        
        public void SetPositionOf(IRenderingApplication app, Rectangle rec)
        {
            if (SplitScreens.ContainsKey(app))
            {
                SplitScreens[app].Dimensions = rec;
            }
        }
        public void TranslatePositionOf(IRenderingApplication app, Point p)
        {
            if (SplitScreens.ContainsKey(app))
            {
                SplitScreens[app].Dimensions.Offset(p);
            }
        }


    }
}
