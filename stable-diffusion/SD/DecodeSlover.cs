using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NcnnDotNet;
namespace stable_diffusion.SD
{
    internal class DecodeSlover
    {
        float[] factor = { 5.48998f, 5.48998f, 5.48998f, 5.48998f };

        float[] _mean_ = { -1.0f, -1.0f, -1.0f };
        float[] _norm_ = { 127.5f, 127.5f, 127.5f };

        Net net = new Net();

        public DecodeSlover(int h , int w)
            {
            net.Opt.UseVulkanCompute = true;
            net.Opt.UseWinogradConvolution = false;
            net.Opt.UseSgemmConvolution = false;
            net.Opt.UseFP16Packed = true;
            net.Opt.UseFP16Storage = true;
            net.Opt.UseFP16Arithmetic = true;
            net.Opt.UsePackingLayout = true;
           // net.LoadParam("./assets/AutoencoderKL-fp16.param");
            


           
            generate_param(h, w);
            net.LoadParam("assets/tmp-AutoencoderKL-" + h.ToString() + "-" +w.ToString() + "-fp16.param");
            net.LoadModel("./assets/AutoencoderKL-fp16.bin");
        }
        void generate_param(int height, int width)
        {
            string line;
            using (var decoderFile = new StreamReader("assets/AutoencoderKL-fp16.param"))
            using (var decoderFileNew = new StreamWriter($"assets/tmp-AutoencoderKL-{height}-{width}-fp16.param"))
            {
                int cnt = 0;
                while ((line = decoderFile.ReadLine()) != null)
                {
                    if (line.StartsWith("Reshape"))
                    {
                        if (cnt < 3)
                            line = line.Substring(0, line.Length - 12) + $"0={width * height / 8 / 8} 1=512";
                        else
                            line = line.Substring(0, line.Length - 15) + $"0={width / 8} 1={height / 8} 2=512";
                        cnt++;
                    }
                    decoderFileNew.WriteLine(line);
                }
            }
        }
       /* public void generate_param(int height, int width)
        {
            string line;
            StreamReader encoderFile = new StreamReader("assets/AutoencoderKL-fp16.param");
            StreamWriter encoderFileNew = new StreamWriter("assets/tmp-AutoencoderKL-encoder-" + height.ToString() + "-" + width.ToString() + "-fp16.param");

            int cnt = 0;
            while ((line = encoderFile.ReadLine()) != null)
            {
                if (line.Substring(0, 7) == "Reshape")
                {
                    switch (cnt)
                    {
                        case 0:
                            line = line.Substring(0, line.Length - 12) + "0=" + (width * height / 8 / 8).ToString() + " 1=512";
                            break;
                        case 1:
                            line = line.Substring(0, line.Length - 15) + "0=" + (width / 8).ToString() + " 1=" + (height / 8).ToString() + " 2=512";
                            break;
                        default:
                            break;
                    }

                    cnt++;
                }
                encoderFileNew.WriteLine(line);
            }
            encoderFileNew.Close();
            encoderFile.Close();
        }
       */
        internal NcnnDotNet.Mat decode(NcnnDotNet.Mat sample)
        {
            NcnnDotNet.Mat x_samples_ddim = new Mat();
            {
                float test0 = sample.Channel(0)[0];
                sample.SubstractMeanNormalize(new float[] { 0,0,0,0}, factor);
                float test1 = sample.Channel(0)[0];
                float test11 = sample[0];
                {
                    NcnnDotNet.Extractor ex = net.CreateExtractor();
                    ex.SetLiteMode(true);
                    ex.Input("input.1", sample);
                    ex.Extract("815", x_samples_ddim);
                    ex.Dispose();
                }
                float test2 = x_samples_ddim.Channel(0)[0];
                x_samples_ddim.SubstractMeanNormalize(_mean_, _norm_);
                float test3 = x_samples_ddim.Channel(0)[0];
            }

            return x_samples_ddim;
        }

    }
}
