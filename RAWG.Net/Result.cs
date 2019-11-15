using Microsoft.Win32.SafeHandles;
using System;
using System.Net;
using System.Runtime.InteropServices;

namespace RAWG.Net
{
    public class Result : IDisposable
    {
        public DateTime Time { get; protected set; } = DateTime.Now;

        internal Response response;
        protected RAWGClient client;
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        private bool disposed = false;

        internal Result Initialize(HttpStatusCode status, RAWGClient client)
        {
            this.client = client;
            response = new Response(status);
            return this;
        }

        internal string GetValue(object _object)
        {
            if (_object is null ||
                (_object is string && string.IsNullOrWhiteSpace(_object as string)))
                return "N/A";
            return _object.ToString();
        }

        internal string GetValue(DateTime? time)
        {
            if (time is null)
                return "N/A";
            return time?.ToString("yyyy-MM-dd");
        }

        public override string ToString()
        {
            return $"[{Time.ToString("HH:mm:ss")}] {response.ToString()}";
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
                handle.Dispose();
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
