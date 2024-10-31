namespace SharedMemoryManager
{
    public struct Dimensions
    {
        public int Width { get; }
        public int Height { get; }

        public Dimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString() => $"{Width}x{Height}";
    }
}
