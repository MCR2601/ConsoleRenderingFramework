using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRenderingFramework;

namespace Doom
{
    /// <summary>
    /// This Interface provides the Methodes required for an Object in
    /// The Doom space to work. 
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// This renders the Object to the current Position of the Player
        /// This ALLWAYS returns 1 stripe of <see cref="PInfo"/>
        /// </summary>
        /// <param name="Dist">Distance from Player to Object</param>
        /// <param name="ScreenHeight">The height of the Screen</param>
        /// <param name="PlayerHeight">The height of the Player</param>
        /// <param name="roomHeight">The height of the Room</param>
        /// <param name="DistFromLeft">Contactpoint of renderline, measured from the origin(usually left) of the object</param>
        /// <returns></returns>
        PInfo[,] Render(double Dist, int ScreenHeight, double PlayerHeight, double roomHeight, double DistFromLeft);
        /// <summary>
        /// Tells the renderere if this is blocks the entire few or if anything behind this can be seen
        /// </summary>
        bool isTransparent { get; set; }

        
    }
}
