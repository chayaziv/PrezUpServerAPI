using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using PrezUp.Core.IServices;

namespace PrezUp.Service.Services
{
    public class AudioUploadService : IAudioUpLoadService
    {
        private readonly IConfiguration _configuration;

        public AudioUploadService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public async Task<string> UploadFileToS3Async(string filePath, string objectKey)
        //{
        //    var accessKey = _configuration["AWS:AccessKey"];
        //    var secretKey = _configuration["AWS:SecretKey"];
        //    var bucketName = _configuration["AWS:BucketName"];
        //    if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
        //    {
        //        throw new InvalidOperationException("AWS credentials are not set in the environment variables.");
        //    }

        //    using var s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.EUNorth1);
        //    var fileTransferUtility = new TransferUtility(s3Client);
        //    await fileTransferUtility.UploadAsync(filePath, bucketName, objectKey);

        //    return $"https://{bucketName}.s3.amazonaws.com/{objectKey}";
        //}
        public async Task<string> UploadFileToS3Async(Stream fileStream, string objectKey)
        {
            var accessKey = _configuration["AWS:AccessKey"];
            var secretKey = _configuration["AWS:SecretKey"];
            var bucketName = _configuration["AWS:BucketName"];
            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("AWS credentials are not set in the environment variables.");
            }

            using var s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.EUNorth1);
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = fileStream,
                BucketName = bucketName,
                Key = objectKey,
                ContentType = "audio/wav"
            };

            var fileTransferUtility = new TransferUtility(s3Client);
            await fileTransferUtility.UploadAsync(uploadRequest);
            Console.WriteLine(  "_-----------------------------------------------------------------");
            return $"https://{bucketName}.s3.amazonaws.com/{objectKey}";
        }

    }
}
