using System;
using System.Collections.Generic;
using System.Text;

namespace Wayz.Memhole.Kernel
{
    internal enum ErrorCodes
    {
        /// <summary>
        /// Invalid Device Error (the device struct is invalid)
        /// 
        /// Error INValid DEVice
        /// </summary>
        EINVDEV = 4,

        /// <summary>
        /// Memhole Not Found Error (memhole was not found at /dev/memhole)
        /// 
        /// Error MEMhole Not Found
        /// </summary>
        EMEMHNF = 8,

        /// <summary>
        /// Memhole busy error (memhole is already in use by another process/thread)
        /// 
        /// Error MEMhole BuSY
        /// </summary>
        EMEMBSY = 16,

        /// <summary>
        /// Invalid PID error (the PID supplied to memhole was invalid)
        /// 
        /// Error INValid PID
        /// </summary>
        EINVPID = 32,

        /// <summary>
        /// Kernel Memory Allocation Error (the kernel failed to allocate memory for the kernel-level buffer)
        /// 
        /// Error Kernel MALlOC
        /// </summary>
        EKMALOC = 64,

        /// <summary>
        /// Unsupported Operation Error (the memhole version installed is likely older than the wrapper being used, and should be updated)
        ///
        /// Error UnSuPorted OPeratioN
        /// </summary>
        EUSPOPN = 128
    }

    internal enum MemholeParallelMode
    {
        SKMFAST = 1,
        SKMSAFE = 2,
        SKMSFNB = 4
    }

}
