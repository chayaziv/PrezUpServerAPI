using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IServices;
using PrezUp.Core.models;
using PrezUp.Core.Utils;
using Microsoft.Extensions.Configuration;

namespace PrezUp.Service.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;


        public AnalysisService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Result<Analysis>> AnalyzeAudioAsync(string fileUrl)
        {

            using var client = _httpClientFactory.CreateClient();


            using var responseStream = await client.GetStreamAsync(fileUrl);
            using var content = new MultipartFormDataContent
    {
        { new StreamContent(responseStream), "audio", "temp_audio.wav" }
    };
            try
            {
                var response = await client.PostAsync(_configuration["AnalysisApi:Url"], content);
                Console.WriteLine(  "!!!!!!!!!!!!!!!!!!!\n\n\n"+response+"\n\n\n!!!!!!!!!!!!!!!!!!!!!");
                if (!response.IsSuccessStatusCode)
                {

                    return Result<Analysis>.Failure("failed to analyze audio ");
                }
                Console.WriteLine("after NLP\n\n");
                var responseContent = await response.Content.ReadAsStringAsync();

                int jsonStartIndex = responseContent.IndexOf("```json");

                if (jsonStartIndex != -1)
                {

                    responseContent = responseContent.Substring(jsonStartIndex + 7);
                }


                var cleanedContent = responseContent.Replace("```", "").Trim();


                JObject jsonObject = JObject.Parse(cleanedContent);

                var analysis = ParseAnalysisResult(jsonObject);
                return Result<Analysis>.Success(analysis);
            }
            catch (Exception e)
            {
                Console.WriteLine("erorrrrrrrrrrrrrrrrrrrrrrrrrrrrr\n\n");

                return Result<Analysis>.Failure($"Error {e.Message}");
            }
        }

        private Analysis ParseAnalysisResult(JObject jsonObject)
        {
            var analysis = new Analysis
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

            analysis.Score = (analysis.Clarity + analysis.Fluency + analysis.Confidence + analysis.Engagement + analysis.SpeechStyle) / 5;

            return analysis;
        }
    }
}
