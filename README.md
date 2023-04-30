# Stable-Diffusion.NET-NCNN

A c# port of [Stable-Diffusion-NCNN](https://github.com/EdVince/Stable-Diffusion-NCNN) using [NcnnDotNet](https://github.com/takuya-takeuchi/NcnnDotNet) libraries.
The code uses only CPU and requires 8GB ram.
Project was created with Visual Studio 2022

2023-05-01 update:
Added support for different size images and fast speed high ram mode.

## Usages

Download from [GDrive](https://drive.google.com/drive/folders/1myB4uIQ2K5okl51XDbmYhetLF9rUyLZS?usp=sharing) the 3 .bin models (about 2GB in total) and put them in the "[assets](https://github.com/andreae293/Stable-Diffusion.NET-NCNN/tree/main/stable-diffusion/bin/Debug/net6.0-windows/assets)" folder, then start the stable-diffusion.exe or compile it yourself.
The 3 .bin files are:

-AutoencoderKL-fp16.bin

-FrozenCLIPEmbedder-fp16.bin

-UNetModel-MHA-fp16.bin

![image](./img-example/sd.net.png)
