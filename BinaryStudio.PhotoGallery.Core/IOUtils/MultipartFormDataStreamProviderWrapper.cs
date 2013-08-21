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
        private readonly MultipartFormDataStreamProvider multipartFormDataStreamProvider;
        
        public MultipartFormDataStreamProviderWrapper(string rootPath) : base(rootPath)
        {
            multipartFormDataStreamProvider = new MultipartFormDataStreamProvider(rootPath);
        }

        public MultipartFormDataStreamProviderWrapper(string rootPath, int bufferSize) : base(rootPath, bufferSize)
        {
            multipartFormDataStreamProvider = new MultipartFormDataStreamProvider(rootPath, bufferSize);
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            return multipartFormDataStreamProvider.GetStream(parent, headers);
        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return multipartFormDataStreamProvider.GetLocalFileName(headers);
        }

        public new Collection<HttpContent> Contents {
            get
            {
                return multipartFormDataStreamProvider.Contents;
            }
        }

        public override Task ExecutePostProcessingAsync()
        {
            return multipartFormDataStreamProvider.ExecutePostProcessingAsync();
        }

        public new NameValueCollection FormData {
            get
            {
                return multipartFormDataStreamProvider.FormData;
            }
        }

        public new Collection<MultipartFileData> FileData {
            get
            {
                return multipartFormDataStreamProvider.FileData;
            }
        }
    }
}
