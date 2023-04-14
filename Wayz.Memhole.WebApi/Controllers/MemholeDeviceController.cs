using Microsoft.AspNetCore.Mvc;
using System;
using Wayz.Memhole.Kernel;

namespace MemholeApi.Controllers
{
    [ApiController]
    [Route("memhole")]
    public class MemholeDeviceController : ControllerBase
    {
        private readonly IMemholeDevice _memholeDevice;

        public MemholeDeviceController(IMemholeDevice memholeDevice)
        {
            _memholeDevice = memholeDevice;
        }

        [HttpPost("connect")]
        public IActionResult Connect()
        {
            _memholeDevice.Connect();
            return Ok();
        }

        [HttpPost("disconnect")]
        public IActionResult Disconnect()
        {
            _memholeDevice.Disconnect();
            return Ok();
        }

        [HttpPost("attachToPid")]
        public IActionResult AttachToPid(int pid)
        {
            _memholeDevice.AttachToPid(pid);
            return Ok();
        }

        [HttpPost("setMemoryPosition")]
        public IActionResult SetMemoryPosition(long position)
        {
            long newPosition = _memholeDevice.SetMemoryPosition(position);
            return Ok(newPosition);
        }

        [HttpGet("read")]
        public IActionResult Read(long len)
        {
            ReadOnlySpan<byte> data = _memholeDevice.Read(len);
            return Ok(data.ToArray());
        }

        [HttpGet("readFrom")]
        public IActionResult ReadFrom(long position, long len)
        {
            ReadOnlySpan<byte> data = _memholeDevice.ReadFrom(position, len);
            return Ok(data.ToArray());
        }

        [HttpPost("write")]
        public IActionResult Write([FromBody] ReadOnlySpan<byte> data)
        {
            long bytesWritten = _memholeDevice.Write(data);
            return Ok(bytesWritten);
        }

        [HttpPost("writeTo")]
        public IActionResult WriteTo(long position, [FromBody] ReadOnlySpan<byte> data)
        {
            long bytesWritten = _memholeDevice.WriteTo(position, data);
            return Ok(bytesWritten);
        }
    }
}
