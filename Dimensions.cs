namespace SharedMemoryManager
{
    /// <summary>
    /// Represents image dimensions.
    /// </summary>
    public struct Dimensions
    {
        public readonly int Width;
        public readonly int Height;

        /// <summary>
        /// Initialises a Dimensions instance.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        public Dimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Returns a string representation of the dimensions.
        /// </summary>
        /// <returns>A string representation.</returns>
        public override string ToString() => $"({Width}x{Height})";
    }
}
