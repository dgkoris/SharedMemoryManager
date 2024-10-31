namespace SharedMemoryManager
{
    public class ImageData
    {
        public byte[] Data { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public Dimensions ImageDimensions { get; set; }

        public ImageData(byte[] data, string name, int size, Dimensions imageDimensions)
        {
            Data = data;
            Name = name;
            Size = size;
            ImageDimensions = imageDimensions;
        }

        public override string ToString() => $"{Name}\t\t{Size} bytes\t{ImageDimensions}";
    }
}
