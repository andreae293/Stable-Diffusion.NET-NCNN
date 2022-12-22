using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NcnnDotNet.Layers;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using NcnnDotNet;
namespace stable_diffusion.SD
{
    internal class DiffusionSlover
    {
        Net net = new Net();
        float[] log_sigmas = new float[1000]; 
        public DiffusionSlover(Form1 f)
        {
            this.f = f;
            net.Opt.UseWinogradConvolution = false;
            net.Opt.UseVulkanCompute = false;
            net.Opt.UseSgemmConvolution = false;
            net.Opt.UseSgemmConvolution = false;
            net.Opt.UseFP16Packed = true;
            net.Opt.UseFP16Storage = true;
            net.Opt.UseFP16Arithmetic = true;
            net.Opt.UsePackingLayout = true;
            net.LoadParam("./assets/UNetModel-fp16.param");
            net.LoadModel("./assets/UNetModel-fp16.bin");

            byte[] tmp_log_sigmas = File.ReadAllBytes("./assets/log_sigmas.bin");
            Buffer.BlockCopy(tmp_log_sigmas, 0, log_sigmas, 0, tmp_log_sigmas.Length);
            float tmp0 =   log_sigmas[0];



        }

        float sample_gaus(Random r)
        {
            double u1 = 1.0 - r.NextDouble(); 
            double u2 = 1.0 - r.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return (float)( 0 + 1 * randStdNormal); // 0 mean 1 standard deviation
        }

        NcnnDotNet.Mat randn_4_64_64(int seed)
        {
            Random rand = new Random(seed); 
            
            NcnnDotNet.Mat cv_x = new NcnnDotNet.Mat(64, 64, 4);
            //(cv::Size(64, 64), CV_32FC4);

            for (int ct = 0; ct < 4; ct++)
            {

                for (int hw = 0; hw < cv_x.W * cv_x.H; hw++)
                {
                    cv_x.Channel(ct)[hw] =sample_gaus(rand); //fill the matrix with a random number with gaussian distribussion
                }
            }

            return cv_x;
        }

        NcnnDotNet.Mat CFGDenoiser_CompVisDenoiser(ref NcnnDotNet.Mat input, float sigma, NcnnDotNet.Mat cond, NcnnDotNet.Mat uncond)
        {

            float c_out = -1.0F * sigma;
            float c_in = (float)(1.0 / Math.Sqrt(sigma * sigma + 1));
            float log_sigma = (float)Math.Log(sigma);

            List<float> dists = new List<float>(1000);
            for (int i = 0; i < 1000; i++)
            {
                dists.Add(0);
                if (log_sigma - log_sigmas[i] >= 0)
                { dists[i] = 1; }
                else
                { dists[i] = 0; }
                if (i == 0) { continue; }
                dists[i] += dists[i - 1];
            }

            int low_idx = Math.Min((int)(dists.Max()), 1000 - 2);
            int high_idx = low_idx + 1;
            float low = log_sigmas[low_idx];
            float high = log_sigmas[high_idx];
            float w = (low - log_sigma) / (low - high);
            w = Math.Max(0.0f, Math.Min(1.0f, w));
            float t = (1 - w) * low_idx + w * high_idx;

            NcnnDotNet.Mat t_mat = new NcnnDotNet.Mat(1);
            t_mat[0] = t;

            NcnnDotNet.Mat c_in_mat = new NcnnDotNet.Mat(1);
            c_in_mat[0] = c_in;

            NcnnDotNet.Mat c_out_mat = new NcnnDotNet.Mat(1);
            c_out_mat[0] = c_out;

            NcnnDotNet.Mat denoised_cond = new NcnnDotNet.Mat();
            {
                NcnnDotNet.Extractor ex = net.CreateExtractor();
                ex.SetLiteMode(true);
                ex.Input("in0", input);
                ex.Input("in1", t_mat);
                ex.Input("in2", cond);
                ex.Input("c_in", c_in_mat);
                ex.Input("c_out", c_out_mat);
                ex.Extract("outout", denoised_cond);
                ex.Dispose();
            }

            NcnnDotNet.Mat denoised_uncond = new NcnnDotNet.Mat();
            {
                NcnnDotNet.Extractor ex = net.CreateExtractor();
                ex.SetLiteMode(true);
                ex.Input("in0", input);
                ex.Input("in1", t_mat);
                ex.Input("in2", uncond);
                ex.Input("c_in", c_in_mat);
                ex.Input("c_out", c_out_mat);
                ex.Extract("outout", denoised_uncond);
                ex.Dispose();
            }


            for (int c = 0; c < 4; c++)
            {

                for (int hw = 0; hw < 64 * 64; hw++)
                {
                    denoised_uncond.Channel(c)[hw] = denoised_uncond.Channel(c)[hw] + 7 * (denoised_cond.Channel(c)[hw] - denoised_uncond.Channel(c)[hw]);

                }
            }
            t_mat.Dispose();
            c_in_mat.Dispose();
            c_out_mat.Dispose();
            denoised_cond.Dispose();
            return denoised_uncond;
        }
        Form1 f;
        void log_text(string tmp)
        {
            f.log(tmp);
        }
         internal NcnnDotNet.Mat sampler(int seed, int step, ref NcnnDotNet.Mat c, ref NcnnDotNet.Mat uc)
        {
            NcnnDotNet.Mat x_mat = randn_4_64_64(seed % 1000);
            float test0=    x_mat.Channel(0)[0];
            List<float> sigma = new List<float>(step);
            float delta = 0.0F - 999.0F / (step - 1);
            for (int i = 0; i < step; i++)
            {
                float t = 999.0F + i * delta;
                int low_idx = (int)Math.Floor(t);
                int high_idx = (int)Math.Ceiling(t);
                float w = t - low_idx;
                sigma.Add(0.0F);
                sigma[i] = (float)Math.Exp((1 - w) * log_sigmas[low_idx] + w * log_sigmas[high_idx]);
            }
            sigma.Add(0.0F);
            float[] _norm_ = { sigma[0], sigma[0], sigma[0], sigma[0] };
            x_mat.SubstractMeanNormalize(null, _norm_);
            Stopwatch stopWatch = new Stopwatch();
            {
                for (int i = 0; i < sigma.Count() - 1; i++)
                {
                    log_text("step:" + i.ToString());
                    stopWatch.Reset();
                    stopWatch.Start();
                    NcnnDotNet.Mat denoised = CFGDenoiser_CompVisDenoiser(ref x_mat, sigma[i], c, uc);
                    stopWatch.Stop();
                    log_text(stopWatch.Elapsed.TotalMilliseconds + "ms");

                    float sigma_up = Math.Min(sigma[i + 1], (float)Math.Sqrt(sigma[i + 1] * sigma[i + 1] * (sigma[i] * sigma[i] - sigma[i + 1] * sigma[i + 1]) / (sigma[i] * sigma[i])));
                    float sigma_down = (float)Math.Sqrt(sigma[i + 1] * sigma[i + 1] - sigma_up * sigma_up);

                    Random r = new Random(seed +i);
                    NcnnDotNet.Mat randn = randn_4_64_64(r.Next());

                    for (int ct = 0; ct < 4; ct++)
                    {

                        for (int hw = 0; hw < 64 * 64; hw++)
                        {
                            x_mat.Channel(ct)[hw] = x_mat.Channel(ct)[hw] + ((x_mat.Channel(ct)[hw] - denoised.Channel(ct)[hw]) / sigma[i]) * (sigma_down - sigma[i]) + randn.Channel(ct)[hw] * sigma_up;

                        }
                    }
                    randn.Dispose();
                    denoised.Dispose();

                }
            }



            return x_mat;
        }


        unsafe NcnnDotNet.Mat Clone(NcnnDotNet.Mat from)
        {

            if (from.IsEmpty)
            { return new NcnnDotNet.Mat(); }

            NcnnDotNet.Mat m = new NcnnDotNet.Mat();
            m.CreateLike(from, from.Allocator);
            if (m.Total > 0)
            {
                Buffer.MemoryCopy(from.Data.ToPointer(), m.Data.ToPointer(), (long)from.Total * from.ElemSize, (long)from.Total * from.ElemSize);
             
            }

            return m;
        }
    }
}
