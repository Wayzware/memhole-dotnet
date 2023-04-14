using System;
using System.Collections.Generic;
using System.Text;

namespace Wayz.Memhole.Kernel
{
    /// <summary>
    /// A controller/manager of memhole
    /// </summary>
    public interface IMemholeDevice : IDisposable
    {
        /// <summary>
        /// Connect the memhole device to the kernel level driver
        /// </summary>
        /// <exception cref="MemholeInvalidDeviceException"></exception>
        void Connect();

        /// <summary>
        /// Disconnect the memhole device from the kernel level driver
        /// </summary>
        /// <exception cref="MemholeInvalidDeviceException"></exception>
        void Disconnect();

        /// <summary>
        /// Attach memhole to a process's memory
        /// </summary>
        /// <param name="pid">Process ID to attach to</param>
        /// <exception cref="MemholeInvalidDeviceException"></exception>
        /// <exception cref="MemholeInvalidPidException"></exception>
        void AttachToPid(int pid);

        /// <summary>
        /// Sets the memory position for a read or write operation.
        /// </summary>
        /// <param name="position">The memory address to read or write from</param>
        /// <returns>The memory address seeked to</returns>
        long SetMemoryPosition(long position);

        /// <summary>
        /// Read memory from the target position
        /// </summary>
        /// <param name="len">The number of bytes to read</param>
        /// <returns>The memory read</returns>
        ReadOnlySpan<byte> Read(long len);

        /// <summary>
        /// Read memory from the target position
        /// </summary>
        /// <param name="position">The memory address to read from</param>
        /// <param name="len">The number of bytes to read</param>
        /// <returns>The memory read</returns>
        ReadOnlySpan<byte> ReadFrom(long position, long len);

        // Coming soon
        /// <summary>
        /// Write memory to the target position
        /// </summary>
        /// <param name="data">The data to write</param>
        /// <returns>The number of bytes written</returns>
        long Write(ReadOnlySpan<byte> data);

        // Coming soon
        /// <summary>
        /// Write memory to the target position
        /// </summary>
        /// <param name="position">The memory address to write to</param>
        /// <param name="data">The data to be written</param>
        /// <returns>The number of bytes written</returns>
        long WriteTo(long position, ReadOnlySpan<byte> data);
    }
}
