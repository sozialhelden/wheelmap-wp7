using System;
using System.Net;
using System.Security;

namespace SharpGIS
{
	public class GZipWebClient : WebClient
	{
		[SecuritySafeCritical]
		public GZipWebClient()
			: base()
		{

		}
		protected override WebRequest GetWebRequest(Uri address)
		{
			return new GzipHttpWebRequest(address);
		}
		protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			return new GzipHttpWebResponse(request, result);
		}
	}
}
