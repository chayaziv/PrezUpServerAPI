using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrezUp.Core.IServices
{
    public interface IAudioUpLoadService
    {
        public Task<string> UploadFileToS3Async(Stream fileStream, string objectKey);
    }
}
