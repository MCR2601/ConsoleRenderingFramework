using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleRenderingFramework
{
    public partial class NotAConsoleWindow : Form
    {

        public Bitmap image;



        public NotAConsoleWindow()
        {
            InitializeComponent();
        }

        private void RenderTick_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("Tick tick");
            screen.Image = image;
            screen.Refresh();
        }

        public void EnableRender()
        {
            RenderTick.Enabled = true;
        }
        public void DisableRender()
        {
            RenderTick.Enabled = false;
        }
        public void SetUpdateRate(int time)
        {
            RenderTick.Interval = time;
        }

        private void test_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Tick tick");
            RenderTick.Enabled = true;
            screen.Image = image;
            screen.Refresh();
            test.Text = "Click";
        }
    }
}
