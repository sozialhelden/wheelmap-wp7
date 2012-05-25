using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;

namespace SharpGIS
{
	internal class HttpRequestStream : Stream
	{
		MemoryStream stream;
		public HttpRequestStream(MemoryStream stream)
		{
			this.stream = stream;
		}
		public override bool CanRead
		{
			get { return false; }
		}

		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override void Flush()
		{
			stream.Flush();
		}

		public override long Length
		{
			get { return stream.Length; }
		}

		public override long Position
		{
			get
			{
				return stream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			stream.Write(buffer, offset, count);
		}
	}
}
