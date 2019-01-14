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
        public Form RenderingForm;
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

        public void SetRederingForm(Form f)
        {
            RenderingForm = f;
            g = f.CreateGraphics();
        }


    }
}
