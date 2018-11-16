using System;
using System.IO;
using System.Security.Cryptography;
using System.Web;

namespace FiveActionFiltersDemo.ActionFilters
{
    public class ETagFilter : MemoryStream
    {
        private HttpResponseBase _response = null;
        private HttpRequestBase _request;
        private Stream _filter = null;

        public ETagFilter(HttpResponseBase response, HttpRequestBase request)
        {
            _response = response;
            _request = request;
            _filter = response.Filter;
        }

        private string GetToken(Stream stream)
        {
            var checksum = new byte[0];
            checksum = MD5.Create().ComputeHash(stream);
            return Convert.ToBase64String(checksum, 0, checksum.Length);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var data = new byte[count];

            Buffer.BlockCopy(buffer, offset, data, 0, count);

            var token = GetToken(new MemoryStream(data));
            var clientToken = _request.Headers["If-None-Match"];

            if (token != clientToken)
            {
                _response.AddHeader("ETag", token);
                _filter.Write(data, 0, count);
            }
            else
            {
                _response.SuppressContent = true;
                _response.StatusCode = 304;
                _response.StatusDescription = "Not Modified";
                _response.AddHeader("Content-Length", "0");
            }
        }
    }
}