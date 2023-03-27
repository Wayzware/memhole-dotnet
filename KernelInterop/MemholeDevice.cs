using System;
using System.Collections.Generic;
using System.Text;

namespace Wayz.Memhole.Kernel
{
    public sealed unsafe class MemholeDevice : IMemholeDevice
    {
        private IMemholeBuffer _buffer;
        private readonly void* _memhole;

        public MemholeDevice()
        {
            _buffer = new MemholeBuffer();
            _memhole = StaticCppInterop.memhole_create();
            if (_memhole == null || _memhole == (void*)0)
            {
                throw new OutOfMemoryException("Could not allocate memhole_t struct in C++ interop library.");
            }
        }

        public void AttachToPid(int pid)
            => ThrowIfExceptional(StaticCppInterop.memhole_attach_to_pid(_memhole, pid));

        //public void AttachToPids(int pid, int pid2)
        //    => ThrowIfExceptional(StaticCppInterop.memhole_attach_to_pids(_memhole, pid, pid2));

        //public void AttachToSecondaryPid(int pid)
        //    => ThrowIfExceptional(StaticCppInterop.memhole_attach_secondary_pid(_memhole, pid));

        public void Connect()
            => ThrowIfExceptional(StaticCppInterop.memhole_connect(_memhole));
        public void Disconnect()
            => ThrowIfExceptional(StaticCppInterop.memhole_disconnect(_memhole));

        public ReadOnlySpan<byte> Read(long len)
        {
            _buffer.SetBufferSize((ulong)len);
            _buffer.UseUnsafeBuffer((ptr) =>
            {
                byte* data = (byte*)ptr;
                ThrowIfExceptional(StaticCppInterop.memhole_read(_memhole, data, len, 1));
            });
            return _buffer.GetBuffer();
        }
        public ReadOnlySpan<byte> ReadFrom(long position, long len)
        {
            SetMemoryPosition(position);
            return Read(len);
        }
        public long SetMemoryPosition(long position)
            => ThrowIfExceptional(StaticCppInterop.memhole_set_mem_pos(_memhole, position, (int)MemholeParallelMode.SKMFAST));
        
        private long Write(ReadOnlySpan<byte> data)
        {
            int dataLen = data.Length;
            _buffer.SetBuffer(data);
            _buffer.UseUnsafeBuffer((ptr) =>
            {
                byte* dataPtr = (byte*)ptr;
                ThrowIfExceptional(StaticCppInterop.memhole_write(_memhole, dataPtr, dataLen, 1));
            });
            return dataLen;
        }
        private long WriteTo(long position, ReadOnlySpan<byte> data)
        {
            SetMemoryPosition(position);
            return Write(data);
        }

        private long ThrowIfExceptional(long returnVal)
        {
            if(returnVal >= 0)
            {
                return returnVal;
            }
            if (!Enum.TryParse<ErrorCodes>((-returnVal).ToString(), out var ec))
            {
                return returnVal;
            }

            throw ec switch
            {
                ErrorCodes.EINVDEV => new MemholeInvalidDeviceException(),
                ErrorCodes.EMEMHNF => new MemholeDeviceNotFoundException(),
                ErrorCodes.EMEMBSY => new MemholeDeviceBusyException(),
                ErrorCodes.EINVPID => new MemholeInvalidPidException(),
                ErrorCodes.EKMALOC => new MemholeKernelMallocFailureException(),
                ErrorCodes.EUSPOPN => new MemholeUnsupportedOperationException(),
                _ => new NotSupportedException($"Unknown error code {ec}")
            };
        }

        public void Dispose()
        {
            StaticCppInterop.memhole_disconnect(_memhole);
            StaticCppInterop.memhole_delete(_memhole);
        }
    }
}
