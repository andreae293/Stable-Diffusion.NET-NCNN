namespace stable_diffusion
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtPosprompt = new System.Windows.Forms.TextBox();
            this.txtNegprompt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtseed = new System.Windows.Forms.TextBox();
            this.txtstep = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.label_width = new System.Windows.Forms.Label();
            this.label_height = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 112);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 37);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 154);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(400, 324);
            this.textBox1.TabIndex = 1;
            // 
            // txtPosprompt
            // 
            this.txtPosprompt.Location = new System.Drawing.Point(117, 7);
            this.txtPosprompt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPosprompt.Name = "txtPosprompt";
            this.txtPosprompt.Size = new System.Drawing.Size(886, 23);
            this.txtPosprompt.TabIndex = 2;
            this.txtPosprompt.Text = "best high quality landscape, in the morning light, Overlooking TOKYO beautiful ci" +
    "ty with Fujiyama， from a tall house";
            // 
            // txtNegprompt
            // 
            this.txtNegprompt.Location = new System.Drawing.Point(117, 32);
            this.txtNegprompt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNegprompt.Name = "txtNegprompt";
            this.txtNegprompt.Size = new System.Drawing.Size(886, 23);
            this.txtNegprompt.TabIndex = 3;
            this.txtNegprompt.Text = "people, fog";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Positive prompt:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Negative prompt:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(418, 59);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(596, 431);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // txtseed
            // 
            this.txtseed.Location = new System.Drawing.Point(175, 126);
            this.txtseed.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtseed.Name = "txtseed";
            this.txtseed.Size = new System.Drawing.Size(47, 23);
            this.txtseed.TabIndex = 7;
            this.txtseed.Text = "42";
            // 
            // txtstep
            // 
            this.txtstep.Location = new System.Drawing.Point(228, 126);
            this.txtstep.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtstep.Name = "txtstep";
            this.txtstep.Size = new System.Drawing.Size(47, 23);
            this.txtstep.TabIndex = 8;
            this.txtstep.Text = "15";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(175, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Seed";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(228, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Steps";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(12, 61);
            this.trackBar1.Maximum = 14;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(127, 45);
            this.trackBar1.TabIndex = 12;
            this.trackBar1.Value = 2;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(145, 60);
            this.trackBar2.Maximum = 14;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(127, 45);
            this.trackBar2.TabIndex = 12;
            this.trackBar2.Value = 2;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // label_width
            // 
            this.label_width.AutoSize = true;
            this.label_width.Location = new System.Drawing.Point(278, 61);
            this.label_width.Name = "label_width";
            this.label_width.Size = new System.Drawing.Size(63, 15);
            this.label_width.TabIndex = 13;
            this.label_width.Text = "Width: 512";
            // 
            // label_height
            // 
            this.label_height.AutoSize = true;
            this.label_height.Location = new System.Drawing.Point(278, 76);
            this.label_height.Name = "label_height";
            this.label_height.Size = new System.Drawing.Size(67, 15);
            this.label_height.TabIndex = 14;
            this.label_height.Text = "Height: 512";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(282, 128);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(130, 19);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "fast speed high ram\r\n";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 489);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label_height);
            this.Controls.Add(this.label_width);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtstep);
            this.Controls.Add(this.txtseed);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNegprompt);
            this.Controls.Add(this.txtPosprompt);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "SD .net";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private TextBox txtPosprompt;
        private TextBox txtNegprompt;
        private Label label1;
        private Label label2;
        private PictureBox pictureBox1;
        private TextBox txtseed;
        private TextBox txtstep;
        private Label label3;
        private Label label4;
        private TrackBar trackBar1;
        private TrackBar trackBar2;
        private Label label_width;
        private Label label_height;
        private CheckBox checkBox1;
    }
}