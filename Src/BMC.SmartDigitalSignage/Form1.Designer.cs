namespace BMC.SmartDigitalSignage
{
    partial class Form1
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
            this.SlidePicture = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.TimerDetector = new System.Windows.Forms.Timer(this.components);
            this.SlideTimer = new System.Windows.Forms.Timer(this.components);
            this.DetectedFacePic = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.SlidePicture)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetectedFacePic)).BeginInit();
            this.SuspendLayout();
            // 
            // SlidePicture
            // 
            this.SlidePicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SlidePicture.Location = new System.Drawing.Point(0, 0);
            this.SlidePicture.Name = "SlidePicture";
            this.SlidePicture.Size = new System.Drawing.Size(800, 450);
            this.SlidePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SlidePicture.TabIndex = 0;
            this.SlidePicture.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 424);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 26);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLbl
            // 
            this.StatusLbl.Name = "StatusLbl";
            this.StatusLbl.Size = new System.Drawing.Size(80, 20);
            this.StatusLbl.Text = "Welcome...";
            // 
            // TimerDetector
            // 
            this.TimerDetector.Interval = 2000;
            this.TimerDetector.Tick += new System.EventHandler(this.TimerDetector_Tick);
            // 
            // SlideTimer
            // 
            this.SlideTimer.Interval = 5000;
            this.SlideTimer.Tick += new System.EventHandler(this.SlideTimer_Tick);
            // 
            // DetectedFacePic
            // 
            this.DetectedFacePic.Location = new System.Drawing.Point(12, 12);
            this.DetectedFacePic.Name = "DetectedFacePic";
            this.DetectedFacePic.Size = new System.Drawing.Size(300, 225);
            this.DetectedFacePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.DetectedFacePic.TabIndex = 2;
            this.DetectedFacePic.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DetectedFacePic);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.SlidePicture);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Smart Digital Signage 1.0";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.SlidePicture)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetectedFacePic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox SlidePicture;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLbl;
        private System.Windows.Forms.Timer TimerDetector;
        private System.Windows.Forms.Timer SlideTimer;
        private System.Windows.Forms.PictureBox DetectedFacePic;
    }
}

