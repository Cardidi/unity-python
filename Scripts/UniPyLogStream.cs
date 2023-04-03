using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Exodrifter.UnityPython
{
    public class UniPyLogStream : Stream
    {
        
        private static void ThrowNotSupport() => throw new NotSupportedException("Do not support read operation.");
        
        private Action<string> m_logger;

        private Byte[] m_buf_swap = Array.Empty<Byte>();

        public UniPyLogStream(Action<string> logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            m_logger = logger;
        }
        
        
        public override void Flush() {}

        public override int Read(byte[] buffer, int offset, int count)
        {
            ThrowNotSupport();
            return -1;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            ThrowNotSupport();
            return -1;
        }

        public override void SetLength(long value)
        {
            ThrowNotSupport();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "offset should not be negative");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "count should not be negative");
            if (offset >= buffer.Length)
                throw new ArgumentException("offset is greater than length of buffer");
            if (offset + count > buffer.Length)
                throw new ArgumentException("offset + count is greater than length of buffer");

            Array.Resize(ref m_buf_swap, count);
            Array.Copy(buffer, offset, m_buf_swap, 0, count);
            var str = Encoding.UTF8.GetString(m_buf_swap);
            m_logger.Invoke(str);
        }

        public override bool CanRead => false;
        public override bool CanSeek  => false;
        public override bool CanWrite => true;
        public override long Length { get; }
        public override long Position { get => -1; set => ThrowNotSupport(); }
    }
}