using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using PrezUp.Core.Entity;
using PrezUp.Core.IRepositories;
using PrezUp.Core.IServices;

namespace PrezUp.Service.Services
{
    public class PresentationService : IPresentationService
    {
        readonly IRepositoryManager _repository;
        private readonly IHttpClientFactory _httpClientFactory;
        //readonly IMapper _mapper;

        public PresentationService(IRepositoryManager repository, IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _httpClientFactory = httpClientFactory;
            //_mapper = mapper;
        }
        public async Task<AnalysisResult> AnalyzeAudioAsync(IFormFile audio)
        {
            var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "temp_audio.wav");

            // שמירת קובץ האודיו
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await audio.CopyToAsync(stream);
            }

            try
            {
                // שליחה לשרת פייתון
                var analysisResult = await SendAudioToNlpServerAsync(tempFilePath);

                // עדכון הטבלה
                await _repository.SavePresentationAsync(analysisResult);
                _repository.SaveAsync();
                return analysisResult;
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
        private async Task<AnalysisResult> SendAudioToNlpServerAsync(string filePath)
        {
            using var client = _httpClientFactory.CreateClient();

            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var content = new MultipartFormDataContent
    {
        {   new StreamContent(fileStream), "audio", "temp_audio.wav" }
    };

            var response = await client.PostAsync("http://localhost:5000/analyze-audio", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to analyze audio");
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

                return result;
            }
            else
            {
                throw new Exception("Invalid JSON response from server.");
            }
        }


        public async Task<List<PresentationDTO>> getallAsync()
        {
            var list = await _repository.Presentations.GetListAsync();
            //var listDTOs = new List<Presentation>();
            //foreach (var item in list)
            //{
            //    listDTOs.Add(_mapper.Map<Presentation>(item));
            //}
           
            return list;
        }

        public async Task<PresentationDTO> getByIdAsync(int id)
        {
            var item = await _repository.Presentations.GetByIdAsync(id);

            //return _mapper.Map<Presentation>(item);
            
            return item;
        }

        public async Task<PresentationDTO> addAsync(PresentationDTO agreement)
        {
            //var model = _mapper.Map<Agreement>(agreement);
            var model = agreement;
            await _repository.Presentations.AddAsync(model);

            await _repository.SaveAsync();
            //return _mapper.Map<Presentation>(model);
            return model;
        }

        public async Task<PresentationDTO> updateAsync(int id, PresentationDTO agreement)
        {
            //var model = _mapper.Map<Agreement>(agreement);
            var model = agreement;
            var updated =  _repository.Presentations.UpdateAsync(model);
            await _repository.SaveAsync();
            //return _mapper.Map<Presentation>(updated);
            return updated;
        }

        public async Task<bool> deleteAsync(int id)
        {
            PresentationDTO itemToDelete = await _repository.Presentations.GetByIdAsync(id);
            _repository.Presentations.DeleteAsync(itemToDelete);
            await _repository.SaveAsync();
            return true;
        }


    }
}

