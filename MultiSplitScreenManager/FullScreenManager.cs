using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;
using System.Drawing;

namespace BasicScreenManagerPackage
{
    /// <summary>
    /// <see cref="IScreenManager"/> that covers whole screen with 1 <see cref="IRenderingApplication"/>
    /// </summary>
    public class FullScreenManager : IScreenManager, IRenderingApplication
    {

        IRenderingApplication app;
        
        public int width { get; set; }
        public int height { get; set; }

        public event DrawEvent DrawScreen;

        /// <summary>
        /// new <see cref="FullScreenManager"/>
        /// </summary>
        /// <param name="DrawHirachy">methode of next lower level</param>
        /// <param name="w">width of this screen</param>
        /// <param name="h">height of this screen</param>
        public FullScreenManager(int w, int h,DrawEvent DrawHirachy)
        {
            DrawScreen += DrawHirachy;
            width = w;
            height = h;
        }

        /// <summary>
        /// forwards the child to the next layer
        /// </summary>
        public void Render()
        {
            app?.Render();
        }
        /// <summary>
        /// add an app to render in this screen
        /// </summary>
        /// <param name="childApp">App</param>
        public void AddApp(IRenderingApplication childApp)
        {
            // if we allready have an app in our screen we remove all subscriptions from it
            // we dont want to draw a screen from an inactive app
            if (app!=null)
            {
                app.DrawScreen -= receiveDraw;
            }

            app = childApp;

            // we register ourself as the next layer
            app.DrawScreen += receiveDraw;
        }
        void receiveDraw(PInfo[,] information, int x, int y,IRenderingApplication sender)
        { 
            if (DrawScreen!=null)
            {
                DrawScreen(information, x, y,this);
            }
        }
        public void App_DrawScreen(PInfo[,] information, int x, int y, IRenderingApplication sender)
        {
            if (DrawScreen != null)
            {
                DrawScreen(information,x, y, this);
            }
        }

    }
}
