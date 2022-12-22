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

        public DecodeSlover()
            {
            net.Opt.UseVulkanCompute = false;
            net.Opt.UseWinogradConvolution = false;
            net.Opt.UseSgemmConvolution = false;
            net.Opt.UseFP16Packed = true;
            net.Opt.UseFP16Storage = true;
            net.Opt.UseFP16Arithmetic = true;
            net.Opt.UsePackingLayout = true;
            net.LoadParam("./assets/AutoencoderKL-fp16.param");
            net.LoadModel("./assets/AutoencoderKL-fp16.bin"); 
            


            }
        
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
