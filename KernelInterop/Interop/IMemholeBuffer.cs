using System;

namespace Wayz.Memhole.Kernel
{
    internal interface IMemholeBuffer : IDisposable
    {
        public unsafe byte* Buffer { get; protected set; }
        
        ulong SetSize(ulong size);

        ulong GetSize();

        ReadOnlySpan<byte> GetBuffer();

        void SetBuffer(ReadOnlySpan<byte> buffer);

    }
}
