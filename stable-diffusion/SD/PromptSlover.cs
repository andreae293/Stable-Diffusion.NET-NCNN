using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NcnnDotNet;
using System.Text.RegularExpressions;
using System.IO;
namespace stable_diffusion.SD
{
    internal class PromptSlover
    {
        Dictionary<string, int> tokenizer_token2idx = new Dictionary<string, int>();
        Dictionary<int, string> tokenizer_idx2token = new Dictionary<int, string>();
        
        Net net = new Net();
        Form1 f;
        public PromptSlover(Form1 f)
        {
            this.f = f;
            net.Opt.UseVulkanCompute = true;
            net.Opt.UseWinogradConvolution = false;
            net.Opt.UseSgemmConvolution = false;
            net.Opt.UseFP16Packed = true;
            net.Opt.UseFP16Storage = true;
            net.Opt.UseFP16Arithmetic = true;
            net.Opt.UsePackingLayout = true;
            net.LoadParam("./assets/FrozenCLIPEmbedder-fp16.param");
            net.LoadModel("./assets/FrozenCLIPEmbedder-fp16.bin");
            string [] vocab = File.ReadAllLines("./assets/vocab.txt");
            for(int i = 0; i < vocab.Length; i++)
            {
                tokenizer_token2idx.Add(vocab[i], i);
                tokenizer_idx2token.Add(i, vocab[i]);
            }
        }

        internal unsafe NcnnDotNet.Mat get_conditioning(string prompt)
        {
            List<Tuple<string, float>> parsed = parse_prompt_attention(prompt);
            List<List<int>> tokenized = new List<List<int>>();

            for (int p = 0; p < parsed.Count; p++)
            {
                List<string> tokens = split(parsed[p].Item1);
                List<int> ids = new List<int>();
                for (int i = 0; i < tokens.Count; i++)
                {
                    if (!tokenizer_token2idx.ContainsKey(tokens[i]))
                    {
                        tokenizer_token2idx.Add(tokens[i], 0);
                    }
                    ids.Add(tokenizer_token2idx[tokens[i]]);
                }
                tokenized.Add(ids);
            }

            List<int> remade_tokens = new List<int>();
            List<float> multipliers = new List<float>();
            {
                int last_comma = -1;

            for (int it_tokenized = 0; it_tokenized < tokenized.Count(); it_tokenized++)
            {
                List<int> tokens = tokenized[it_tokenized];
                float weight = parsed[it_tokenized].Item2;
                int i = 0;
                while (i < tokens.Count())
                {
                    int token = tokens[i];
                    if (token == 267)
                    {
                        last_comma = remade_tokens.Count();
                    }
                    else if ((Math.Max(remade_tokens.Count(), 1) % 75 == 0) && (last_comma != -1) && (remade_tokens.Count() - last_comma <= 20))
                    {
                        last_comma += 1;
                        List<int> reloc_tokens = remade_tokens.Skip<int>(last_comma).ToList<int>(); 
                        List<float> reloc_mults = multipliers.Skip<float>(last_comma).ToList<float>();
                        List<int> _remade_tokens_ = remade_tokens.Take<int>(last_comma).ToList<int>();
                        remade_tokens = _remade_tokens_;
                        int length = remade_tokens.Count();
                        int rem = (int)(Math.Ceiling(length / 75.0) * 75 - length);
                        List<int> tmp_tokent = new List<int>(Enumerable.Repeat(49407, rem));
                        remade_tokens = remade_tokens.Concat<int>(tmp_tokent).ToList<int>();
                        remade_tokens = remade_tokens.Concat<int>(reloc_tokens).ToList<int>();
                        List<float> _multipliers = multipliers.Take(multipliers.Count + last_comma).ToList<float>();
                        List<float> tmp_multiplierst = new List<float>(Enumerable.Repeat(1.0F, rem));
                        _multipliers = _multipliers.Concat<float>(tmp_multiplierst).ToList<float>();
                        _multipliers = _multipliers.Concat<float>(reloc_mults).ToList<float>();
                        multipliers = _multipliers;
                    }
                    remade_tokens.Add(token);
                    multipliers.Add(weight);
                    i += 1;

                }

            }
            int prompt_target_length = (int)Math.Ceiling(Math.Max(remade_tokens.Count(), 1) / 75.0) * 75;
            int tokens_to_add = prompt_target_length - remade_tokens.Count();
            List<int> tmp_token = new List<int>(Enumerable.Repeat(49407, tokens_to_add));
            remade_tokens = remade_tokens.Concat<int>(tmp_token).ToList<int>();
            List<float> tmp_multipliers = new List<float>(Enumerable.Repeat(1.0F, tokens_to_add));
            multipliers = multipliers.Concat<float>(tmp_multipliers).ToList<float>();
        }
            Mat conds = new Mat(768, 0);
            {
                while (remade_tokens.Count() > 0) 
                {
                    List<int> rem_tokens = remade_tokens.Skip(75).ToList<int>();
                    List<float> rem_multipliers = multipliers.Skip(75).ToList<float>();
                    List<int> current_tokens = new List<int>();
                    List<float> current_multipliers = new List<float>();
                    if (remade_tokens.Count() > 0)
                    {
                        current_tokens = current_tokens.Concat<int>(remade_tokens.Take(75)).ToList<int>();
                        current_multipliers = current_multipliers.Concat<float>(multipliers.Take(75)).ToList<float>();
                        //  current_tokens.insert(current_tokens.end(), remade_tokens.begin(), remade_tokens.begin() + 75);
                        //  current_multipliers.insert(current_multipliers.end(), multipliers.begin(), multipliers.begin() + 75);
                    }
                    else
                    {
                        List<int> tmp_token = new List<int>(Enumerable.Repeat(49407, 75));
                        current_tokens = current_tokens.Concat<int>(tmp_token).ToList<int>();
                        List<float> tmp_multipliers = new List<float>(Enumerable.Repeat(1.0F, 75));
                        current_multipliers=current_multipliers.Concat<float>(tmp_multipliers).ToList<float>();

                    }

                    {
                        Mat token_mat = new Mat(77);
                        token_mat.Fill(49406);
                        Mat multiplier_mat = new Mat(77);
                        multiplier_mat.Fill(1.0f);

                        int* token_ptr = (int*)token_mat.Data;

                        for (int i = 0; i < 75; i++)
                        {
                            token_ptr[i +1 ] = current_tokens[i]; // tokens must be written like an int even if token_mata.data is a float [] type
                     //       token_mat[i +1] = current_tokens[i];
                            multiplier_mat[i +1] = current_multipliers[i];
                        }
                        var test0 = multiplier_mat[0];
                        var test1 = multiplier_mat[1];
                        var test2 = multiplier_mat[2];
                        Extractor ex = net.CreateExtractor();
                        ex.SetLiteMode(true);
                        ex.Input("token", token_mat);
                        ex.Input("multiplier", multiplier_mat);
                        ex.Input("cond", conds);
                        Mat new_conds = new Mat();
                        ex.Extract("conds", new_conds);
                        conds = new_conds;
                    }

                    remade_tokens = rem_tokens;
                    multipliers = rem_multipliers;
                }
               
            }
            return conds;


        }


            List<Tuple<string,float>> parse_prompt_attention(string texts)
        {
            List<Tuple<string, float>> res = new List<Tuple<string, float>>();
            Stack<int> round_brackets = new Stack<int>();
            Stack<int> square_brackets = new Stack<int>();
            const float round_bracket_multiplier = 1.1F;
            const float square_bracket_multiplier = 1 / 1.1F;
            List<string> ms = new List<string>();

            for(int i = 0; i<texts.Length;i++)
            {
                string s = texts.Substring(i, 1);
                if (s == "(" || s == "[" || s == ")" || s == "]")
                {
                    ms.Add(s);
                }
                else
                {
                    if(ms.Count <1)
                    { ms.Add(""); }
                    string last = ms[ms.Count - 1];
                    if (last == "(" || last == "[" || last == ")" || last == "]")
                    {
                        ms.Add("");
                    }
                    ms[ms.Count() - 1] += s;
                }
            }

            for (int i = 0; i < ms.Count; i++)
            {
                string text = ms[i];
                if (text == "(")
                {
                    round_brackets.Push(res.Count());
                }
                else if (text == "[")
                {
                    square_brackets.Push(res.Count());
                }
                else if (text == ")" && round_brackets.Count() > 0)
                {
                    for (int p = round_brackets.First(); p < res.Count(); p++)
                    {
                        res[p] = new Tuple<string, float>(res[p].Item1, res[p].Item2 * round_bracket_multiplier);
                    }
                    round_brackets.Pop();
                }
                else if (text == "]" && square_brackets.Count() > 0)
                {
                    for (int p = square_brackets.First(); p < res.Count(); p++)
                    {
                        res[p] = new Tuple<string, float>(res[p].Item1, res[p].Item2 * square_bracket_multiplier);
                    }
                    square_brackets.Pop();

                }
                else
                {
                    res.Add( new Tuple<string, float>(text,1.0F));
                }

            }
            while (round_brackets.Count() != 0)
            {
                for (int p = round_brackets.First(); p < res.Count(); p++)
                {
                    res[p] = new Tuple<string, float>(res[p].Item1, res[p].Item2 * round_bracket_multiplier);

                }
                round_brackets.Pop();
            }

            while (square_brackets.Count() != 0)
            {
                for (int p = square_brackets.First(); p < res.Count(); p++)
                {
                    res[p] = new Tuple<string, float>(res[p].Item1, res[p].Item2 * square_bracket_multiplier);
                }
                square_brackets.Pop();
            }

            int j = 0;
            while (j + 1 < res.Count())
            {
                if (res[j].Item2 == res[j + 1].Item2)
                {
                    res[j] = new Tuple<string, float>(res[j].Item1 + res[j + 1].Item1, res[j].Item2 );
                    
                    res.RemoveAt( j + 1); 
                }
                else
                {
                    j += 1;
                }
            }


            return res;
        }


        string whitespace_clean(string text)
        {
            return Regex.Replace(text, @"\s+", " ");
        }



        List<string> split(string str)
        {
            int pos;
            List<string> result = new List<string>();
            str += " ";
            int size = str.Length;
            for (int i = 0; i < size; i++)
            {
                int i0 = str.IndexOf(" ", i);
                int i1 = str.IndexOf(",", i);
                if(i0 <0 ) { i0 = 9999999; }
                if (i1 < 0) { i1 = 9999990; }
                pos = Math.Min(i0,i1);
                if (pos <0 ) { return result; }
                if (pos < size)
                {
                    string s = str.Substring(i, pos - i);
                    string pat = str.Substring(pos, 1);

                    if (s.Length > 0)
                    { result.Add(s + "</w>"); }
                    if (pat != " ")
                    { result.Add(pat + "</w>"); }
                    i = pos;
                }
            }
            return result;
        }


    }
}
