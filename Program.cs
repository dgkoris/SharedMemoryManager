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

            var images = LoadBmpImagesFromFolder(imageFolderPath);

            ImageSerialiser serialiser = new ImageSerialiser();
            List<byte> serialisedData = serialiser.SerialiseImages(images);

            // TODO: Send to shared memory.

            Console.WriteLine($"Serialised data size: {serialisedData.Count} bytes.\n\nPress Enter to exit...");
            Console.ReadLine();
        }

        static List<ImageData> LoadBmpImagesFromFolder(string folderPath)
        {
            var images = new List<ImageData>();
            
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"Folder doesn't exist: {folderPath}");
                return images;
            }

            string[] imageFiles = Directory.GetFiles(folderPath, "*.bmp", SearchOption.AllDirectories);

            ImageSerialiser serialiser = new ImageSerialiser();

            foreach (var imageFile in imageFiles)
            {
                try
                {
                    byte[] imageBytes = File.ReadAllBytes(imageFile);
                    var dimensions = serialiser.GetBmpImageDimensions(imageBytes);

                    images.Add(new ImageData(imageBytes, dimensions));

                    Console.WriteLine($"Loaded image ({dimensions.ToString()}): {Path.GetFileName(imageFile)}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error loading image {Path.GetFileName(imageFile)}: {e.Message}");
                }
            }

            return images;
        }
    }
}
