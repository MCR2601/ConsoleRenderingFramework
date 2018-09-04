using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleRenderingFramework
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
        }

        private void Window_Load(object sender, EventArgs e)
        {
            
        }



        public void DrawPixel()
        {
            Graphics g = this.CreateGraphics();
            g.FillRectangle( Brushes.Black, new Rectangle(20, 20, 20, 20));
            //RenderWindow.Update();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawPixel();
        }
    }
}
