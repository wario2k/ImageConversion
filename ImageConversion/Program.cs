using ImageMagick;
using System;
using System.Collections.Generic;
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
                if (Converter.FromFolder(args[0]))
                {
                    Console.WriteLine("Converted Images to WebP.");
                }
                else
                    Console.WriteLine("Failed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class Converter
    {
        public static bool FromFolder(string folder)
        {
            var files = Directory.GetFiles(folder);
            List<string> images = new List<string>();
            foreach (var file in files)
            {
                if (Path.GetExtension(file) == ".jpg")
                {
                    images.Add(file);
                }
            }
            return GenerateWebP(images, folder);
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

        private static bool GenerateWebP(List<string> Folder, string root)
        {
            try
            {
                foreach (var file in Folder)
                {
                    MagickImage img = new MagickImage(file);
                    img.Format = MagickFormat.WebP;
                    img.Quality = 75;
                    img.Write(Path.Combine(root, $"{Path.GetFileNameWithoutExtension(file)}.webp"));
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
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