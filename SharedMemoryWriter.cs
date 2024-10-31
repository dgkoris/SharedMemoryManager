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

        public SharedMemoryWriter(string sharedMemoryName, long memorySize)
        {
            if (string.IsNullOrEmpty(sharedMemoryName))
            {
                throw new ArgumentException("Shared memory name cannot be null or empty.", nameof(sharedMemoryName));
            }
            if (memorySize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(memorySize), "Memory size must be positive.");
            }

            _memorySize = memorySize;

            _mappedFile = MemoryMappedFile.CreateOrOpen(sharedMemoryName, memorySize);
            _viewAccessor = _mappedFile.CreateViewAccessor();
        }

        public void WriteData(byte[] data)
        {
            if (data.Length > _memorySize)
            {
                throw new ArgumentException("Data size exceeds shared memory capacity.");
            }

            _viewAccessor.WriteArray(0, data, 0, data.Length);
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
