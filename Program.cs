using System;
using System.Collections.Generic;
using System.IO;

namespace SharedMemoryManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string imageFolderPath = Path.Combine(solutionDirectory, "TestImages");

            var images = LoadImagesFromFolder(imageFolderPath);

            Console.WriteLine($"Loaded {images.Count} images into memory. Press Enter to exit.");
            Console.ReadLine();
        }

        static List<byte[]> LoadImagesFromFolder(string folderPath)
        {
            var imageList = new List<byte[]>();

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"Folder doesn't exist: {folderPath}");
                return imageList;
            }

            string[] imageFiles = Directory.GetFiles(folderPath, "*.bmp", SearchOption.AllDirectories);

            foreach (var imageFile in imageFiles)
            {
                try
                {
                    byte[] imageBytes = File.ReadAllBytes(imageFile);
                    imageList.Add(imageBytes);
                    Console.WriteLine($"Loaded image: {Path.GetFileName(imageFile)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading image {Path.GetFileName(imageFile)}: {ex.Message}");
                }
            }

            return imageList;
        }
    }
}
