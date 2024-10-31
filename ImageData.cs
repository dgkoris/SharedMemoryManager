namespace SharedMemoryManager
{
    public class ImageData
    {
        public byte[] Data { get; set; }
        public Dimensions ImageDimensions { get; set; }

        public ImageData(byte[] data, Dimensions imageDimensions)
        {
            Data = data;
            ImageDimensions = imageDimensions;
        }
    }
}
