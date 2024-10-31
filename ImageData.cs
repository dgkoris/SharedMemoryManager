namespace SharedMemoryManager
{
    public class ImageData
    {
        public byte[] Data { get; }
        public string Name { get; }
        public int Size { get; }
        public Dimensions ImageDimensions { get; }

        public ImageData(byte[] data, string name, int size, Dimensions imageDimensions)
        {
            Data = data;
            Name = name;
            Size = size;
            ImageDimensions = imageDimensions;
        }

        public override string ToString() => $"{Name,-25}\t{Size,8} bytes\t{ImageDimensions}";
    }
}
