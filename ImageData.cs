using System;

namespace SharedMemoryManager
{
    public class ImageData
    {
        public readonly string Name;
        public readonly int Size;
        public readonly Dimensions ImageDimensions;
        public readonly byte[] Data;

        public ImageData(string name, int size, Dimensions imageDimensions, byte[] data)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Size = size;
            ImageDimensions = imageDimensions;
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public override string ToString() => $"{Name,-25}\t{Size,8} bytes\t{ImageDimensions}";
    }
}
