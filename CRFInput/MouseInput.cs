using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ConsoleRenderingFramework.Input
{
    /// <summary>
    /// Provides Mouse Input management
    /// </summary>
    public static class MouseInput
    {

        public static MousePosition GetMousePosition()
        {
            var asdt = new System.Windows.Forms.Cursor(System.Windows.Forms.Cursor.Current.Handle);

            var t = System.Windows.Forms.Control.MousePosition;

            return new MousePosition(t.X, t.Y);
        }
        public static MousePosition GetMouseConsolePosition()
        {
            MousePosition pos = GetMousePosition();

            int x = Console.WindowLeft;
            int y = Console.WindowTop;
            int w = Console.WindowWidth;
            int h = Console.WindowHeight;

            

            return new MousePosition(x,y,w,h);
        }

    }
}
