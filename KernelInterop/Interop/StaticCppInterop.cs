using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Wayz.Memhole.Kernel
{
    /// <summary>
    /// These functions are direct maps of those in version 1.7 of the C wrapper (wrappers/C/memhole.h).
    /// Requires Memhole v1.4.x or newer for all features
    /// </summary>
    internal static class StaticCppInterop
    {
        private const string LibraryPath = "memhole";

        [DllImport(LibraryPath)]
        internal static extern unsafe void* memhole_create();

        [DllImport(LibraryPath)]
        internal static extern unsafe int memhole_delete(void* memhole);

        [DllImport(LibraryPath)]
        internal static extern unsafe int memhole_connect(void* memhole);

        [DllImport(LibraryPath)]
        internal static extern unsafe int memhole_disconnect(void* memhole);

        [DllImport(LibraryPath)]
        internal static extern unsafe long memhole_attach_to_pid(void* memhole, int pid);

        [DllImport(LibraryPath)]
        internal static extern unsafe long memhole_attach_secondary_pid(void* memhole, int pid);

        [DllImport(LibraryPath)]
        internal static extern unsafe long memhole_attach_to_pids(void* memhole, int pid1, int pid2);

        [DllImport(LibraryPath)]
        internal static extern unsafe long memhole_set_mem_pos(void* memhole, long pos, int mode);

        [DllImport(LibraryPath)]
        internal static extern unsafe long memhole_set_buffer_size(void* memhole, long len);

        [DllImport(LibraryPath)]
        internal static extern unsafe long memhole_read(void* memhole, byte* buffer, long len, int mode);

        [DllImport(LibraryPath)]
        internal static extern unsafe long memhole_write(void* memhole, byte* buffer, long len, int mode);
    }
}
