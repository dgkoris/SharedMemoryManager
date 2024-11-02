using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharedMemoryManager
{
    /// <summary>
    /// Main program to load BMP images, serialise them, and write data to shared memory.
    /// </summary>
    internal class Program
    {
        private const string SharedMemoryName = "Local\\SharedMemoryImages";
        private const int SharedMemorySize = 1024 * 1024 * 100; // 100MB of memory

        /// <summary>
        /// Entry point of the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        static void Main(string[] args)
        {
            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string imageFolderPath = Path.Combine(solutionDirectory, "TestImages");

            List<ImageData> images = LoadBmpImagesFromFolder(imageFolderPath);

            List<byte> serialisedData = ImageSerialiser.SerialiseImages(images);

            int totalImageSize = images.Sum(image => image.Size);

            Console.WriteLine($"\nTotal images: {images.Count,-24}{totalImageSize,10} bytes");

            using (SharedMemoryWriter writer = new SharedMemoryWriter(SharedMemoryName, SharedMemorySize))
            {
                writer.WriteData(serialisedData.ToArray());

                Console.WriteLine($"\nData written to shared memory '{SharedMemoryName}'.\n\nPress Enter to clear the shared memory.");
                Console.ReadLine();
            }

            Console.WriteLine("Shared memory cleared!\n\nPress Enter to exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Loads BMP images from the specified folder and retrieves their metadata.
        /// </summary>
        /// <param name="folderPath">The folder path containing BMP images.</param>
        /// <returns>A list of loaded <see cref="ImageData"/> objects.</returns>
        static List<ImageData> LoadBmpImagesFromFolder(string folderPath)
        {
            List<ImageData> images = new List<ImageData>();

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"Folder doesn't exist: {folderPath}");
                return images;
            }

            string[] imageFiles = Directory.GetFiles(folderPath, "*.bmp", SearchOption.AllDirectories);

            foreach (string imageFile in imageFiles)
            {
                string fileName = Path.GetFileName(imageFile);

                try
                {
                    byte[] imageBytes = File.ReadAllBytes(imageFile);
                    Dimensions dimensions = ImageSerialiser.GetBmpImageDimensions(imageBytes);

                    ImageData image = new ImageData(fileName, imageBytes.Length, dimensions, imageBytes);
                    images.Add(image);

                    Console.WriteLine($"Loaded image: {image}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error loading image {fileName}: {e.Message}");
                }
            }

            return images;
        }
    }
}
