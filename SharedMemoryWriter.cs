using System;
using System.IO.MemoryMappedFiles;

namespace SharedMemoryManager
{
    /// <summary>
    /// Manages writing data to shared memory with safe disposal of resources.
    /// </summary>
    public class SharedMemoryWriter : IDisposable
    {
        private readonly long _memorySize;
        private MemoryMappedFile _mappedFile;
        private MemoryMappedViewAccessor _viewAccessor;
        private bool _disposed;
        private readonly object _lockObject = new object();

        /// <summary>
        /// Initialises a SharedMemoryWriter instance with the specified shared memory name and size.
        /// </summary>
        /// <param name="sharedMemoryName">The name of the shared memory.</param>
        /// <param name="memorySize">The size of the shared memory in bytes.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sharedMemoryName"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="memorySize"/> is zero or negative.</exception>
        public SharedMemoryWriter(string sharedMemoryName, long memorySize)
        {
            if (string.IsNullOrEmpty(sharedMemoryName))
            {
                throw new ArgumentException("Shared memory name can't be null or empty.", nameof(sharedMemoryName));
            }
            if (memorySize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(memorySize), "Memory size must be positive.");
            }

            _memorySize = memorySize;
            _mappedFile = MemoryMappedFile.CreateOrOpen(sharedMemoryName, memorySize);
            _viewAccessor = _mappedFile.CreateViewAccessor();
        }

        /// <summary>
        /// Writes data to shared memory at the specified offset.
        /// </summary>
        /// <param name="data">The byte array containing the data to write.</param>
        /// <param name="offset">The byte offset in shared memory where data writing begins. Defaults to 0.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the data size exceeds shared memory capacity.</exception>
        public void WriteData(byte[] data, long offset = 0)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Data can't be null.");
            }
            if (data.Length + offset > _memorySize)
            {
                throw new ArgumentException("Data size exceeds shared memory capacity.");
            }

            try
            {
                lock (_lockObject)
                {
                    _viewAccessor.WriteArray(offset, data, 0, data.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to write data to shared memory: {e.Message}");
            }
        }

        /// <summary>
        /// Releases the resources used by the SharedMemoryWriter instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases resources used by the SharedMemoryWriter. Enables subclasses to extend the disposal pattern.
        /// </summary>
        /// <param name="disposing">Indicates whether the method is called from Dispose or the finaliser.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _viewAccessor?.Dispose();
                _mappedFile?.Dispose();
            }

            _disposed = true;
        }
    }
}
