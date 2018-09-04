using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;
using System.Drawing;

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

        public MultiSplitScreenManager(DrawEvent DrawHirachy, int w, int h)
        {
            DrawScreen += DrawHirachy;
            width = w;
            height = h;
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
                Tuple<Rectangle, int> pos = SplitScreens[sender];
                DrawScreen(information, x + pos.Item1.X, y + pos.Item1.Y, this);
            }
        }

        public void Render()
        {
            foreach (var item in SplitScreens)
            {
                item.Key.Render();
            }
        }



    }
}
