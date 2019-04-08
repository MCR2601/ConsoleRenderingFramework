using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleRenderingFramework
{
    /// <summary>
    /// This is an extension of the <see cref="GMU"/>.
    /// This prints to a Form instead of a windows console, making it not rely on the slow rendering speed
    /// </summary>
    public class GMUF : GMU
    {
        public NotAConsoleWindow RenderingForm;
        Graphics g;

        public int pixelSize = 4;

        public GMUF(int height, int width)
        {
            this.height = height;
            this.width = width;
        }

        protected override void PrintPixel(int y, int x)
        {
            PInfo pi = ScreenBuffer[x, y];
            Brush b = PInfoUtil.GetPInfoBrush(pi);

            g.FillRectangle(b, new Rectangle(x * pixelSize, y * pixelSize, pixelSize, pixelSize));

        }

        public override void PrintFrame()
        {
            Bitmap map = new Bitmap(width, height);


            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!ScreenBuffer[x, y].Equals(CurrentScreen[x, y]))
                    {
                        //PrintPixel(y, x);
                        CurrentScreen[x, y].Override(ScreenBuffer[x, y]);
                    }
                    ScreenBuffer[x, y].isChanged = false;
                    map.SetPixel(x, y, ConsoleToColor(CurrentScreen[x, y].Background));
                }
            }
            RenderingForm.image = map;
        }

        public void SetRederingForm(NotAConsoleWindow f)
        {
            RenderingForm = f;
        }

        public Color ConsoleToColor(ConsoleColor cc)
        {
            switch (cc)
            {
                case ConsoleColor.Black:
                    return Color.Black;
                    break;
                case ConsoleColor.DarkBlue:
                    return Color.DarkBlue;
                    break;
                case ConsoleColor.DarkGreen:
                    return Color.DarkGreen;
                    break;
                case ConsoleColor.DarkCyan:
                    return Color.DarkCyan;
                    break;
                case ConsoleColor.DarkRed:
                    return Color.DarkRed;
                    break;
                case ConsoleColor.DarkMagenta:
                    return Color.DarkMagenta;
                    break;
                case ConsoleColor.DarkYellow:
                    return Color.Yellow;
                    break;
                case ConsoleColor.Gray:
                    return Color.Gray;
                    break;
                case ConsoleColor.DarkGray:
                    return Color.DarkGray;
                    break;
                case ConsoleColor.Blue:
                    return Color.Blue;
                    break;
                case ConsoleColor.Green:
                    return Color.Green;
                    break;
                case ConsoleColor.Cyan:
                    return Color.Cyan;
                    break;
                case ConsoleColor.Red:
                    return Color.Red;
                    break;
                case ConsoleColor.Magenta:
                    return Color.Magenta;
                    break;
                case ConsoleColor.Yellow:
                    return Color.LightYellow;
                    break;
                case ConsoleColor.White:
                    return Color.White;
                    break;
                default:
                    return Color.White;
                    break;
            }
        }

    }
}
