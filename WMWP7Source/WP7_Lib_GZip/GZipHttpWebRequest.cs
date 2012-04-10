using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SharpGIS.ZLib;

namespace SharpGIS
{
	internal class GzipHttpWebRequest : HttpWebRequest
	{
		private Uri uri;
		private GZipClientAsyncResult asyncState;
		private Stream WriteStream;
		private string headerString;
		private MemoryStream body;
		private Socket _socket = null;
		private SocketAsyncEventArgs args;
		private static ManualResetEvent _clientDone = new ManualResetEvent(false);
		private const int TIMEOUT_MILLISECONDS = 5000;
		private const int MAX_BUFFER_SIZE = 2048;
		private bool isBusy = false;
		
		public GzipHttpWebRequest(Uri address)
			: base()
		{
			this.uri = address;
		}

		/// <summary>
		/// Begins an asynchronous request for a <see cref="T:System.IO.Stream"/> object to use to write data.
		/// </summary>
		/// <param name="callback">The <see cref="T:System.AsyncCallback"/> delegate.</param>
		/// <param name="state">The state object for this request.</param>
		/// <returns>
		/// An <see cref="T:System.IAsyncResult"/> that references the asynchronous request.
		/// </returns>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.HttpWebRequest.Method"/> property is GET-or- The <see cref="P:System.Net.WebRequest.ContentLength"/> property was set to a value that does not match the size of the provided request body.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is being used by a previous call to <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)"/>-or- The thread pool is running out of threads. </exception>
		/// <exception cref="T:System.NotImplementedException">This method is not implemented. </exception>
		/// <exception cref="T:System.NotSupportedException">The request cache validator indicated that the response for this request can be served from the cache; however, requests that write data must not use the cache. This exception can occur if you are using a custom cache validator that is incorrectly implemented. </exception>
		/// <exception cref="T:System.Net.WebException">
		/// 	<see cref="M:System.Net.HttpWebRequest.Abort"/> was previously called. </exception>
		/// <exception cref="T:System.ObjectDisposedException">In a .NET Framework application, a request stream with zero content length was not obtained and closed correctly.</exception>
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			var asyncState = new GZipClientAsyncResult()
			{
				Callback = callback,
				AsyncState = state,
				AsyncWaitHandle = _clientDone
			};
			System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() => asyncState.Callback(asyncState as IAsyncResult));
			return this.asyncState;
		}
		/// <summary>
		/// Begins an asynchronous request to an Internet resource.
		/// </summary>
		/// <param name="callback">The <see cref="T:System.AsyncCallback"/> delegate</param>
		/// <param name="state">The state object for this request.</param>
		/// <returns>
		/// An <see cref="T:System.IAsyncResult"/> that references the asynchronous request for a response.
		/// </returns>
		/// <exception cref="T:System.InvalidOperationException">The stream is already in use by a previous call to <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)"/>-or- The thread pool is running out of threads. </exception>
		/// <exception cref="T:System.NotImplementedException">This method is not implemented. </exception>
		/// <exception cref="T:System.NotSupportedException">The <paramref name="callback"/> parameter is null. </exception>
		/// <exception cref="T:System.Net.ProtocolViolationException">
		/// 	<see cref="P:System.Net.HttpWebRequest.Method"/> is GET. </exception>
		/// <exception cref="T:System.Net.WebException">
		/// 	<see cref="M:System.Net.HttpWebRequest.Abort"/> was previously called. </exception>
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			this.asyncState = new GZipClientAsyncResult()
			{
				Callback = callback,
				AsyncState = state,
				AsyncWaitHandle = _clientDone
			};
			var socketEventArg = SetUpSocket();
			string method = Method ?? (WriteStream == null ? "GET" : "POST");
			var length = (WriteStream == null ? 0 : WriteStream.Length);
			var stream = this.WriteRequestHeader(WriteStream, length, method);
			if (WriteStream != null)
				this.WriteRequestBody(WriteStream, stream);
			this.FlushToSocket(socketEventArg, stream);
			return this.asyncState;
		}
		/// <summary>
		/// Gets the original Uniform Resource Identifier (URI) of the request.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Uri"/> that contains the URI of the Internet resource passed to the <see cref="M:System.Net.WebRequest.Create(System.Uri)"/> method.</returns>
		/// <exception cref="T:System.NotImplementedException">This property is not implemented. </exception>
		public override Uri RequestUri
		{
			get { return uri;}
		}

		internal WebHeaderCollection ResponseHeaders { get; private set; }
		/// <summary>
		/// Gets or sets the value of the Content-type HTTP header.
		/// </summary>
		/// <value></value>
		/// <returns>The value of the Content-type HTTP header. The default value is null.</returns>
		public override string ContentType
		{
			get
			{
				return ResponseHeaders["Content-Type"];
			}
			set
			{
				base.ContentType = value;
			}
		}
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			WriteStream = new MemoryStream();
			return new HttpRequestStream(WriteStream as MemoryStream);
		}
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			return base.EndGetResponse(asyncResult);
		}
		public override void Abort()
		{
			base.Abort();
		}

		private SocketAsyncEventArgs SetUpSocket()
		{
			if (isBusy) throw new InvalidOperationException("Already downloading!");

			isBusy = true;

			string result = string.Empty;
			ResponseHeaders = null;
			body = null;
			// Create DnsEndPoint. The hostName and port are passed in to this method.
			DnsEndPoint hostEntry = new DnsEndPoint(uri.Host, uri.Port);
			// Create a stream-based, TCP socket using the InterNetwork Address Family. 
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			// Create a SocketAsyncEventArgs object to be used in the connection request
			SocketAsyncEventArgs socketEventArg = args = new SocketAsyncEventArgs();
			socketEventArg.RemoteEndPoint = hostEntry;

			// Inline event handler for the Completed event.
			// Note: This even handler was implemented inline in order to make this method self-contained.
			EventHandler<SocketAsyncEventArgs> handler = null;
			handler = new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
			{
				socketEventArg.Completed -= handler;
				if (e.SocketError == SocketError.Success)
				{
					socketEventArg.Completed += socketEventArg_Completed;
					_socket.ReceiveAsync(socketEventArg);
				}
				else
				{
					EndRequestError(new Exception(e.SocketError.ToString()));
					return;
				}
				_clientDone.Set();
			});
			socketEventArg.Completed += handler;
			// Sets the asyncState of the event to nonsignaled, causing threads to block
			_clientDone.Reset();
			return socketEventArg;
		}
		private Stream WriteRequestHeader(Stream WriteStream, long length = 0, string method = "GET")
		{
			string path = uri.AbsoluteUri;
			MemoryStream stream = new MemoryStream();
			StreamWriter wr = new StreamWriter(stream);
			wr.NewLine = "\r\n";
			wr.WriteLine(method + " " + path + " HTTP/1.0");

			foreach (var key in Headers.AllKeys)
				wr.WriteLine("{0}: {1}", key, Headers[key]);
			if (Headers[HttpRequestHeader.Accept] == null)
				wr.WriteLine("Accept: text/html, application/xhtml+xml, */*");
			if (Headers[HttpRequestHeader.AcceptLanguage] == null)
				wr.WriteLine("Accept-Language: en-US");
			if (Headers[HttpRequestHeader.UserAgent] == null)
			{
				string UserAgent = "Mozilla/5.0 (compatible; MSIE9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0; Microsoft Corporation)";
				wr.WriteLine("User-Agent: {0}", UserAgent);
			}
			if(Headers[HttpRequestHeader.AcceptEncoding] == null)
				wr.WriteLine("Accept-Encoding: gzip");
			if (Headers[HttpRequestHeader.Host] == null)
				wr.WriteLine("Host: {0}", uri.Host);
			if (Headers[HttpRequestHeader.ContentLength] != null)
				throw new ArgumentException("ContentLength header not supported");
			wr.WriteLine("Content-Length: {0}", length);
			if (Headers[HttpRequestHeader.KeepAlive] == null)
				wr.WriteLine("Connection: Keep-Alive");

			wr.WriteLine(); //Header ends with two linebreaks

			wr.Flush();
			return stream;
		}
		private void WriteRequestBody(Stream body, Stream socketStream)
		{
			body.Seek(0, SeekOrigin.Begin);
			for (int i = 0; i < body.Length; i++)
				socketStream.WriteByte((byte)body.ReadByte());
		}

		private void FlushToSocket(SocketAsyncEventArgs socketEventArg, Stream stream)
		{
			stream.Seek(0, SeekOrigin.Begin);
			byte[] buffer = new byte[stream.Length];
			stream.Read(buffer, 0, buffer.Length);
			socketEventArg.SetBuffer(buffer, 0, (int)stream.Length);
			_clientDone.Reset();
			_socket.ConnectAsync(socketEventArg);
			stream.Close();
			System.Diagnostics.Debug.WriteLine(System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length));
		}

		private void socketEventArg_Completed(object sender, SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				if (e.BytesTransferred == 0)
				{
					EndRequestSuccess();
					return;
				}
				string stringVal = System.Text.Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
				// Retrieve the data from the buffer
				if (ResponseHeaders == null)
				{
					string response = System.Text.Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
					headerString += response.Trim('\0');
					var idx = headerString.IndexOf("\r\n\r\n");
					if (idx > -1) //We have reached the body
					{
						ResponseHeaders = new WebHeaderCollection();
						var headerPart = headerString.Substring(0, idx);
						var headerParts = headerPart.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
						foreach (var part in headerParts)
						{
							var idx2 = part.IndexOf(":");
							if (idx2 > -1)
							{
								var key = part.Substring(0, idx2).Trim();
								var value = part.Substring(idx2 + 1).Trim();
								ResponseHeaders[key] = value;
							}
						}
						if (ResponseHeaders["Location"] != null) //Content moved
						{
							string newLocation = ResponseHeaders["Location"];
							_socket.Close();
							isBusy = false;
							this.uri = new Uri(newLocation, UriKind.Absolute);
							//TODO
							EndRequestError(new NotSupportedException("Redirection not supported"));
							return;
						}
						int length = 0;
						if (ResponseHeaders["Transfer-Encoding"] != null)
						{
							var transferEncoding = ResponseHeaders["Transfer-Encoding"];
							if (transferEncoding == "Chunked")
								EndRequestError(new NotSupportedException("Chunked transfer encoding not supported"));
							return;
						}
						body = new MemoryStream();
						var start = response.IndexOf("\r\n\r\n") + 4;
						length = Math.Max(e.Buffer.Length - start, length);
						byte[] bodyStart = new byte[length];
						for (int i = 0; i < bodyStart.Length; i++)
						{
							bodyStart[i] = e.Buffer[i + start];
						}
						WriteResponseBody(bodyStart);
					}
				}
				else if (body != null)
				{
					WriteResponseBody(e.Buffer);
				}
				if (isBusy)
					_socket.ReceiveAsync(args);
			}
			else
			{
				EndRequestError(new Exception(e.SocketError.ToString()));
			}
		}

		private void WriteResponseBody(byte[] buffer)
		{
			int contentlength = int.MaxValue;
			if (ResponseHeaders[HttpRequestHeader.ContentLength] != null)
				contentlength = int.Parse(ResponseHeaders[HttpRequestHeader.ContentLength]);
			int length = buffer.Length;
			if (contentlength < body.Length + length)
			{
				body.Write(buffer, 0, contentlength - (int)body.Length);
				EndRequestSuccess();
			}
			else
				body.Write(buffer, 0, length);
		}

		private void EndRequestError(Exception ex)
		{
			isBusy = false;
			_clientDone.Set();
			_socket.Close();
			this.asyncState.Error = ex;
			this.asyncState.Callback(this.asyncState as IAsyncResult);
			_clientDone.Set();
		}

		private void EndRequestSuccess()
		{
			_clientDone.Set();
			_socket.Close();
			isBusy = false;
			body.Seek(0, SeekOrigin.Begin);
			Stream result = body;
			if (ResponseHeaders[HttpRequestHeader.ContentEncoding] != null)
			{
				string encoding = ResponseHeaders[HttpRequestHeader.ContentEncoding];
				if (encoding == "gzip")
				{
                    GZipStream s = new GZipStream(body);
					MemoryStream ms = new MemoryStream();
					try
					{
						var b = s.ReadByte();
						while (b > -1)
						{
							ms.WriteByte((byte)b);
							b = s.ReadByte();
						}
					}
					catch { } //Will fail when we get to end of gzip stream - probably a more elegant way than this
					ms.Seek(0, SeekOrigin.Begin);
					result = ms;
				}
				//else if (encoding == "deflate")
				//{
				//	TODO
				//}
				else
				{
					this.asyncState.Error = new InvalidOperationException("Content-Encoding '" + encoding + "' not supported");
					this.asyncState.Callback(this.asyncState as IAsyncResult);
					return;
				}
			}
			this.asyncState.Result = result;
			this.asyncState.Complete();
			this.asyncState.Callback(this.asyncState as IAsyncResult);
		}
	}
}
