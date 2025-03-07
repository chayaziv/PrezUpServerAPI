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
    public class PresentationService: IPresentationService
    {
        readonly IRepositoryManager _repository;
        private readonly IHttpClientFactory _httpClientFactory;
        //readonly IMapper _mapper;

        public PresentationService(IRepositoryManager repository)
        {
            _repository = repository;
            //_mapper = mapper;
        }
        public async Task<List<Presentation>> getallAsync()
        {
            var list = _repository.Presentations.GetList();
            //var listDTOs = new List<Presentation>();
            //foreach (var item in list)
            //{
            //    listDTOs.Add(_mapper.Map<Presentation>(item));
            //}
            await _repository.SaveAsync();
            return list;
        }

        public async Task<Presentation> getByIdAsync(int id)
        {
            var item =  _repository.Presentations.GetById(id);

            //return _mapper.Map<Presentation>(item);
            await _repository.SaveAsync();
            return item;
        }

        public async Task<Presentation> addAsync(Presentation agreement)
        {
            //var model = _mapper.Map<Agreement>(agreement);
            var model = agreement;
            _repository.Presentations.Add(model);

            await _repository.SaveAsync();
            //return _mapper.Map<Presentation>(model);
            return model;
        }

        public async Task<Presentation> updateAsync(int id, Presentation agreement)
        {
            //var model = _mapper.Map<Agreement>(agreement);
            var model = agreement;
            var updated = _repository.Presentations.Update(model);
            await _repository.SaveAsync();
            //return _mapper.Map<Presentation>(updated);
            return updated;
        }

        public async Task<bool> deleteAsync(int id)
        {
            Presentation itemToDelete = _repository.Presentations.GetById(id);
            _repository.Presentations.Delete(itemToDelete);
            await _repository.SaveAsync();
            return true;
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
                await _presentationRepository.SavePresentationAsync(analysisResult);

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
            var client = _httpClientFactory.CreateClient();
            var content = new MultipartFormDataContent
        {
            { new StreamContent(new FileStream(filePath, FileMode.Open)), "audio", "temp_audio.wav" }
        };

            var response = await client.PostAsync("http://localhost:5000/analyze-audio", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to analyze audio");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            JObject jsonObject = JObject.Parse(responseContent);

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
    }
}

