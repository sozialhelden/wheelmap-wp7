using System;
using System.Threading;

namespace SharpGIS
{
	internal class GZipClientAsyncResult : IAsyncResult
	{
		internal Exception Error { get; set; }
		internal System.IO.Stream Result { get; set; }
		internal AsyncCallback Callback { get; set; }
		public void Complete()
		{
			IsCompleted = true;
			(AsyncWaitHandle as ManualResetEvent).Set();
		}

		#region IAsyncResult Members

		/// <summary>
		/// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
		/// </summary>
		/// <value></value>
		/// <returns>A user-defined object that qualifies or contains information about an asynchronous operation.</returns>
		public object AsyncState { get; internal set; }

		/// <summary>
		/// Gets a <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
		/// </summary>
		/// <value></value>
		/// <returns>A wait handle that is used to wait for an asynchronous operation to complete.</returns>
		public System.Threading.WaitHandle AsyncWaitHandle { get; internal set; }

		/// <summary>
		/// Gets a value that indicates whether the asynchronous operation completed synchronously.
		/// </summary>
		/// <value></value>
		/// <returns>true if the asynchronous operation completed synchronously; otherwise, false.</returns>
		public bool CompletedSynchronously { get; internal set; }

		/// <summary>
		/// Gets a value that indicates whether the asynchronous operation has completed.
		/// </summary>
		/// <value></value>
		/// <returns>true if the operation is complete; otherwise, false.</returns>
		public bool IsCompleted { get; private set; }

		#endregion
	}
}
