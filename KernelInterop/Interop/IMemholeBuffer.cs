using System;

namespace Wayz.Memhole.Kernel
{
    internal interface IMemholeBuffer
    {
        ulong GetSize();

        ReadOnlySpan<byte> GetBuffer();

        void SetBuffer(ReadOnlySpan<byte> buffer);

        void UseUnsafeBuffer(Action<IntPtr> action);

        void SetBufferSize(ulong size);
    }
}
