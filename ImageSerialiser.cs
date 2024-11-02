using System;
using System.Collections.Generic;
using System.Text;

namespace SharedMemoryManager
{
    /// <summary>
    /// Provides methods to serialise images and retrieve image dimensions.
    /// </summary>
    public class ImageSerialiser
    {
        /// <summary>
        /// Serialises image data with metadata before writing to shared memory.
        /// </summary>
        /// <param name="images">The list of images to serialise.</param>
        /// <returns>A byte list containing the serialised image data and metadata.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="images"/> is null.</exception>
        public static List<byte> SerialiseImages(List<ImageData> images)
        {
            List<byte> serialisedData = new List<byte>();

            // 1. Write the number of images
            serialisedData.AddRange(BitConverter.GetBytes(images.Count));

            // 2. Write metadata for each image
            foreach (ImageData image in images)
            {
                serialisedData.AddRange(BitConverter.GetBytes(image.Data.Length));              // Byte size
                serialisedData.AddRange(BitConverter.GetBytes(image.ImageDimensions.Width));    // Width
                serialisedData.AddRange(BitConverter.GetBytes(image.ImageDimensions.Height));   // Height

                byte[] nameBytes = Encoding.UTF8.GetBytes(image.Name);
                serialisedData.AddRange(BitConverter.GetBytes(nameBytes.Length));               // Name length
                serialisedData.AddRange(nameBytes);                                             // Name bytes
            }

            // 3. Append image data
            foreach (ImageData image in images)
            {
                serialisedData.AddRange(image.Data);
            }

            return serialisedData;
        }

        /// <summary>
        /// Retrieves dimensions from a BMP image data byte array.
        /// </summary>
        /// <param name="bmpData">The BMP image data.</param>
        /// <returns>The dimensions of the BMP image.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="bmpData"/> is not in BMP format.</exception>
        public static Dimensions GetBmpImageDimensions(byte[] bmpData)
        {
            if (bmpData == null)
            {
                throw new ArgumentNullException(nameof(bmpData), "BMP data cannot be null.");
            }

            // For more information about BMP format offsets visit https://en.wikipedia.org/wiki/BMP_file_format and search for "BITMAPINFOHEADER header".
            int width = BitConverter.ToInt32(bmpData, 18);  // Offset for width in BMP format
            int height = BitConverter.ToInt32(bmpData, 22); // Offset for height in BMP format

            return new Dimensions(width, height);
        }
    }
}
