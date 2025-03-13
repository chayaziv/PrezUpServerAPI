using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Utils;

namespace PrezUp.Core.IServices
{
    public interface Is3Service
    {
        public Task<Result<S3Data>> UploadFileToS3Async(Stream fileStream, string objectKey);
        public  Task<Result<S3Data>> DeleteFileFromS3Async(string fileUrl);
    }
}
