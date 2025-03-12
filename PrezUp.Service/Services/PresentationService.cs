using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Transfer;
using Amazon.S3;
using Amazon;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IRepositories;
using PrezUp.Core.IServices;
using PrezUp.Core.models;

using Microsoft.Extensions.Configuration;


namespace PrezUp.Service.Services
{
    public class PresentationService : IPresentationService
    {
        readonly IRepositoryManager _repository;
        private readonly IHttpClientFactory _httpClientFactory;
        readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        
        public PresentationService(IRepositoryManager repository, IHttpClientFactory httpClientFactory, IMapper mapper, IConfiguration configuration)
        {
            _repository = repository;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<AudioResult> AnalyzeAudioAsync(IFormFile audio, bool isPublic, int userId)
        {
            var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "temp_audio.wav");

            // שמירת קובץ האודיו
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await audio.CopyToAsync(stream);
            }
            string fileUrl = string.Empty;
            try
            {
                fileUrl = await UploadFileToS3Async(tempFilePath, "my-audio-files-presentations", "recordings/" + Guid.NewGuid() + ".wav");
                
                // שליחה לשרת פייתון
                var audioResult = await SendAudioToNlpServerAsync(tempFilePath);
                if(audioResult.Succeeded)
                {
                    // עדכון הטבלה
                    await _repository.Presentations.SaveAnalysisAsync(audioResult.analysis, isPublic, userId,fileUrl);
                    int res = await _repository.SaveAsync();
                    if (res == 0)
                    {
                        return new AudioResult() { Succeeded = false, Errors = { "errors in save to database" } };
                    }
                    return audioResult;
                }
                else
                {
                    return audioResult;
                }
                    
            }
            finally
            {
                // מחיקת קובץ אחרי שליחה
                if (System.IO.File.Exists(tempFilePath))
                {
                    System.IO.File.Delete(tempFilePath);
                }
            }
        }
        
        private async Task<string> UploadFileToS3Async(string filePath, string bucketName, string objectKey)
        {

            //Env.Load();
            //// שליפת מפתחות ה-AWS מתוך משתני סביבה
            //var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            //var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");


            var accessKey = _configuration["AWS:AccessKey"];
            var secretKey = _configuration["AWS:SecretKey"];

            // אם אין ערכים במשתני סביבה, אפשר להרים חריגה או לספק ערכים ברירת מחדל
            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("AWS credentials are not set in the environment variables.");
            }

            // יצירת לקוח S3 עם המפתחות שנשלפו
            var s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.EUNorth1);
            var fileTransferUtility = new TransferUtility(s3Client);
            await fileTransferUtility.UploadAsync(filePath, bucketName, objectKey);

            return $"https://{bucketName}.s3.amazonaws.com/{objectKey}";
        }

        private async Task<AudioResult> SendAudioToNlpServerAsync(string filePath)
        {
            using var client = _httpClientFactory.CreateClient();

            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var content = new MultipartFormDataContent
    {
        {   new StreamContent(fileStream), "audio", "temp_audio.wav" }
    };
            HttpResponseMessage response;
            try
            {
                 response = await client.PostAsync("http://localhost:5000/analyze-audio", content);

                if (!response.IsSuccessStatusCode)
                {
                    return new AudioResult() { Succeeded = false, Errors = { "errors in NLP server,failed to analayze audio" } };
                }
            }
            catch(Exception e)
            {
                return new AudioResult() { Errors = { e.Message }, Succeeded = false };
            }
            

            var responseContent = await response.Content.ReadAsStringAsync();

            var cleanedContent = responseContent
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            if (!string.IsNullOrEmpty(cleanedContent))
            {
                JObject jsonObject = JObject.Parse(cleanedContent);
                Console.WriteLine("after SendAudioToNlpServerAsync" + jsonObject.ToString());
                var result = new AnalysisResult
                {
                    Clarity = (int?)jsonObject["scores"]["clarity"]["score"] ?? 0,
                    ClarityFeedback = (string)jsonObject["scores"]["clarity"]["reason"],
                    Fluency = (int?)jsonObject["scores"]["fluency"]["score"] ?? 0,
                    FluencyFeedback = (string)jsonObject["scores"]["fluency"]["reason"],
                    Confidence = (int?)jsonObject["scores"]["confidence"]["score"] ?? 0,
                    ConfidenceFeedback = (string)jsonObject["scores"]["confidence"]["reason"],
                    Engagement = (int?)jsonObject["scores"]["engagement"]["score"] ?? 0,
                    EngagementFeedback = (string)jsonObject["scores"]["engagement"]["reason"],
                    SpeechStyle = (int?)jsonObject["scores"]["speech_style"]["score"] ?? 0,
                    SpeechStyleFeedback = (string)jsonObject["scores"]["speech_style"]["reason"],
                    Tips = (string)jsonObject["tips"]
                };

                result.Score = (result.Clarity + result.Fluency + result.Confidence + result.Engagement + result.SpeechStyle) / 5;

                return new AudioResult() { Succeeded = true, analysis = result };
            }
            else
            {

                return new AudioResult() { Succeeded = false, Errors = { "Invalid JSON response from server." } };
            }
        }


        public async Task<List<PresentationDTO>> getallAsync()
        {
            var list = await _repository.Presentations.GetListAsync();
            var listDTOs = new List<PresentationDTO>();
            foreach (var item in list)
            {
                listDTOs.Add(_mapper.Map<PresentationDTO>(item));
            }
            return listDTOs;
        }

        public async Task<PresentationDTO> getByIdAsync(int id)
        {
            var item = await _repository.Presentations.GetByIdAsync(id);

            return _mapper.Map<PresentationDTO>(item);
        }

       

        
        public async Task<bool> deleteAsync(int id, int userId)
        {
            Presentation itemToDelete = await _repository.Presentations.GetByIdAsync(id);
            if (itemToDelete == null)
            {
                throw new KeyNotFoundException("Presentation not found");
            }

            // בדיקה שהמשתמש הנוכחי הוא הבעלים של הפרזנטציה
            if (itemToDelete.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this presentation");
            }

            _repository.Presentations.DeleteAsync(itemToDelete);
            await _repository.SaveAsync();
            return true;
        }



    }
}
