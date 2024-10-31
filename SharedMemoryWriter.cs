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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Enables derived classes to extend the disposal pattern. 
        /// When overriding, subclasses can add their own resource cleanup without changing the base class's disposal logic.
        /// </summary>
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
