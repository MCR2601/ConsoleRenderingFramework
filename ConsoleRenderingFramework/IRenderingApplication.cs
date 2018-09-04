using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderingFramework
{
    /// <summary>
    /// this interface provides methodes and members for all
    /// apps which want to use the <see cref="GMU"/> or any ScreenManager 
    /// </summary>
    public interface IRenderingApplication
    {
        /// <summary>
        /// width of this application
        /// </summary>
        int width { get; set; }
        /// <summary>
        /// height of this application
        /// </summary>
        int height { get; set; }
        /// <summary>s
        /// Event that forwards screen information to the next layer
        /// </summary>
        event DrawEvent DrawScreen;
        void Render();
        void App_DrawScreen(PInfo[,] information, int x, int y, IRenderingApplication sender);
    }
    /// <summary>
    /// An event for drawing pixels into memory
    /// </summary>
    /// <param name="information">What should be written</param>
    /// <param name="x">x-coordinate of top left corner</param>
    /// <param name="y">y-coordinate of top left corner</param>
    public delegate void DrawEvent(PInfo[,] information, int x, int y, IRenderingApplication sender);
}
