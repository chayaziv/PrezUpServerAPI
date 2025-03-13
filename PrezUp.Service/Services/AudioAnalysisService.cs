using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IServices;
using PrezUp.Core.models;

namespace PrezUp.Service.Services
{
    public class AudioAnalysisService : IAudioAnalysisService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        

        public AudioAnalysisService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AudioResult> AnalyzeAudioAsync(string fileUrl)
        {
           
            using var client = _httpClientFactory.CreateClient();

          
            using var responseStream = await client.GetStreamAsync(fileUrl);
            using var content = new MultipartFormDataContent
    {
        { new StreamContent(responseStream), "audio", "temp_audio.wav" }
    };
            try
            {
                var response = await client.PostAsync("http://localhost:5000/analyze-audio", content);

                if (!response.IsSuccessStatusCode)
                {
                    return new AudioResult { Succeeded = false, Errors = { "errors in NLP server, failed to analyze audio" } };
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var cleanedContent = responseContent.Replace("```json", "").Replace("```", "").Trim();
                JObject jsonObject = JObject.Parse(cleanedContent);

                return ParseAnalysisResult(jsonObject);
            }
            catch (Exception e)
            {
                return new AudioResult { Succeeded = false, Errors = { e.Message } };
            }
        }

        private AudioResult ParseAnalysisResult(JObject jsonObject)
        {
            var result = new PresentationDTO
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
            
            return new AudioResult { Succeeded = true, analysis = result };
        }
    }
}
