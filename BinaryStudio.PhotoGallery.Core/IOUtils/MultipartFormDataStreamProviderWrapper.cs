using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Core.IOUtils
{
    public class MultipartFormDataStreamProviderWrapper : MultipartFormDataStreamProvider, IMultipartFormDataStreamProviderWrapper
    {
        public MultipartFormDataStreamProviderWrapper(string rootPath) : base(rootPath)
        {
            throw new NotImplementedException();
        }

        public MultipartFormDataStreamProviderWrapper(string rootPath, int bufferSize) : base(rootPath, bufferSize)
        {
            throw new NotImplementedException();
        }

        public new Collection<HttpContent> Contents { get; private set; }

        public new Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            throw new NotImplementedException();
        }

        public new string GetLocalFileName(HttpContentHeaders headers)
        {
            throw new NotImplementedException();
        }

        public new Task ExecutePostProcessingAsync()
        {
            throw new NotImplementedException();
        }

        public new NameValueCollection FormData { get; private set; }
        public new Collection<MultipartFileData> FileData { get; private set; }
        public new string RootPath { get; private set; }
        public new int BufferSize { get; private set; }
    }
}
