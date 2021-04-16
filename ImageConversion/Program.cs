using ImageMagick;
using System;
using System.Diagnostics;
using System.IO;

namespace ImageConversion
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Converter.FromFolder(@"C:\Users\ashrestha\source\repos\ImageConversion\ImageConversion\j2k_bscan\");
            }
            catch (Exception ex)
            {
            }
            Console.Read();
        }
    }

    public class Converter
    {
        public static void FromFolder(string folder)
        {
            var files = Directory.GetFiles(folder);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //generate jpeg
            long dirSizeJpeg = DirSize(GenerateJpeg(files));
            timer.Stop();
            Console.WriteLine($"Time to process {files.Length} images to jpeg = {timer.ElapsedMilliseconds} ms, size of directory containing all images = ~{dirSizeJpeg / 1000}kb");
            timer.Restart();
            long dirSizeWebP = DirSize(GenerateWebP(files));
            timer.Stop();
            Console.WriteLine($"Time to process {files.Length} images to webP = {timer.ElapsedMilliseconds} ms, size of directory containing all images = ~{dirSizeWebP / 1000}kb");
            //generate webP
        }

        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        private static DirectoryInfo GenerateJpeg(string[] files)
        {
            //create output dir
            if (!Directory.Exists("./JpegImages"))
                Directory.CreateDirectory("./JpegImages");
            foreach (var file in files)
            {
                MagickImage img = new MagickImage(file);
                img.Format = MagickFormat.Jpeg;
                img.Write($"./JpegImages/{Path.GetFileNameWithoutExtension(file)}.jpg");
            }
            return new DirectoryInfo("./JpegImages");
        }

        private static DirectoryInfo GenerateWebP(string[] files)
        {
            //create output dir
            if (!Directory.Exists("./WebPImages"))
                Directory.CreateDirectory("./WebPImages");
            foreach (var file in files)
            {
                MagickImage img = new MagickImage(file);
                img.Format = MagickFormat.WebP;
                img.Write($"./WebPImages/{Path.GetFileNameWithoutExtension(file)}.webp");
            }
            return new DirectoryInfo("./WebPImages");
        }
    }
}