using System;
using System.Text;

namespace DM.Services.Authentication.Implementation
{
    public class HashProviderException : Exception
    {
        public HashProviderException(byte[] bytes, Exception inner)
            : base($"Fail to compute hash for {Encoding.UTF8.GetString(bytes)}", inner)
        {
        }
    }
}