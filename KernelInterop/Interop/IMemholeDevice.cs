using System;
using System.Collections.Generic;
using System.Text;

namespace Wayz.Memhole.Kernel
{
    /// <summary>
    /// A MemholeDevice should be a controller of memhole. 
    /// </summary>
    internal interface IMemholeDevice : IDisposable
    {
        int Connect();

        int Disconnect();

        long AttachToPid(int pid);

        long AttachToSecondaryPid(int pid);

        long AttachToPids(int pid, int pid2);

        long SetMemoryPosition(long position);

        long GetMemoryPosition();

        ReadOnlySpan<byte> Read(long len);

        ReadOnlySpan<byte> ReadFrom(long position, long len);

        long Write(ReadOnlySpan<byte> data);

        long WriteTo(long position, ReadOnlySpan<byte> data);
    }
}
