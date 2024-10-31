namespace SharedMemoryManager
{
    public struct Dimensions
    {
        public readonly int Width;
        public readonly int Height;

        public Dimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString() => $"({Width}x{Height})";
    }
}
