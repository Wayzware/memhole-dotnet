using System;
using System.Collections.Generic;
using System.Text;

namespace Wayz.Memhole.Kernel
{
    public sealed class MemholeDevice : IMemholeDevice
    {
        public long AttachToPid(int pid)
        {
            throw new NotImplementedException();
        }

        public long AttachToPids(int pid, int pid2)
        {
            throw new NotImplementedException();
        }

        public long AttachToSecondaryPid(int pid)
        {
            throw new NotImplementedException();
        }

        public int Connect()
        {
            throw new NotImplementedException();
        }

        public int Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public long GetMemoryPosition()
        {
            throw new NotImplementedException();
        }

        public ReadOnlySpan<byte> Read(long len)
        {
            throw new NotImplementedException();
        }

        public ReadOnlySpan<byte> ReadFrom(long position, long len)
        {
            throw new NotImplementedException();
        }

        public long SetMemoryPosition(long position)
        {
            throw new NotImplementedException();
        }

        public long Write(ReadOnlySpan<byte> data)
        {
            throw new NotImplementedException();
        }

        public long WriteTo(long position, ReadOnlySpan<byte> data)
        {
            throw new NotImplementedException();
        }

        private long 
    }
}
