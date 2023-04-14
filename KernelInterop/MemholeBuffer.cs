using System;
using System.Collections.Generic;
using System.Text;

namespace Wayz.Memhole.Kernel
{
    internal class MemholeBuffer : IMemholeBuffer
    {
        private ulong _size;

        private byte[] _buffer = Array.Empty<byte>();
        private object BufferLock = new object();

        public unsafe void UseUnsafeBuffer(Action<IntPtr> action)
        {
            lock (BufferLock)
            {
                fixed (byte* buffer = _buffer)
                {
                    action.Invoke((IntPtr) buffer);
                }
            }
        }

        public ReadOnlySpan<byte> GetBuffer()
        {
            return _buffer;
        }

        public ulong GetSize()
        {
            return _size;
        }

        public void SetBuffer(ReadOnlySpan<byte> buffer)
        {
            lock (BufferLock)
            {
                _size = (ulong)buffer.Length;
                _buffer = buffer.ToArray();
            }
        }

        public void SetBufferSize(ulong size)
        {
            _buffer = new byte[size];
        }

    }
}
