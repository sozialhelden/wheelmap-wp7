using System;
using System.IO;
using System.Net;

namespace SharpGIS
{
	internal class GzipHttpWebResponse : HttpWebResponse
	{
		GZipClientAsyncResult result;
		GzipHttpWebRequest request;
		/// <summary>
		/// Initializes a new instance of the <see cref="GzipHttpWebResponse"/> class.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="result">The result.</param>
		public GzipHttpWebResponse(WebRequest request, IAsyncResult result)
			: base()
		{
			if (!(request is GzipHttpWebRequest) || !(result is GZipClientAsyncResult))
				throw new ArgumentException();
			this.request = request as GzipHttpWebRequest;
			this.result = result as GZipClientAsyncResult;
		}
		/// <summary>
		/// Gets the headers that are associated with this response from the server.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection"/> that contains the 
		/// header information returned with the response.</returns>
		public override WebHeaderCollection Headers
		{
			get
			{
				return request.ResponseHeaders;
			}
		}

		/// <summary>
		/// Closes the response stream.
		/// </summary>
		public override void Close()
		{
			base.Close();
		}
		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged
		/// resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
		/// <summary>
		/// Gets the length of the content returned by the request.
		/// </summary>
		/// <value></value>
		/// <returns>The number of bytes returned by the request. Content length does not include header information.</returns>
		public override long ContentLength
		{
			get
			{
				if (result != null && result.Result != null)
					return result.Result.Length;
				if (request != null && request.ResponseHeaders[HttpRequestHeader.ContentLength] != null)
					return long.Parse(request.ResponseHeaders[HttpRequestHeader.ContentLength]);
				return 0;
			}
		}
		/// <summary>
		/// Gets the content type of the response.
		/// </summary>
		/// <value></value>
		/// <returns>A string that contains the content type of the response.</returns>
		public override string ContentType
		{
			get
			{
				if (request != null && request.ResponseHeaders[HttpRequestHeader.ContentType] != null)
					return request.ResponseHeaders[HttpRequestHeader.ContentType];
				return base.ContentType;
			}
		}
		/// <summary>
		/// Gets the stream that is used to read the body of the response from the server.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.IO.Stream"/> containing the body of the response.
		/// </returns>
		public override Stream GetResponseStream()
		{
			if (result.Error != null)
				throw result.Error;
			return result.Result;
		}
		/// <summary>
		/// Gets a value that indicates whether the <see cref="P:System.Net.WebResponse.Headers"/> 
		/// property is supported by the descendant class for the <see cref="T:System.Net.WebResponse"/>
		/// instance.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="P:System.Net.WebResponse.Headers"/> property is 
		/// supported by the <see cref="T:System.Net.HttpWebRequest"/> instance in the descendant
		/// class; otherwise, false.</returns>
		public override bool SupportsHeaders
		{
			get
			{
				return true;
			}
		}
	}
}
