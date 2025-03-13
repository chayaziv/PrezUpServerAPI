
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using PrezUp.Core.IServices;
using PrezUp.Core.Utils;

namespace PrezUp.Service.Services
{
    public class AudioUploadService : Is3Service
    {
        private readonly IConfiguration _configuration;
        private readonly string _bucketName;
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly RegionEndpoint _region = RegionEndpoint.EUNorth1;

        public AudioUploadService(IConfiguration configuration)
        {
            _configuration = configuration;
            _accessKey = _configuration["AWS:AccessKey"];
            _secretKey = _configuration["AWS:SecretKey"];
            _bucketName = _configuration["AWS:BucketName"];
            _region= RegionEndpoint.EUNorth1;

            if (string.IsNullOrEmpty(_accessKey) || string.IsNullOrEmpty(_secretKey))
            {
                throw new InvalidOperationException("AWS credentials are not set in the environment variables.");
            }
        }

        public async Task<Result<S3Data>> UploadFileToS3Async(Stream fileStream, string objectKey)
        {
            try
            {
                using var s3Client = new AmazonS3Client(_accessKey, _secretKey, _region);
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = fileStream,
                    BucketName = _bucketName,
                    Key = objectKey,
                    ContentType = "audio/wav"
                };

                var fileTransferUtility = new TransferUtility(s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);

                return Result<S3Data>.Success(new S3Data() { Url = $"https://{_bucketName}.s3.amazonaws.com/{objectKey}" });
            }
            catch (Exception ex)
            {

                return Result<S3Data>.BadRequest($"Error deleting file from S3: {ex.Message}");
            }

        }

        public async Task<Result<S3Data>> DeleteFileFromS3Async(string fileUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUrl))
                {
                    throw new ArgumentException("Invalid file URL.");
                }

               
                Uri uri = new Uri(fileUrl);
                string objectKey = uri.AbsolutePath.TrimStart('/');

                using var s3Client = new AmazonS3Client(_accessKey, _secretKey, _region);
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = objectKey
                };

                await s3Client.DeleteObjectAsync(deleteObjectRequest);
                return Result<S3Data>.SuccessNoContent();
            }
            catch (Exception ex)
            {
              
                return Result<S3Data>.BadRequest($"Error deleting file from S3: {ex.Message}");
            }
        }
    }
}
