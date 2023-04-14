using System;
using System.Collections.Generic;
using System.Text;

namespace Wayz.Memhole.Kernel
{
    public class MemholeInvalidDeviceException : Exception { }
    public class MemholeDeviceNotFoundException : Exception { }
    public class MemholeDeviceBusyException : Exception { }
    public class MemholeInvalidPidException : Exception { }
    public class MemholeKernelMallocFailureException : Exception { }
    public class MemholeUnsupportedOperationException : Exception { }

}
