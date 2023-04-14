using System.Diagnostics;
using Xunit.Abstractions;

namespace KernelInteropTests
{
    public class BasicTests : IDisposable
    {
        private readonly IMemholeDevice _memhole;
        private readonly ITestOutputHelper output;

        private const long BufferSize = 1024L * 1024L * 1024L * 16L;
        private readonly unsafe byte* _buffer = (byte*)0x7fc492c17010;
        private const int Pid = 430366;

        public unsafe BasicTests(ITestOutputHelper output)
        {
            this.output = output;
            _memhole = new MemholeDevice();
            //_process.Start();

            //var buffer = new char[1024];

            // if (!long.TryParse(buffer.ToString(), out var longBuffer))
            // {
            //     Assert.Fail("Could not read buffer location");
            // }
            // _buffer = (byte*)longBuffer;

            try
            {
                _memhole.Connect();
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to connect to memhole device: " + e.Message);
            }

            try
            {
                _memhole.AttachToPid(Pid);
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to attach to process: " + e.Message);
            }

            try
            {
                _memhole.SetMemoryPosition((long)_buffer);
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to set memory position: " + e.Message);
            }
        }

        public void Dispose()
        {
            _memhole.Disconnect();
            _memhole.Dispose();
            //_process.Kill();
        }

        [Fact]
        public void CanRead()
        {
            var result = _memhole.Read(1024);
            output.WriteLine(Convert.ToHexString(result));
            Assert.Fail("Test failed");
            Assert.Equal(1024, result.Length);
        }

        [Fact]
        public unsafe void CanReadFrom()
        {
            var result = _memhole.ReadFrom((long)_buffer, 1024);
            output.WriteLine(Convert.ToHexString(result));
            Assert.Equal(1024, result.Length);
        }

        //[Fact]
        public void CanWrite()
        {
            var data = new byte[1024];
            var result = _memhole.Write(data);
            Assert.Equal(1024, result);
        }

        //[Fact]
        public unsafe void CanWriteFrom()
        {
            var data = new byte[1024];
            var result = _memhole.WriteTo((long)_buffer + 4096L, data);
            Assert.Equal(1024, result);
        }

        [Fact]
        public unsafe void CanSetMemoryPosition()
        {
            var result = _memhole.SetMemoryPosition((long)_buffer + 2048L);
            Assert.Equal((long)_buffer + 2048L, result);
        }
    }
}