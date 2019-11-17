namespace ConsoleRenderingFramework
{
    partial class NotAConsoleWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.RenderTick = new System.Windows.Forms.Timer(this.components);
            this.screen = new System.Windows.Forms.PictureBox();
            this.test = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.screen)).BeginInit();
            this.SuspendLayout();
            // 
            // RenderTick
            // 
            this.RenderTick.Enabled = true;
            this.RenderTick.Tick += new System.EventHandler(this.RenderTick_Tick);
            // 
            // screen
            // 
            this.screen.Location = new System.Drawing.Point(49, 28);
            this.screen.Name = "screen";
            this.screen.Size = new System.Drawing.Size(536, 337);
            this.screen.TabIndex = 0;
            this.screen.TabStop = false;
            // 
            // test
            // 
            this.test.Location = new System.Drawing.Point(650, 50);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(75, 23);
            this.test.TabIndex = 1;
            this.test.TabStop = false;
            this.test.Text = "Test";
            this.test.UseVisualStyleBackColor = true;
            this.test.Click += new System.EventHandler(this.test_Click);
            // 
            // NotAConsoleWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.test);
            this.Controls.Add(this.screen);
            this.Name = "NotAConsoleWindow";
            this.Text = "NotAConsoleWindow";
            ((System.ComponentModel.ISupportInitialize)(this.screen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer RenderTick;
        private System.Windows.Forms.PictureBox screen;
        private System.Windows.Forms.Button test;
    }
}