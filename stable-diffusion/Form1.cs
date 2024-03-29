using NcnnDotNet;
namespace stable_diffusion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int step;
        int seed;
        string positive_prompt;
        string negative_prompt;
        SD.PromptSlover prompt_slover;
        SD.DiffusionSlover diffusion_slover;
        SD.DecodeSlover decode_slover;
        bool first_time = false;
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Task trms = Task.Run(() => generate_img()); ;

           
        }

        void generate_img()
        {
            step = int.Parse(txtstep.Text);
            seed = int.Parse(txtseed.Text);
            positive_prompt = txtPosprompt.Text;
            negative_prompt = txtNegprompt.Text;
            int h = 512;
            int w = 512;
            int mode = 0;
            trackBar2.Invoke(new Action(() =>
            {
                h = ((trackBar2.Value + 2) * 128);
                w = ((trackBar1.Value + 2) * 128);
                if ( checkBox1.Checked) { mode = 1; } else { mode = 0; }
            }));
          

            if (!first_time)
            {
                log("----------------[init]--------------------");
                first_time = true;
                prompt_slover = new SD.PromptSlover(this);
                diffusion_slover = new SD.DiffusionSlover(this,h,w, mode);
                decode_slover = new SD.DecodeSlover( h, w);
            }

            log("----------------[prompt]------------------");
            NcnnDotNet.Mat cond = prompt_slover.get_conditioning(positive_prompt);
            NcnnDotNet.Mat uncond = prompt_slover.get_conditioning(negative_prompt);
            log("----------------[diffusion]---------------");
            Mat sample = diffusion_slover.sampler(seed, step, ref cond, ref uncond);


            log("----------------[decode]------------------");
            Mat x_samples_ddim = decode_slover.decode(sample);

            log("----------------[save]--------------------");

            
            Bitmap bit_img = save_bmp("./result_" + seed.ToString() + "_" + step.ToString() + ".bmp", x_samples_ddim);

         //   NcnnDotNet.OpenCV.Mat image = new NcnnDotNet.OpenCV.Mat(512, 512, NcnnDotNet.OpenCV.Cv2.CV_8UC3, x_samples_ddim.Data);
            //x_samples_ddim.ToPixels(image.Data, NcnnDotNet.PixelType.Rgb2Bgr);
           // NcnnDotNet.OpenCV.Cv2.ImWrite("./result_" + seed.ToString() + "_" + step.ToString() + ".png", image);
            x_samples_ddim.Dispose();
            sample.Dispose();
            cond.Dispose();
            uncond.Dispose();

            button1.Invoke(new Action(() =>
            {
                pictureBox1.Image = bit_img;
                button1.Enabled = true;
            }));
            System.GC.Collect();

        }

        public void log(string tmp)
        {
            textBox1.Invoke(new Action(() =>
            {
                textBox1.Text = textBox1.Text + tmp + Environment.NewLine;
            }));
        }
        Bitmap bmp = new Bitmap(512, 512);
        int c0 = 0;
        int c1 = 0;
        int c2 = 0;
        Bitmap save_bmp(string path,NcnnDotNet.Mat x_samples_ddim)
        {
           // Bitmap bmp = new Bitmap(512, 512);
            bmp = new Bitmap ( x_samples_ddim.W, x_samples_ddim.H );
            for (int j = 0; j < x_samples_ddim.H; j++)
            {
                for (int i = 0; i < x_samples_ddim.W; i++)
                {
                  int   c0 =(int) x_samples_ddim[((i + j * x_samples_ddim.W) ) +0]; //w ==512
                  int   c1 = (int)x_samples_ddim[((i + j * x_samples_ddim.W) ) + x_samples_ddim.W * x_samples_ddim.H];
                  int   c2 = (int)x_samples_ddim[((i + j * x_samples_ddim.W) ) + x_samples_ddim.W * x_samples_ddim.H];
                     
              //  int   c0 =  (int)x_samples_ddim.Channel(0)[i + j * 512];
              //  int   c1 =  (int)x_samples_ddim.Channel(1)[i + j * 512];
              //  int   c2 =  (int)x_samples_ddim.Channel(2)[i + j * 512];

                   bmp.SetPixel(i, j, Color.FromArgb(255, Clamp(c0, 0, 255), Clamp(c1, 0, 255), Clamp(c2,0,255))); ; //remap values that can be negative or over 255
                }
            }
            bmp.Save(path );
            return bmp;
        }
        int Clamp( int value, int min , int max)
        {
            if (value < min) { return min; }
            if (value > max) { return max; }
            return value;
        }


            private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label_width.Text = "Width: " +((trackBar1.Value +2 ) * 128).ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label_height.Text = "Height: " + ((trackBar2.Value + 2) * 128).ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}