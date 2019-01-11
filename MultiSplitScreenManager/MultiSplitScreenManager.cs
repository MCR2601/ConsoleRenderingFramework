using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;
using System.Drawing;
using BasicRenderProviders;

namespace BasicScreenManagerPackage
{
    public class MultiSplitScreenManager : IRenderingApplication, IScreenManager
    {

        /// <summary>
        /// this stores all the screens by Application plus the layer
        /// </summary>
        Dictionary<IRenderingApplication, Tuple<Rectangle, int>> SplitScreens = new Dictionary<IRenderingApplication, Tuple<Rectangle, int>>();
        
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
            SplitScreens.Add(app, new Tuple<Rectangle, int>(rec, highestLayer));
            highestLayer++;
            RegisterApp(app);
        }

        public void AddScreen(IRenderingApplication app, Rectangle rec, int layer)
        {
            SplitScreens.Add(app, new Tuple<Rectangle, int>(rec, layer));
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
                    Tuple<Rectangle, int> pos = SplitScreens[sender];
                    DrawScreen(information, x + pos.Item1.X, y + pos.Item1.Y, this);
                }                
            }
        }

        public void Render()
        {
            App_DrawScreen(BasicProvider.getInked(width, height, Background), 0, 0, this);

            

            foreach (var item in SplitScreens.OrderBy(x=>x.Value.Item2))
            {
                if (EnablingBordering)
                {

                    App_DrawScreen(BasicProvider.getInked(item.Value.Item1.Width + 2, item.Value.Item1.Height + 2, Border), item.Value.Item1.X - 1, item.Value.Item1.Y - 1, this);
                    App_DrawScreen(BasicProvider.getInked(item.Value.Item1.Width, item.Value.Item1.Height, Background), item.Value.Item1.X, item.Value.Item1.Y, this);

                }
                item.Key.Render();
            }
        }

        public void ChangeLayerOf(IRenderingApplication app, int newLayer)
        {
            if (SplitScreens.ContainsKey(app))
            {
                SplitScreens[app] = new Tuple<Rectangle, int>(SplitScreens[app].Item1,newLayer);
            }
        }

    }
}
