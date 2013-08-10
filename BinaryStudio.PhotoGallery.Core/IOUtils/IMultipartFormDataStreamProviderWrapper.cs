using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Core.IOUtils
{
    public interface IMultipartFormDataStreamProviderWrapper
    {
        Stream GetStream(HttpContent parent, HttpContentHeaders headers);
        string GetLocalFileName(HttpContentHeaders headers);
        Task ExecutePostProcessingAsync();

        Collection<HttpContent> Contents { get; }
        NameValueCollection FormData { get; }
        Collection<MultipartFileData> FileData { get; }
    }
}