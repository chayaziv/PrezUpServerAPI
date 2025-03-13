using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IRepositories;
using PrezUp.Core.IServices;
using PrezUp.Core.models;

namespace PrezUp.Service.Services
{
    public class PresentationService : IPresentationService
    {

        private readonly IRepositoryManager _repository;
        private readonly IAudioUpLoadService _audioUploadService;
        private readonly IAudioAnalysisService _audioAnalysisService;
        readonly IMapper _mapper;

        public PresentationService(
            IRepositoryManager repository,
            IAudioUpLoadService audioUploadService,
            IAudioAnalysisService audioAnalysisService,
            IMapper mapper)
        {
            _repository = repository;
            _audioUploadService = audioUploadService;
            _audioAnalysisService = audioAnalysisService;
            _mapper = mapper;
        }   
        public async Task<AudioResult> AnalyzeAudioAsync(IFormFile audio, bool isPublic, int userId)
        {
            
            string fileKey = $"recordings/{Guid.NewGuid()}.wav";
            string fileUrl = await _audioUploadService.UploadFileToS3Async(audio.OpenReadStream(), fileKey);
            var audioResult = await _audioAnalysisService.AnalyzeAudioAsync(fileUrl);
       
            if (audioResult.Succeeded)
            {
                audioResult.analysis.FileUrl = fileUrl;
                audioResult.analysis.IsPublic = isPublic;
                audioResult.analysis.UserId = userId;
                var model = _mapper.Map<Presentation>(audioResult.analysis);
                 await _repository.Presentations.AddAsync(model);
                if (await _repository.SaveAsync() == 0)
                {
                    return new AudioResult { Succeeded = false, Errors = { "Error saving to database" } };
                }
                audioResult.analysis = _mapper.Map<PresentationDTO>(model);
            }

            return audioResult;
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
        public async Task<List<PresentationDTO>> GetPublicPresentationsAsync()
        {
            var list = await _repository.Presentations.GetPublicPresentationsAsync();
            var listDTOs = new List<PresentationDTO>();
            foreach (var item in list)
            {
                listDTOs.Add(_mapper.Map<PresentationDTO>(item));
            }
            return listDTOs;
        }
       
        public async Task<bool> deleteAsync(int id, int userId)
        {
            var itemToDelete = await _repository.Presentations.GetByIdAsync(id);
            if (itemToDelete == null)
            {
                throw new KeyNotFoundException("Presentation not found");
            }

            if (itemToDelete.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this presentation");
            }

          
            if (!string.IsNullOrEmpty(itemToDelete.FileUrl))
            {
                await _audioUploadService.DeleteFileFromS3Async(itemToDelete.FileUrl);
            }

           
            _repository.Presentations.DeleteAsync(itemToDelete);
              await _repository.SaveAsync();
            return true;
        }

    }
}
