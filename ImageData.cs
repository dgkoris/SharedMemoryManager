using System;

namespace SharedMemoryManager
{
    /// <summary>
    /// Represents image data.
    /// </summary>
    public class ImageData
    {
        public readonly string Name;
        public readonly int Size;
        public readonly Dimensions ImageDimensions;
        public readonly byte[] Data;

        /// <summary>
        /// Initialises an ImageData instance.
        /// </summary>
        /// <param name="name">Image name.</param>
        /// <param name="size">Image size in bytes.</param>
        /// <param name="imageDimensions">Image dimensions.</param>
        /// <param name="data">Image data as a byte array.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ImageData(string name, int size, Dimensions imageDimensions, byte[] data)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Size = size;
            ImageDimensions = imageDimensions;
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// Returns a string representation of the image data.
        /// </summary>
        /// <returns>A string representation.</returns>
        public override string ToString() => $"{Name,-25}\t{Size,8} bytes\t{ImageDimensions}";
    }
}
