namespace SharedMemoryManager
{
    public class ImageData
    {
        public byte[] Data { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ImageData(byte[] data, int width, int height)
        {
            Data = data;
            Width = width;
            Height = height;
        }
    }
}
