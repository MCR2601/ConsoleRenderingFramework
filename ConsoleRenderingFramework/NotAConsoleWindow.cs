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

        public static readonly object BitmapLock = new object();

        public Bitmap image;

        private bool running = false;

        public Action IdleHandle;

        public NotAConsoleWindow()
        {
            InitializeComponent();
            Application.Idle += HandleApplicationIdle;
            

        }

        private void HandleApplicationIdle(object sender, EventArgs e)
        {
            if (running)
            {
                IdleHandle?.Invoke();
            }
        }

        
        private void test_Click(object sender, EventArgs e)
        {
            if (running)
            {
                return;
            }

            running = true;
        }

        public void DoDrawing(object sender, EventArgs e)
        {
            //screen.Image = image;
            lock (BitmapLock)
            {
                screen.Refresh();
            }
        }
    }
}
