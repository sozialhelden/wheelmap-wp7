using System;

namespace Sozialhelden.Wheelmap.Lib.DataAccess
{
    /// <summary>
    /// an Exception whil will fired if an api - key is invalid
    /// </summary>
    public class APIKeyException : Exception
    {
        public APIKeyException(): base()
        {

        }

        public APIKeyException(string message): base(message)
        {
            
        }
    }
}
